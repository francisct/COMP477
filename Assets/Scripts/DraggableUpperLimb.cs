using System.Linq;
using UnityEngine;

public class DraggableUpperLimb : Draggable
{
    private GameObject _upperLimb;
    private GameObject _lowerLimb;

    private GameObject[] _childObjects;
    private Vector3[]    _childObjectPositions;
    private Quaternion[] _childObjectRotations;

    // Joints
    private Vector3 _upperLimbHingeJointAnchor;
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

        var hingeJoints = GetComponentsInChildren<HingeJoint>();
        _upperLimb = hingeJoints[0].gameObject;
        _lowerLimb = hingeJoints[1].gameObject;

        _upperLimbHingeJointAnchor = _upperLimb.GetComponent<HingeJoint>().connectedAnchor;
        _lowerLimbHingeJointAnchor = _lowerLimb.GetComponent<HingeJoint>().connectedAnchor;
    }

    public override void DestroyHingeJoints()
    {
        Destroy(_upperLimb.GetComponent<HingeJoint>());
        Destroy(_upperLimb.GetComponent<Rigidbody>());
        Destroy(_lowerLimb.GetComponent<HingeJoint>());
        Destroy(_lowerLimb.GetComponent<Rigidbody>());
    }

    public override void AddRigidBodies()
    {
        _upperLimb.AddComponent<Rigidbody>();
        _lowerLimb.AddComponent<Rigidbody>();
    }

    protected override void UpdateChildComponents()
    {
        for (var index = 0; index < _childObjects.Length; index++)
        {
            _childObjects[index].transform.localPosition = _childObjectPositions[index];
            _childObjects[index].transform.localRotation = _childObjectRotations[index];
        }

        var upperJoint = _upperLimb.AddComponent<HingeJoint>();
        upperJoint.autoConfigureConnectedAnchor = false;
        upperJoint.connectedBody = _upperLimb.transform.parent.GetComponent<Rigidbody>();
        upperJoint.connectedAnchor = _upperLimbHingeJointAnchor;

        var lowerJoint = _lowerLimb.AddComponent<HingeJoint>();
        lowerJoint.autoConfigureConnectedAnchor = false;
        lowerJoint.connectedBody = _upperLimb.GetComponent<Rigidbody>();
        lowerJoint.connectedAnchor = _lowerLimbHingeJointAnchor;
    }

    public override void ModifyRigidBodies(bool isKinematic)
    {
        _upperLimb.GetComponent<Rigidbody>().isKinematic = isKinematic;
        if (_lowerLimb.GetComponent<Rigidbody>() != null)
        _lowerLimb.GetComponent<Rigidbody>().isKinematic = isKinematic;
    }

    public override void CreateDetachedConfiguration()
    {
        base.CreateDetachedConfiguration();

        HingeJoints[0].connectedBody = Rigidbodies[1];
        var anch = HingeJoints[0].anchor;
        anch.y = -1;
        HingeJoints[0].anchor = anch;

        if (HingeJoints.Length >= 2)
            HingeJoints[1].connectedBody = Rigidbodies[1];
        if (HingeJoints.Length >= 3)
            HingeJoints[2].connectedBody = Rigidbodies[2];

        var mouseDrag = ObjectChildren[1].gameObject.AddComponent<DragDetachedJoint>();
        ObjectChildren[1].position = CalculateMousePosition();
        mouseDrag.Draggable = this;
        mouseDrag.Rigidbodies = Rigidbodies;
        mouseDrag.HingeJoints = HingeJoints;
    }
}
