using UnityEngine;

public class DragDetachedJoint : MonoBehaviour
{
    internal Draggable Draggable;

    private Camera _mainCamera;
    private Vector3 _offset;
    private bool _originalRestored;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    //static int hi = 0;
    private Vector3 CalculateMousePosition()
    {
        //Debug.Log(hi);
        //++hi;

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

        var audio = GetComponent<AudioSource>();
        audio.volume = 0.3f;
        audio.Play();
    }

    private void OnMouseDrag()
    {
        var worldMousePosition = CalculateMousePosition();
        transform.position = worldMousePosition + _offset;

        var distanceToParent = CalculateDistanceToParent();
        if (distanceToParent >= Draggable.AttachDistance || _originalRestored)
            return;

        transform.parent.parent.SetParent(Draggable.OriginalParent);
        Destroy(GetComponent<DragDetachedJoint>());
        Draggable.RestoreOriginalConfiguration();
        Draggable.Attached = true;
        _originalRestored = true;
    }

    private void OnMouseUp()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private float CalculateDistanceToParent()
    {
        return Mathf.Sqrt(Mathf.Pow(transform.position.x - Draggable.OriginalParent.position.x, 2) +
                          Mathf.Pow(transform.position.y - Draggable.OriginalParent.position.y, 2));
    }
}
