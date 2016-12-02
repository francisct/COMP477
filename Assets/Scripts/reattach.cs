using UnityEngine;
using System.Collections;

public class reattach : MonoBehaviour
{
    private float radius = 0.2f;
    private static float timeBeforeRattach = 4f;
    private float timeOut = timeBeforeRattach;
    private GameObject parentParent;

    Vector3 initialAnchorPos;

    void Start()
    {
        initialAnchorPos = transform.parent.GetComponent<HingeJoint>().connectedAnchor;
        parentParent = transform.parent.parent.gameObject;
    }

    void Update()
    {
        timeOut -= Time.deltaTime;
    }

    void OnMouseDrag()
    {

        if (GetComponent<JointSnap>().snapped && timeOut < 0)
            limbInRadius(transform.position, radius);
    }

    void limbInRadius(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Joint")
            {
                GameObject parentLimb = transform.parent.gameObject;
                if (hitColliders[i].gameObject != parentParent)
                {
                    return;
                }

                //refactor radius if possible
                //float r = parentParentLimb.GetComponent<SphereCollider>().radius;

                //Vector3 offset = new Vector3(0f, -0.02f, 0f);

                //Have to set position before adding hingejoint
                parentLimb.transform.position = parentParent.transform.position;

                parentLimb.AddComponent<HingeJoint>().connectedBody =
                    parentParent.GetComponent<Rigidbody>();
                //parentLimb.transform.parent = parentParent.transform;
                //necessary to make the script ScaleFromOneEnd work
                HingeJoint hinge = parentLimb.GetComponent<HingeJoint>();
                hinge.autoConfigureConnectedAnchor = false;
                //this prevents the reattached limb from having a large air gap with his parent
                hinge.connectedAnchor = initialAnchorPos;

                reset();

                //transform.parent.parent = hitColliders[i].transform;
                break;
            }
            ++i;
        }
    }

    void reset()
    {
        GetComponent<JointSnap>().snapped = false;
        timeOut = timeBeforeRattach;
    }
}

