using UnityEngine;
using System.Collections;

public class JointSnap : MonoBehaviour {

    public float maxRadius;

    public bool snapped = false;
    private float magnitude;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 initialPos;

    bool bounceAppliedAfterSnapped = false;
    // Use this for initialization
    void Start () {
    }


    void OnMouseDown()
    {
    }

    void OnMouseDrag()
    {
        if (!snapped)
        {
            initialPos = GetComponent<HingeJoint>().connectedBody.GetComponent<HingeJoint>().connectedBody.transform.position;
            initialPos = Camera.main.WorldToScreenPoint(initialPos);
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            offset = mousePos - initialPos;
            magnitude = Vector3.Magnitude(offset);
            Debug.Log("Force magnitude:" + magnitude);
            if (magnitude > maxRadius)
            {
                RemoveConnectedBody(transform.parent);
            }
            else
            {
                GetComponent<HingeJoint>().connectedBody.GetComponent<LimbSpring>().stretch(magnitude, false);
            }
        }

        else if (!bounceAppliedAfterSnapped)
        {
            //reset its length after snapping
            GetComponent<HingeJoint>().connectedBody.GetComponent<LimbSpring>().stretch(magnitude * 0.5f, true);
            bounceAppliedAfterSnapped = true;
        }
    }

    void OnMouseUp()
    {
        if (!snapped)
        GetComponent<HingeJoint>().connectedBody.GetComponent<LimbSpring>().stretch(magnitude, true);
    }

    void RemoveConnectedBody(Transform limb)
    {
        HingeJoint hinge = limb.GetComponent<HingeJoint>();
        Component.Destroy(hinge);
        snapped = true;
    }
}
