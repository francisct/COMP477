using UnityEngine;

public class DraggableLowerArm : Draggable
{
    private GameObject _leftUpperArm;

    // 1
    private GameObject _lowerArm;
    private Vector3 _lowerArmPosition;
    private Quaternion _lowerArmRotation;

    // 2
    private GameObject _lowerArmScale;
    private Vector3 _lowerArmScalePosition;
    private Quaternion _lowerArmScaleRotation;

    // 3
    private GameObject _lowerArmObject;
    private Vector3 _lowerArmObjectPostion;
    private Quaternion _lowerArmObjectRotation;

    // 4
    private GameObject _hand;
    private Vector3 _handPosition;
    private Quaternion _handRotation;

    // 5
    private GameObject _handObject;
    private Vector3 _handObjectPosition;
    private Quaternion _handObjectRotation;

    // Joints
    private Vector3 _lowerArmHingeJointAnchor;

    protected override void Start()
    {
        base.Start();

        OriginalPosition = transform.localPosition;
        OriginalRotation = transform.localRotation;

        _leftUpperArm = GameObject.Find($"{SideString}UpperArm");

        // 1
        _lowerArm         = GameObject.Find($"{SideString}LowerArm");
        _lowerArmPosition = _lowerArm.transform.localPosition;
        _lowerArmRotation = _lowerArm.transform.localRotation;

        // 2
        _lowerArmScale         = GameObject.Find($"{SideString}LowerArmScale");
        _lowerArmScalePosition = _lowerArmScale.transform.localPosition;
        _lowerArmScaleRotation = _lowerArmScale.transform.localRotation;

        // 3
        _lowerArmObject         = GameObject.Find($"{SideString}LowerArmObject");
        _lowerArmObjectPostion  = _lowerArmObject.transform.localPosition;
        _lowerArmObjectRotation = _lowerArmObject.transform.localRotation;

        // 4
        _hand         = GameObject.Find($"{SideString}Hand");
        _handPosition = _hand.transform.localPosition;
        _handRotation = _hand.transform.localRotation;

        // 5
        _handObject         = GameObject.Find($"{SideString}HandObject");
        _handObjectPosition = _handObject.transform.localPosition;
        _handObjectRotation = _handObject.transform.localRotation;

        _lowerArmHingeJointAnchor = _lowerArm.GetComponent<HingeJoint>().connectedAnchor;
    }

    public override void RestoreOriginalConfiguration()
    {
        var lowerBody = _lowerArm.AddComponent<Rigidbody>();
        lowerBody.isKinematic = true;

        // 1
        _lowerArm.transform.localPosition = _lowerArmPosition;
        _lowerArm.transform.localRotation = _lowerArmRotation;

        // 2
        _lowerArmScale.transform.localPosition = _lowerArmScalePosition;
        _lowerArmScale.transform.localRotation = _lowerArmScaleRotation;

        // 3
        _lowerArmObject.transform.localPosition = _lowerArmObjectPostion;
        _lowerArmObject.transform.localRotation = _lowerArmObjectRotation;

        // 4
        _hand.transform.localPosition = _handPosition;
        _hand.transform.localRotation = _handRotation;

        // 5
        _handObject.transform.localPosition = _handObjectPosition;
        _handObject.transform.localRotation = _handObjectRotation;

        var lowerJoint = _lowerArm.AddComponent<HingeJoint>();
        lowerJoint.autoConfigureConnectedAnchor = false;
        lowerJoint.connectedBody = _leftUpperArm.GetComponent<Rigidbody>();
        lowerJoint.connectedAnchor = _lowerArmHingeJointAnchor;

        lowerBody.isKinematic = false;
    }

    public override void DestroyHingeJoints()
    {
        Destroy(_lowerArm.GetComponent<HingeJoint>());
        Destroy(_lowerArm.GetComponent<Rigidbody>());
    }

    public override void ModifyRigidBodies(bool isKinematic)
    {
        var lowerArm = GameObject.Find($"{SideString}LowerArm");

        lowerArm.GetComponent<Rigidbody>().isKinematic = isKinematic;
    }

    public override void CreateDetachedConfiguration()
    {
        base.CreateDetachedConfiguration();

        HingeJoints[0].connectedBody = Rigidbodies[1];
        var anch = HingeJoints[0].anchor;
        anch.y = -1;
        HingeJoints[0].anchor = anch;

        var mouseDrag = ObjectChildren[1].gameObject.AddComponent<SimpleMouseDrag>();
        ObjectChildren[1].position = CalculateMousePosition();
        mouseDrag.Draggable = this;
        mouseDrag.Rigidbodies = Rigidbodies;
        mouseDrag.HingeJoints = HingeJoints;
    }
}
