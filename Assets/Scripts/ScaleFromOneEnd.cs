using UnityEngine;
using System.Collections;
using System;

public class ScaleFromOneEnd : MonoBehaviour
{
    

    float globalScale;
    float initialGlobalScale;

    private float buffer = 0.01f;

    void Start()
    {
        
        initialGlobalScale = transform.lossyScale.y;
    }

    void Update()
    {
        globalScale = transform.lossyScale.y;

        //if the global scale changed
        if (Math.Abs(globalScale - initialGlobalScale) > buffer)
        {
            PreventScalingFromBothEnd();
            initialGlobalScale = transform.lossyScale.y;
        }
    }

    void PreventScalingFromBothEnd()
    {
        
        float diff = globalScale - initialGlobalScale;
        HingeJoint hinge = GetComponent<HingeJoint>();
        if (hinge != null)
        {
            hinge.anchor = new Vector3(hinge.anchor.x, hinge.anchor.y + diff, hinge.anchor.z);
        }
        else
        {
            // minus diff because we want the limb to strech on the opposite side of the hand
            transform.position = new Vector3(transform.position.x, transform.position.y - diff, transform.position.z);
        }
        PushChildren(diff);
    }

    void PushChildren(float diff)
    {
        HingeJoint childHinge = transform.GetChild(0).GetComponent<HingeJoint>();
        childHinge.anchor = new Vector3(childHinge.anchor.x, childHinge.anchor.y + diff, childHinge.anchor.z);
    }

}
