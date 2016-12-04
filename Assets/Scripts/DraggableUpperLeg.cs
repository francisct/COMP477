using System.Linq;
using UnityEngine;

public class DraggableUpperLeg : Draggable
{
    private GameObject _upperLeg;
    private GameObject _lowerLeg;

    private GameObject[] _childObjects;
    private Vector3[]    _childObjectPositions;
    private Quaternion[] _childObjectRotations;

    // Joints
    private Vector3 _upperFootHingeJointAnchor;
    private Vector3 _lowerFootHingeJointAnchor;

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

        _upperLeg = GameObject.Find($"{SideString}UpperLeg");
        _lowerLeg = GameObject.Find($"{SideString}LowerLeg");

        _upperFootHingeJointAnchor = _upperLeg.GetComponent<HingeJoint>().connectedAnchor;
        _lowerFootHingeJointAnchor = _lowerLeg.GetComponent<HingeJoint>().connectedAnchor;
    }

    public override void DestroyHingeJoints()
    {
        Destroy(_upperLeg.GetComponent<HingeJoint>());
        Destroy(_upperLeg.GetComponent<Rigidbody>());
        Destroy(_lowerLeg.GetComponent<HingeJoint>());
        Destroy(_lowerLeg.GetComponent<Rigidbody>());
    }

    public override void AddRigidBodies()
    {
        _upperLeg.AddComponent<Rigidbody>();
        _lowerLeg.AddComponent<Rigidbody>();
    }

    protected override void UpdateChildComponents()
    {
        for (var index = 0; index < _childObjects.Length; index++)
        {
            _childObjects[index].transform.localPosition = _childObjectPositions[index];
            _childObjects[index].transform.localRotation = _childObjectRotations[index];
        }

        var upperJoint = _upperLeg.AddComponent<HingeJoint>();
        upperJoint.autoConfigureConnectedAnchor = false;
        upperJoint.connectedBody = _upperLeg.transform.parent.GetComponent<Rigidbody>();
        upperJoint.connectedAnchor = _upperFootHingeJointAnchor;

        var lowerJoint = _lowerLeg.AddComponent<HingeJoint>();
        lowerJoint.autoConfigureConnectedAnchor = false;
        lowerJoint.connectedBody = _upperLeg.GetComponent<Rigidbody>();
        lowerJoint.connectedAnchor = _lowerFootHingeJointAnchor;
    }

    public override void ModifyRigidBodies(bool isKinematic)
    {
        _upperLeg.GetComponent<Rigidbody>().isKinematic = isKinematic;
        _lowerLeg.GetComponent<Rigidbody>().isKinematic = isKinematic;
    }

    public override void CreateDetachedConfiguration()
    {
        base.CreateDetachedConfiguration();

        HingeJoints[0].connectedBody = Rigidbodies[1];
        var anch = HingeJoints[0].anchor;
        anch.y = -1;
        HingeJoints[0].anchor = anch;
        HingeJoints[1].connectedBody = Rigidbodies[1];
        HingeJoints[2].connectedBody = Rigidbodies[2];

        var mouseDrag = ObjectChildren[1].gameObject.AddComponent<SimpleMouseDrag>();
        ObjectChildren[1].position = CalculateMousePosition();
        mouseDrag.Draggable = this;
        mouseDrag.Rigidbodies = Rigidbodies;
        mouseDrag.HingeJoints = HingeJoints;
    }
}
