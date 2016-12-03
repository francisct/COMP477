using UnityEngine;

public class SimpleMouseDrag : MonoBehaviour
{
    internal Draggable Draggable;
    internal Rigidbody[] Rigidbodies;
    internal HingeJoint[] HingeJoints;

    private Camera _mainCamera;
    private Vector3 _offset;
    private bool _originalRestored;

    private void Start()
    {
        _mainCamera = Camera.main;
    }
    
    private void Update()
    {
    
    }

    private Vector3 CalculateMousePosition()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = transform.position.z - Camera.main.transform.position.z;
        var worldMousePosition = _mainCamera.ScreenToWorldPoint(mousePosition);
        worldMousePosition.z = transform.position.z;
        return worldMousePosition;
    }

    private void OnMouseDown()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        var worldMousePosition = CalculateMousePosition();
        _offset = transform.position - worldMousePosition;
    }

    private void OnMouseDrag()
    {
        var worldMousePosition = CalculateMousePosition();
        transform.position = worldMousePosition + _offset;

        var distanceToParent = (transform.position - Draggable.OriginalParent.position).magnitude;
        if (distanceToParent >= Draggable.AttachDistance)
            return;

        transform.parent.parent.SetParent(Draggable.OriginalParent);
        AddTheThing();
    }

    private void AddTheThing()
    {
        if (_originalRestored)
            return;

        Destroy(GetComponent<SimpleMouseDrag>());
        RestoreOriginal();
        Draggable.RestoreOriginalConfiguration();
        Draggable.Attached = true;
        _originalRestored = true;
    }

    private void OnMouseUp()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void RestoreOriginal()
    {
        foreach (var joint in HingeJoints)
            Destroy(joint);

        foreach (var rigidBody in Rigidbodies)
            Destroy(rigidBody);
    }
}
