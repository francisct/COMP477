using UnityEngine;

public class DraggableUpperLeg : Draggable
{
    // 1
    private GameObject _upperLeg;
    private Vector3 _upperLegPosition;
    private Quaternion _upperLegRotation;

    // 2
    private GameObject _upperLegScale;
    private Vector3 _upperLegScalePosition;
    private Quaternion _upperLegScaleRotation;

    // 3
    private GameObject _upperLegObject;
    private Vector3 _upperLegObjectPosition;
    private Quaternion _upperLegObjectRotation;

    // 4
    private GameObject _lowerLegJoint;
    private Vector3 _lowerLegJointPosition;
    private Quaternion _lowerLegJointRotation;

    // 5
    private GameObject _lowerLegJointObject;
    private Vector3 _lowerLegJointObjectPosition;
    private Quaternion _lowerLegJointObjectRotation;

    // 6
    private GameObject _lowerLeg;
    private Vector3 _lowerLegPosition;
    private Quaternion _lowerLegRotation;

    // 7
    private GameObject _lowerLegScale;
    private Vector3 _lowerLegScalePosition;
    private Quaternion _lowerLegScaleRotation;

    // 8
    private GameObject _lowerLegObject;
    private Vector3 _lowerLegObjectPostion;
    private Quaternion _lowerLegObjectRotation;

    // 9
    private GameObject _foot;
    private Vector3 _footPosition;
    private Quaternion _footRotation;

    // 10
    private GameObject _footObject;
    private Vector3 _footObjectPosition;
    private Quaternion _footObjectRotation;

//    // 11
//    private GameObject _footFingers;
//    private Vector3 _footFingersPosition;
//    private Quaternion _footFingersRotation;
//
//    // 12
//    private GameObject _footFingersObject;
//    private Vector3 _footFingersObjectPosition;
//    private Quaternion _footFingersObjectRotation;

    // Joints
    private Vector3 _upperFootHingeJointAnchor;
    private Vector3 _lowerFootHingeJointAnchor;

    protected override void Start()
    {
        base.Start();

        OriginalPosition = transform.localPosition;
        OriginalRotation = transform.localRotation;

        // 1
        _upperLeg         = GameObject.Find($"{SideString}UpperLeg");
        _upperLegPosition = _upperLeg.transform.localPosition;
        _upperLegRotation = _upperLeg.transform.localRotation;

        // 2
        _upperLegScale         = GameObject.Find($"{SideString}UpperLegScale");
        _upperLegScalePosition = _upperLegScale.transform.localPosition;
        _upperLegScaleRotation = _upperLegScale.transform.localRotation;

        // 3
        _upperLegObject         = GameObject.Find($"{SideString}UpperLegObject");
        _upperLegObjectPosition = _upperLegObject.transform.localPosition;
        _upperLegObjectRotation = _upperLegObject.transform.localRotation;

        // 4
        _lowerLegJoint         = GameObject.Find($"{SideString}LowerLegJoint");
        _lowerLegJointPosition = _lowerLegJoint.transform.localPosition;
        _lowerLegJointRotation = _lowerLegJoint.transform.localRotation;

        // 5
        _lowerLegJointObject         = GameObject.Find($"{SideString}LowerLegJointObject");
        _lowerLegJointObjectPosition = _lowerLegJointObject.transform.localPosition;
        _lowerLegJointObjectRotation = _lowerLegJointObject.transform.localRotation;

        // 6
        _lowerLeg         = GameObject.Find($"{SideString}LowerLeg");
        _lowerLegPosition = _lowerLeg.transform.localPosition;
        _lowerLegRotation = _lowerLeg.transform.localRotation;

        // 7
        _lowerLegScale         = GameObject.Find($"{SideString}LowerLegScale");
        _lowerLegScalePosition = _lowerLegScale.transform.localPosition;
        _lowerLegScaleRotation = _lowerLegScale.transform.localRotation;

        // 8
        _lowerLegObject         = GameObject.Find($"{SideString}LowerLegObject");
        _lowerLegObjectPostion  = _lowerLegObject.transform.localPosition;
        _lowerLegObjectRotation = _lowerLegObject.transform.localRotation;

        // 9
        _foot         = GameObject.Find($"{SideString}Foot");
        _footPosition = _foot.transform.localPosition;
        _footRotation = _foot.transform.localRotation;

        // 10
        _footObject         = GameObject.Find($"{SideString}FootObject");
        _footObjectPosition = _footObject.transform.localPosition;
        _footObjectRotation = _footObject.transform.localRotation;

//        // 11
//        _footFingers         = GameObject.Find($"{SideString}FootFingers");
//        _footFingersPosition = _footFingers.transform.localPosition;
//        _footFingersRotation = _footFingers.transform.localRotation;
//
//        // 12
//        _footFingersObject         = GameObject.Find($"{SideString}FootFingersObject");
//        _footFingersObjectPosition = _footFingersObject.transform.localPosition;
//        _footFingersObjectRotation = _footFingersObject.transform.localRotation;

        _upperFootHingeJointAnchor = _upperLeg.GetComponent<HingeJoint>().connectedAnchor;
        _lowerFootHingeJointAnchor = _lowerLeg.GetComponent<HingeJoint>().connectedAnchor;
    }

    public override void RestoreOriginalConfiguration()
    {
        var upperBody = _upperLeg.AddComponent<Rigidbody>();
        var lowerBody = _lowerLeg.AddComponent<Rigidbody>();
        upperBody.isKinematic = true;
        lowerBody.isKinematic = true;

        // 1
        _upperLeg.transform.localPosition = _upperLegPosition;
        _upperLeg.transform.localRotation = _upperLegRotation;

        // 2
        _upperLegScale.transform.localPosition = _upperLegScalePosition;
        _upperLegScale.transform.localRotation = _upperLegScaleRotation;

        // 3
        _upperLegObject.transform.localPosition = _upperLegObjectPosition;
        _upperLegObject.transform.localRotation = _upperLegObjectRotation;

        // 4
        _lowerLegJoint.transform.localPosition = _lowerLegJointPosition;
        _lowerLegJoint.transform.localRotation = _lowerLegJointRotation;

        // 5
        _lowerLegJointObject.transform.localPosition = _lowerLegJointObjectPosition;
        _lowerLegJointObject.transform.localRotation = _lowerLegJointObjectRotation;

        // 6
        _lowerLeg.transform.localPosition = _lowerLegPosition;
        _lowerLeg.transform.localRotation = _lowerLegRotation;

        // 7
        _lowerLegScale.transform.localPosition = _lowerLegScalePosition;
        _lowerLegScale.transform.localRotation = _lowerLegScaleRotation;

        // 8
        _lowerLegObject.transform.localPosition = _lowerLegObjectPostion;
        _lowerLegObject.transform.localRotation = _lowerLegObjectRotation;

        // 9
        _foot.transform.localPosition = _footPosition;
        _foot.transform.localRotation = _footRotation;

        // 10
        _footObject.transform.localPosition = _footObjectPosition;
        _footObject.transform.localRotation = _footObjectRotation;

//        // 11
//        _footFingers.transform.localPosition = _footFingersPosition;
//        _footFingers.transform.localRotation = _footFingersRotation;
//
//        // 12
//        _footFingersObject.transform.localPosition = _footFingersObjectPosition;
//        _footFingersObject.transform.localRotation = _footFingersObjectRotation;

        var upperJoint = _upperLeg.AddComponent<HingeJoint>();
        upperJoint.autoConfigureConnectedAnchor = false;
        upperJoint.connectedBody = _upperLeg.transform.parent.GetComponent<Rigidbody>();
        upperJoint.connectedAnchor = _upperFootHingeJointAnchor;

        var lowerJoint = _lowerLeg.AddComponent<HingeJoint>();
        lowerJoint.autoConfigureConnectedAnchor = false;
        lowerJoint.connectedBody = upperBody;
        lowerJoint.connectedAnchor = _lowerFootHingeJointAnchor;

        upperBody.isKinematic = false;
        lowerBody.isKinematic = false;
    }

    public override void DestroyHingeJoints()
    {
        Destroy(_upperLeg.GetComponent<HingeJoint>());
        Destroy(_upperLeg.GetComponent<Rigidbody>());
        Destroy(_lowerLeg.GetComponent<HingeJoint>());
        Destroy(_lowerLeg.GetComponent<Rigidbody>());
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
