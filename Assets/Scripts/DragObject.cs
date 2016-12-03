using System.Collections;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class DragObject : MonoBehaviour
{
    public float SnapDistance = 2.5f;
    public float ShrinkSpeed = 10;

    private GameObject _dragObject;
    private GameObject _scaleObject;
    private Transform _childTransform;

    private Draggable _draggable;

    private Vector3 _originalPosition;
    private Vector3 _originalScale;

    private Vector3 _childOriginalPosition;
    private Vector3 _childOriginalLocalPosition;

    private bool _reset;

    private LayerMask _raycastLayers;

    // ReSharper disable once UnusedMember.Local
    private void Start() => _raycastLayers = 1 << LayerMask.NameToLayer("MyRagdoll");

    // ReSharper disable once UnusedMember.Local
    private void Update()
    {
        if (_reset)
        {
            _dragObject.transform.position = Vector3.Lerp(_dragObject.transform.position, _originalPosition,
                                                          Time.deltaTime * ShrinkSpeed);
            _scaleObject.transform.localScale = Vector3.Lerp(_scaleObject.transform.localScale, _originalScale,
                                                             Time.deltaTime * ShrinkSpeed);
            _childTransform.position = Vector3.Lerp(_childTransform.position, _childOriginalPosition,
                                                    Time.deltaTime * ShrinkSpeed);
        }

        if (!Input.GetMouseButtonDown(0))
            return;

        var mainCamera = Camera.main;
        var mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Check if we hit an object in the scene, which is also draggable
        if (!Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit, Mathf.Infinity,
                             _raycastLayers) || !hit.transform.gameObject.CompareTag("Draggable"))
            return;

        // Our object of interest is actually the grand-parent of the hit object
        _dragObject = hit.transform.parent.parent.gameObject;

        // Remove the parent of the drag object
        _dragObject.transform.parent = null;

        // The hit object's parent tells us whether the object is attached or not
        _draggable = _dragObject.GetComponent<Draggable>();

        // We need this offset when moving the objects
        var offset = _dragObject.transform.position - mouseRay.GetPoint(hit.distance);
        
        // If the object is detached, we just drag it
        if (!_draggable.Attached)
        {
            StartCoroutine(DragDetachedObject(hit.distance, offset));
            return;
        }

        // Find the "scale" object in the hierarchy
        foreach (Transform child in _dragObject.transform)
        {
            if (!child.name.Contains("Scale"))
                continue;

            _scaleObject = child.gameObject;
            break;
        }

        // Save original position and scale
        _originalPosition = _dragObject.transform.position;
        _originalScale = _scaleObject.transform.localScale;

        // The second child contains the child limbs of this limb
        _childTransform = _dragObject.transform.GetChild(1);
        _childOriginalPosition = _childTransform.position;
        _childOriginalLocalPosition = _childTransform.localPosition;

        // Start dragging th object
        StartCoroutine(DragAttachedObject(hit.distance, offset));
    }

    private IEnumerator DragAttachedObject(float distance, Vector3 offset)
    {
        while (Input.GetMouseButton(0))
        {
            _reset = false;

            var screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Distance between the hit point and object's original position
            var dist = screenRay.GetPoint(distance) + offset - _originalPosition;
            var distMagnitude = dist.magnitude;

            // Only do modifications when we drag diagonally and increasing
            if (dist.x > 0 && dist.z > 0)
            {
                // Increase the scale
                _scaleObject.transform.localScale = Vector3.one + Vector3.up * distMagnitude -
                                                    Vector3.forward * distMagnitude / 10;

                // Move the child object(s) to retain their relative positions
                _childTransform.position = _childOriginalPosition -
                                           _childTransform.up * distMagnitude;
            }
            else
                _scaleObject.transform.localScale = _originalScale;

            var snapped = distMagnitude > SnapDistance;

            if (distMagnitude > SnapDistance)
            {
                _draggable.Attached = false;
                _dragObject.transform.position = _originalPosition -
                                                 _dragObject.transform.up * distMagnitude;
                _scaleObject.transform.localScale = _originalScale;
                _childTransform.localPosition = _childOriginalLocalPosition;
            }

            if (snapped)
            {
//                _draggable.AddRigidBody();
                yield break;
            }

            yield return null;
        }

        // Reset everything
        _dragObject.transform.parent = _draggable.OriginalParent;
        _reset = true;
    }

    private IEnumerator DragDetachedObject(float distance, Vector3 offset)
    {
        while (Input.GetMouseButton(0))
        {
            var screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            _dragObject.transform.position = screenRay.GetPoint(distance) + offset;
            _childTransform.localPosition = _childOriginalLocalPosition;

            yield return null;
        }

//        _draggable.AddRigidBody();
    }
}