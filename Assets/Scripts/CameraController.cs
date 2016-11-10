using UnityEngine;

// ReSharper disable once CheckNamespace
public class CameraController : MonoBehaviour
{
    public float MouseSensitivity;

    // ReSharper disable once UnusedMember.Local
    private void Update()
    {
        // Zoom the camera using mouse wheel
        Camera.main.fieldOfView -= Input.mouseScrollDelta.y * 2;

        var mouseX = Input.GetAxis ("Mouse X");
        var mouseY = Input.GetAxis ("Mouse Y");
        
        // Right-click to drag camera around
        if (Input.GetMouseButton(1))
            transform.Translate(new Vector3(-mouseX, 0.0f, -mouseY) * MouseSensitivity);
    }
}
