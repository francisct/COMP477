using UnityEngine;
using System.Collections;

public class JointSnap : MonoBehaviour {

    public float maxRadius;

    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 initialPos;

    // Use this for initialization
    void Start () {
    }


    void OnMouseDown()
    {
       
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        initialPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Debug.Log("down");
    }

    void OnMouseDrag()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        offset = mousePos - initialPos;
        float magnitude = Vector3.Magnitude(offset);
        Debug.Log(magnitude);
        if (magnitude > maxRadius)
        {
            RemoveConnectedBody(transform.parent);
        }

    }
    
    void RemoveConnectedBody(Transform limb)
    {
        HingeJoint hinge = limb.GetComponent<HingeJoint>();
        Component.Destroy(hinge);
    }
}
