using UnityEngine;
using System.Collections;
using System;

public class PreventInheritedScaling : MonoBehaviour {

    Vector3 localScale;
    Vector3 globalScale;
    Vector3 initialGlobalScale;

    Vector3 initialParentScale;

    private float buffer = 0.01f;

	// Use this for initialization
	void Start () {
        initialGlobalScale = transform.lossyScale;
        initialParentScale = transform.parent.localScale;
    }
	
	// Update is called once per frame
	void Update () {

        //activate only if parent is scaling
        if (IsParentScaling())
        {

            globalScale = transform.lossyScale;

            //if the global scale changed
            if (Math.Abs(globalScale.y - initialGlobalScale.y) > buffer)
            {
                PreserveChildrenScaleY();
                initialGlobalScale = transform.lossyScale;
            }
        }
    }

    bool IsParentScaling()
    {
        Vector3 parentScale = transform.parent.localScale;
        return Math.Abs(parentScale.y - initialParentScale.y) > buffer;
    }

    void PreserveChildrenScaleX()
    {
        float diff = initialGlobalScale.x / globalScale.x;
        localScale = transform.localScale;
        float newlocalX = localScale.x * diff;

        transform.localScale = new Vector3(newlocalX, localScale.y, localScale.z);
    }

    void PreserveChildrenScaleY()
    {
        float diff = initialGlobalScale.y / globalScale.y;
        localScale = transform.localScale;
        float newlocalY = localScale.y * diff;

        transform.localScale = new Vector3(localScale.x, newlocalY, localScale.z);
    }

    void PreserveChildrenScaleZ()
    {
        float diff = initialGlobalScale.z / globalScale.z;
        localScale = transform.localScale;
        float newlocalZ = localScale.z * diff;

        transform.localScale = new Vector3(localScale.x, localScale.y, newlocalZ);
    }
}
