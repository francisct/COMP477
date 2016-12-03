using System;
using System.Collections;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class RayCastNew : MonoBehaviour
{
    // Candidates for public
    private float _fps = 15;
    private float SnapScale = 6;
//    private float AttachDistance = 1.5f;

    private float _timer, _currentScaleValue, _originalScaleValue;

    private GameObject _dragObject;
    private Draggable _draggable;
    private GameObject _scaleObject;

    private Camera _mainCamera;

    private Transform _childTransform;
    private Vector3 _childOriginalLocalPosition;

    private Vector3 _offset;
    private Vector3 _originalScale;
    private Vector3 _originalRotation;
    private Vector3 _originalPosition;
    private Vector3 _firstVector;
    private Vector3 _firstNormalized;
    private bool _reset;
    private LayerMask _raycastLayers;

    private const float Y_SCALE_MULTIPLIER = 5;
    private const float Z_SCALE_MULTIPLIER = 50;

    // ReSharper disable once UnusedMember.Local
    private void Start()
    {
        _mainCamera = Camera.main;
        _fps = 1 / _fps;

        _raycastLayers = 1 << LayerMask.NameToLayer("MyRagdoll");
    }

    // ReSharper disable once UnusedMember.Local
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ModifyRigidBodies(true);

        if (!Input.GetMouseButtonDown(0))
            return;

        var mouseRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

        // Check if we hit an object in the scene, which is also draggable
        RaycastHit hit;
        if (!Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit, Mathf.Infinity,
                             _raycastLayers) || !hit.transform.gameObject.CompareTag("Joint"))
            return;

        _dragObject = hit.transform.gameObject;
        _draggable = _dragObject.GetComponent<Draggable>();

        // Find the "scale" object in the hierarchy
        foreach (Transform child in _dragObject.transform)
        {
            if (!child.name.Contains("Scale"))
                continue;

            _scaleObject = child.gameObject;
            break;
        }

        _originalRotation = _dragObject.transform.parent.rotation.eulerAngles;
        _originalPosition = _dragObject.transform.position;
        _originalScale = _scaleObject.transform.localScale;
        _originalScaleValue = _scaleObject.transform.localScale.y;

        _childTransform = _dragObject.transform.GetChild(1);
        _childOriginalLocalPosition = _childTransform.localPosition;

        _firstVector = _childTransform.position - _originalPosition;
        _firstNormalized = _firstVector.normalized;

        var worldMousePosition = CalculateMousePosition();

        // We need this offset when moving the objects
        if (_draggable.Attached)
            _offset = _originalPosition - worldMousePosition;
        else
            _offset = _dragObject.transform.position - worldMousePosition;

        StartCoroutine(_draggable.Attached
                           ? DragAttachedObject()
                           : DragDetachedObject());
    }

    private void ModifyRigidBodies(bool isKinematic)
    {
        var upperArm = GameObject.Find("LeftUpperArm");
        var lowerArm = GameObject.Find("LeftLowerArm");

        upperArm.GetComponent<Rigidbody>().isKinematic = isKinematic;
        lowerArm.GetComponent<Rigidbody>().isKinematic = isKinematic;

        upperArm.GetComponent<Rigidbody>().useGravity = !isKinematic;
        lowerArm.GetComponent<Rigidbody>().useGravity = !isKinematic;
    }

    private Vector3 CalculateMousePosition()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = transform.position.z - Camera.main.transform.position.z;
        var worldMousePosition = _mainCamera.ScreenToWorldPoint(mousePosition);
        worldMousePosition.z = transform.position.z;
        return worldMousePosition;
    }

    // ------------------- Trying ray-cast again ---------------------
    private IEnumerator DragDetachedObject()
    {
        print("DETACHED!!!");
        while (Input.GetMouseButton(0))
        {
            ModifyRigidBodies(true);

            var worldMousePosition = CalculateMousePosition();
            _dragObject.transform.position = worldMousePosition + _offset;

//            var distanceToParent =
//                    (_dragObject.transform.parent.position - _dragObject.transform.position)
//                            .sqrMagnitude;
//            if (distanceToParent >= AttachDistance * AttachDistance)
//                yield break;
//
//            _draggable.Attached = true;
//            _dragObject.transform.position = _draggable.OriginalPosition;
//            _dragObject.transform.rotation = _draggable.OriginalRotation;

            yield return null;
        }

        ModifyRigidBodies(false);
    }

    // ---------------------------------------------------------------

    private IEnumerator DragAttachedObject()
    {
        while (Input.GetMouseButton(0))
        {
            ModifyRigidBodies(true);

            var worldMousePosition = CalculateMousePosition();

            //var secondVector = worldMousePosition - _originalPosition + _offset;
            var secondVector = worldMousePosition - _originalPosition;

            Debug.DrawRay(_originalPosition, _offset, Color.red);
            Debug.DrawRay(_originalPosition, _firstVector, Color.green);
            Debug.DrawRay(_originalPosition, secondVector, Color.blue);

            var secondNormalized = secondVector.normalized;
            var sine = _firstNormalized.x * secondNormalized.y - _firstVector.y * secondNormalized.x;
            var cosine = _firstNormalized.x * secondNormalized.x +
                         _firstVector.y * secondNormalized.y;
            var angle = Mathf.Atan2(sine, cosine) * Mathf.Rad2Deg;
            var rotation = _originalRotation - new Vector3(angle, 0, 0);
            //        _dragObject.transform.rotation = Quaternion.Euler(rotation);
            _dragObject.transform.parent.rotation = Quaternion.Euler(rotation);

            // Ratio of length: vector pointing to mouse position to vector point from parent
            //      object to the object we're dragging
            var ratio = secondVector.sqrMagnitude / _firstVector.sqrMagnitude;
            if (ratio < 1)
            {
                _childTransform.localPosition = _childOriginalLocalPosition;
                ModifyRigidBodies(false);
                yield return null;
            }
            else
            {
                // Scale up in 'y' direction and scale down in 'z' direction
                var currentScale = _scaleObject.transform.localScale;
                currentScale.y = _originalScale.y + (ratio - 1) / Y_SCALE_MULTIPLIER;
                currentScale.z = _originalScale.z - (ratio - 1) / Z_SCALE_MULTIPLIER;
                _scaleObject.transform.localScale = currentScale;

                _childTransform.localPosition = _childOriginalLocalPosition -
                                                Vector3.up * (ratio - 1) / Y_SCALE_MULTIPLIER;

                if (_scaleObject.transform.localScale.y > SnapScale)
                {
                    _scaleObject.transform.localScale = _originalScale;
                    _childTransform.localPosition = _childOriginalLocalPosition;
                    ModifyRigidBodies(false);
                    Destroy(_dragObject.GetComponent<HingeJoint>());
                    _draggable.Attached = false;
                    yield break;
                }
            }

            yield return null;
        }

        _scaleObject.transform.localScale = _originalScale;
        _childTransform.localPosition = _childOriginalLocalPosition;
        ModifyRigidBodies(false);
    }
}