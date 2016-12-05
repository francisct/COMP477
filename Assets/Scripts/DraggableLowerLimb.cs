using System.Linq;
using UnityEngine;

public class DraggableLowerLimb : Draggable
{
    private GameObject _upperLimb;
    private GameObject _lowerLimb;

    private GameObject[] _childObjects;
    private Vector3[]    _childObjectPositions;
    private Quaternion[] _childObjectRotations;

    // Joints
    private Vector3 _lowerLimbHingeJointAnchor;

    protected override void Start()
    {
        base.Start();

        OriginalPosition = transform.localPosition;
        OriginalRotation = transform.localRotation;

        _childObjects =
                GetComponentsInChildren<Transform>().Select(trans => trans.gameObject).ToArray();
        _childObjectPositions =
                GetComponentsInChildren<Transform>().Select(trans => trans.localPosition).ToArray();
        _childObjectRotations =
                GetComponentsInChildren<Transform>().Select(trans => trans.localRotation).ToArray();

        _upperLimb = transform.parent.GetComponentInParent<HingeJoint>().gameObject;
        _lowerLimb = GetComponentInChildren<HingeJoint>().gameObject;

        _lowerLimbHingeJointAnchor = _lowerLimb.GetComponent<HingeJoint>().connectedAnchor;
    }

    public override void DestroyHingeJoints()
    {
        Destroy(_lowerLimb.GetComponent<HingeJoint>());
        Destroy(_lowerLimb.GetComponent<Rigidbody>());
    }

    public override void AddRigidBodies()
    {
        _lowerLimb.AddComponent<Rigidbody>();
    }

    protected override void UpdateChildComponents()
    {
        for (var index = 0; index < _childObjects.Length; index++)
        {
            _childObjects[index].transform.localPosition = _childObjectPositions[index];
            _childObjects[index].transform.localRotation = _childObjectRotations[index];
        }
        
        var lowerJoint = _lowerLimb.AddComponent<HingeJoint>();
        lowerJoint.autoConfigureConnectedAnchor = false;
        lowerJoint.connectedBody = _upperLimb.GetComponent<Rigidbody>();
        lowerJoint.connectedAnchor = _lowerLimbHingeJointAnchor;
    }

    public override void ModifyRigidBodies(bool isKinematic)
    {
        _lowerLimb.GetComponent<Rigidbody>().isKinematic = isKinematic;
    }

    public override void CreateDetachedConfiguration()
    {
        base.CreateDetachedConfiguration();

        HingeJoints[0].connectedBody = Rigidbodies[1];
        var anch = HingeJoints[0].anchor;
        anch.y = -1.8f;
        HingeJoints[0].anchor = anch;

        var detachedJointDrag = ObjectChildren[1].gameObject.AddComponent<DragDetachedJoint>();
        ObjectChildren[1].position = CalculateMousePosition();
        detachedJointDrag.Draggable = this;
    }
}
