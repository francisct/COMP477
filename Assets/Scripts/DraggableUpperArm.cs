using UnityEngine;

public class DraggableUpperArm : Draggable
{
    // 1
    private GameObject _upperArm;
    private Vector3 _upperArmPosition;
    private Quaternion _upperArmRotation;

    // 2
    private GameObject _upperArmScale;
    private Vector3 _upperArmScalePosition;
    private Quaternion _upperArmScaleRotation;

    // 3
    private GameObject _upperArmObject;
    private Vector3 _upperArmObjectPosition;
    private Quaternion _upperArmObjectRotation;

    // 4
    private GameObject _lowerArmJoint;
    private Vector3 _lowerArmJointPosition;
    private Quaternion _lowerArmJointRotation;

    // 5
    private GameObject _lowerArmJointObject;
    private Vector3 _lowerArmJointObjectPosition;
    private Quaternion _lowerArmJointObjectRotation;

    // 6
    private GameObject _lowerArm;
    private Vector3 _lowerArmPosition;
    private Quaternion _lowerArmRotation;

    // 7
    private GameObject _lowerArmScale;
    private Vector3 _lowerArmScalePosition;
    private Quaternion _lowerArmScaleRotation;

    // 8
    private GameObject _lowerArmObject;
    private Vector3 _lowerArmObjectPostion;
    private Quaternion _lowerArmObjectRotation;

    // 9
    private GameObject _hand;
    private Vector3 _handPosition;
    private Quaternion _handRotation;

    // 10
    private GameObject _handObject;
    private Vector3 _handObjectPosition;
    private Quaternion _handObjectRotation;

    // Joints
    private Vector3 _upperArmHingeJointAnchor;
    private Vector3 _lowerArmHingeJointAnchor;

    protected override void Start()
    {
        base.Start();

        OriginalPosition = transform.localPosition;
        OriginalRotation = transform.localRotation;

        // 1
        _upperArm         = GameObject.Find($"{SideString}UpperArm");
        _upperArmPosition = _upperArm.transform.localPosition;
        _upperArmRotation = _upperArm.transform.localRotation;

        // 2
        _upperArmScale         = GameObject.Find($"{SideString}UpperArmScale");
        _upperArmScalePosition = _upperArmScale.transform.localPosition;
        _upperArmScaleRotation = _upperArmScale.transform.localRotation;

        // 3
        _upperArmObject         = GameObject.Find($"{SideString}UpperArmObject");
        _upperArmObjectPosition = _upperArmObject.transform.localPosition;
        _upperArmObjectRotation = _upperArmObject.transform.localRotation;

        // 4
        _lowerArmJoint         = GameObject.Find($"{SideString}LowerArmJoint");
        _lowerArmJointPosition = _lowerArmJoint.transform.localPosition;
        _lowerArmJointRotation = _lowerArmJoint.transform.localRotation;

        // 5
        _lowerArmJointObject         = GameObject.Find($"{SideString}LowerArmJointObject");
        _lowerArmJointObjectPosition = _lowerArmJointObject.transform.localPosition;
        _lowerArmJointObjectRotation = _lowerArmJointObject.transform.localRotation;

        // 6
        _lowerArm         = GameObject.Find($"{SideString}LowerArm");
        _lowerArmPosition = _lowerArm.transform.localPosition;
        _lowerArmRotation = _lowerArm.transform.localRotation;

        // 7
        _lowerArmScale         = GameObject.Find($"{SideString}LowerArmScale");
        _lowerArmScalePosition = _lowerArmScale.transform.localPosition;
        _lowerArmScaleRotation = _lowerArmScale.transform.localRotation;

        // 8
        _lowerArmObject         = GameObject.Find($"{SideString}LowerArmObject");
        _lowerArmObjectPostion  = _lowerArmObject.transform.localPosition;
        _lowerArmObjectRotation = _lowerArmObject.transform.localRotation;

        // 9
        _hand         = GameObject.Find($"{SideString}Hand");
        _handPosition = _hand.transform.localPosition;
        _handRotation = _hand.transform.localRotation;

        // 10
        _handObject         = GameObject.Find($"{SideString}HandObject");
        _handObjectPosition = _handObject.transform.localPosition;
        _handObjectRotation = _handObject.transform.localRotation;

        _upperArmHingeJointAnchor = _upperArm.GetComponent<HingeJoint>().connectedAnchor;
        _lowerArmHingeJointAnchor = _lowerArm.GetComponent<HingeJoint>().connectedAnchor;
    }

    public override void RestoreOriginalConfiguration()
    {
        var upperBody = _upperArm.AddComponent<Rigidbody>();
        var lowerBody = _lowerArm.AddComponent<Rigidbody>();
        upperBody.isKinematic = true;
        lowerBody.isKinematic = true;

        // 1
        _upperArm.transform.localPosition = _upperArmPosition;
        _upperArm.transform.localRotation = _upperArmRotation;

        // 2
        _upperArmScale.transform.localPosition = _upperArmScalePosition;
        _upperArmScale.transform.localRotation = _upperArmScaleRotation;

        // 3
        _upperArmObject.transform.localPosition = _upperArmObjectPosition;
        _upperArmObject.transform.localRotation = _upperArmObjectRotation;

        // 4
        _lowerArmJoint.transform.localPosition = _lowerArmJointPosition;
        _lowerArmJoint.transform.localRotation = _lowerArmJointRotation;

        // 5
        _lowerArmJointObject.transform.localPosition = _lowerArmJointObjectPosition;
        _lowerArmJointObject.transform.localRotation = _lowerArmJointObjectRotation;

        // 6
        _lowerArm.transform.localPosition = _lowerArmPosition;
        _lowerArm.transform.localRotation = _lowerArmRotation;

        // 7
        _lowerArmScale.transform.localPosition = _lowerArmScalePosition;
        _lowerArmScale.transform.localRotation = _lowerArmScaleRotation;

        // 8
        _lowerArmObject.transform.localPosition = _lowerArmObjectPostion;
        _lowerArmObject.transform.localRotation = _lowerArmObjectRotation;

        // 9
        _hand.transform.localPosition = _handPosition;
        _hand.transform.localRotation = _handRotation;

        // 10
        _handObject.transform.localPosition = _handObjectPosition;
        _handObject.transform.localRotation = _handObjectRotation;

        var upperJoint = _upperArm.AddComponent<HingeJoint>();
        upperJoint.autoConfigureConnectedAnchor = false;
        upperJoint.connectedBody = _upperArm.transform.parent.GetComponent<Rigidbody>();
        upperJoint.connectedAnchor = _upperArmHingeJointAnchor;

        var lowerJoint = _lowerArm.AddComponent<HingeJoint>();
        lowerJoint.autoConfigureConnectedAnchor = false;
        lowerJoint.connectedBody = upperBody;
        lowerJoint.connectedAnchor = _lowerArmHingeJointAnchor;

        upperBody.isKinematic = false;
        lowerBody.isKinematic = false;
    }

    public override void DestroyHingeJoints()
    {
        Destroy(_upperArm.GetComponent<HingeJoint>());
        Destroy(_upperArm.GetComponent<Rigidbody>());
        Destroy(_lowerArm.GetComponent<HingeJoint>());
        Destroy(_lowerArm.GetComponent<Rigidbody>());
    }

    public override void ModifyRigidBodies(bool isKinematic)
    {
        _upperArm.GetComponent<Rigidbody>().isKinematic = isKinematic;
        _lowerArm.GetComponent<Rigidbody>().isKinematic = isKinematic;
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
