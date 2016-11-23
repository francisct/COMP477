using UnityEngine;

// ReSharper disable once CheckNamespace
public class DragJoint : MonoBehaviour
{
    // Candidates for public
    private float _speed = 10;
    private float _snapScale = 4;

    private GameObject _dragObject;
    private Draggable _draggable;
    private GameObject _scaleObject;

    private Camera _mainCamera;

    private Transform _childTransform;
    private Vector3 _childOriginalPosition;
    private Vector3 _childOriginalLocalPosition;

    private Vector3 _offset;
    private Vector3 _originalScale;
    private Vector3 _originalRotation;
    private Vector3 _originalPosition;
    private Vector3 _firstVector;
    private Vector3 _firstNormalized;
    private bool _reset;

    private const float Y_SCALE_MULTIPLIER = 5;
    private const float Z_SCALE_MULTIPLIER = 50;

    // ReSharper disable once UnusedMember.Local
    private void Start() => _mainCamera = Camera.main;

    // ReSharper disable once UnusedMember.Local
    private void Update()
    {
        if (!_reset)
            return;

        _scaleObject.transform.localScale = Vector3.Lerp(_scaleObject.transform.localScale, _originalScale,
                                                         Time.deltaTime * _speed);
        _childTransform.localPosition = Vector3.Lerp(_childTransform.localPosition, _childOriginalLocalPosition,
                                                     Time.deltaTime * _speed);
    }

    // ReSharper disable once UnusedMember.Local
    private void OnMouseDown()
    {
        // Our object of interest is actually the grand-parent of the hit object
        _dragObject = transform.parent.parent.gameObject;

        // Remove the parent of the drag object
//        _dragObject.transform.parent = null;

        // The hit object's parent tells us whether the object is attached or not
        _draggable = _dragObject.GetComponent<Draggable>();

        // Find the "scale" object in the hierarchy
        foreach (Transform child in _dragObject.transform)
        {
            if (!child.name.Contains("Scale"))
                continue;

            _scaleObject = child.gameObject;
            break;
        }

//        _originalRotation = _dragObject.transform.rotation.eulerAngles;
        _originalRotation = _dragObject.transform.parent.rotation.eulerAngles;
        _originalPosition = _dragObject.transform.position;
        _originalScale = _scaleObject.transform.localScale;

        _firstVector = transform.position - _originalPosition;
        _firstNormalized = _firstVector.normalized;

        // The second child contains the child limbs of this limb
        _childTransform = _dragObject.transform.GetChild(1);
        _childOriginalPosition = _childTransform.position;
        _childOriginalLocalPosition = _childTransform.localPosition;

        var worldMousePosition = CalculateMousePosition();

        // We need this offset when moving the objects
        _offset = _originalPosition - worldMousePosition;
    }

    private Vector3 CalculateMousePosition()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = transform.position.z - Camera.main.transform.position.z;
        var worldMousePosition = _mainCamera.ScreenToWorldPoint(mousePosition);
        worldMousePosition.z = transform.position.z;
        return worldMousePosition;
    }

    // ReSharper disable once UnusedMember.Local
    private void OnMouseDrag()
    {
        _reset = false;

        if (_draggable.Attached)
            DragAttachedObject();
        else
            DragDetachedObject();
    }

    private void DragDetachedObject()
    {
        var worldMousePosition = CalculateMousePosition();
        _dragObject.transform.position = worldMousePosition + _offset;
    }

    private void DragAttachedObject()
    {
        var worldMousePosition = CalculateMousePosition();

        //var secondVector = worldMousePosition - _originalPosition + _offset;
        var secondVector = worldMousePosition - _originalPosition;

        Debug.DrawRay(_originalPosition, _offset, Color.red);
        Debug.DrawRay(_originalPosition, _firstVector, Color.green);
        Debug.DrawRay(_originalPosition, secondVector, Color.blue);

        var secondNormalized = secondVector.normalized;
        var sine = _firstNormalized.x * secondNormalized.y - _firstVector.y * secondNormalized.x;
        var cosine = _firstNormalized.x * secondNormalized.x + _firstVector.y * secondNormalized.y;
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
            return;
        }

        // Scale up in 'y' direction and scale down in 'z' direction
        var currentScale = _scaleObject.transform.localScale;
        currentScale.y = _originalScale.y + (ratio - 1) / Y_SCALE_MULTIPLIER;
        currentScale.z = _originalScale.z - (ratio - 1) / Z_SCALE_MULTIPLIER;
        _scaleObject.transform.localScale = currentScale;

        _childTransform.localPosition = _childOriginalLocalPosition - Vector3.up * (ratio - 1) / Y_SCALE_MULTIPLIER;

        if (_scaleObject.transform.localScale.y <= _snapScale)
            return;

        _scaleObject.transform.localScale = _originalScale;
        _childTransform.localPosition = _childOriginalLocalPosition;
        _draggable.Attached = false;
    }

    // ReSharper disable once UnusedMember.Local
    private void OnMouseUp()
    {
        if (_draggable.Attached)
            _reset = true;
        else
            _draggable.AddRigidBody();
    }
}
