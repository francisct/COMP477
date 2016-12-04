using UnityEngine;

// ReSharper disable once CheckNamespace
public class DragJoint2 : MonoBehaviour
{
    // Candidates for public
    private float _fps = 15;
    private readonly float SnapScale = 6;

    private float _timer, _currentScaleValue, _originalScaleValue;

    private GameObject _dragObject;
    private Draggable _draggable;
    private GameObject _scaleObject;

    private Transform _childTransform;
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
    private void Start()
    {
        _fps = 1 / _fps;
    }

    // ReSharper disable once UnusedMember.Local
    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_reset && _timer <= 0 && Mathf.Abs(_currentScaleValue - _originalScaleValue) > 0.001f)
        {
            var diff = _currentScaleValue - _originalScaleValue;
            var inc = _currentScaleValue > _originalScaleValue ? 0.1f : -0.1f;
            _currentScaleValue = Mathf.Max(0.0001f, _originalScaleValue + -diff + inc);

            if (Mathf.Abs(_currentScaleValue - _originalScaleValue) < 0.05f)
                _currentScaleValue = _originalScaleValue;

            _scaleObject.transform.localScale = new Vector3(_originalScale.x, _currentScaleValue, _originalScale.z);
            _timer = _fps;
        }

        UpdateChild();
    }

    // ReSharper disable once UnusedMember.Local
    private void OnMouseDown()
    {
        _dragObject = transform.gameObject;
        _draggable = _dragObject.GetComponent<Draggable>();

        if (!_draggable.Attached)
            return;

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

        var worldMousePosition = _draggable.CalculateMousePosition();

        _offset = _originalPosition - worldMousePosition;

        _draggable.ModifyRigidBodies(_draggable.Attached);
    }

    // ReSharper disable once UnusedMember.Local
    private void OnMouseDrag()
    {
        if (!_draggable.Attached)
            return;

        _reset = false;
        DragAttachedObject();
    }

    // ReSharper disable once UnusedMember.Local
    private void OnMouseUp()
    {
        if (!_draggable.Attached)
            return;

        _draggable.ModifyRigidBodies(false);

        _currentScaleValue = _scaleObject.transform.localScale.y;
        _reset = true;
    }

    private void DragAttachedObject()
    {
        var worldMousePosition = _draggable.CalculateMousePosition();

        var secondVector = worldMousePosition - _originalPosition;

        Debug.DrawRay(_originalPosition, _offset, Color.red);
        Debug.DrawRay(_originalPosition, _firstVector, Color.green);
        Debug.DrawRay(_originalPosition, secondVector, Color.blue);

        var secondNormalized = secondVector.normalized;
        var sine = _firstNormalized.x * secondNormalized.y - _firstVector.y * secondNormalized.x;
        var cosine = _firstNormalized.x * secondNormalized.x + _firstVector.y * secondNormalized.y;
        var angle = Mathf.Atan2(sine, cosine) * Mathf.Rad2Deg;
        var rotation = _originalRotation - new Vector3(angle, 0, 0);
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

        if (_scaleObject.transform.localScale.y <= SnapScale)
            return;

        _draggable.ModifyRigidBodies(false);

        _scaleObject.transform.localScale = _originalScale;
        _childTransform.localPosition = _childOriginalLocalPosition;
        Destroy(_dragObject.GetComponent<HingeJoint>());
        _draggable.Attached = false;

        _dragObject.transform.SetParent(null);

        _draggable.CreateDetachedConfiguration();
    }

    private void UpdateChild()
    {
        if (_childTransform == null)
            return;

        _childTransform.localPosition = _childOriginalLocalPosition - Vector3.up *
                                        (_scaleObject.transform.localScale.y - 1);

    }
}
