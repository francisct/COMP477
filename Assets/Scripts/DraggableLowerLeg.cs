using UnityEngine;

public class DraggableLowerLeg : Draggable
{
    private GameObject _leftUpperLeg;

    // 1
    private GameObject _lowerLeg;
    private Vector3 _lowerLegPosition;
    private Quaternion _lowerLegRotation;

    // 2
    private GameObject _lowerLegScale;
    private Vector3 _lowerLegScalePosition;
    private Quaternion _lowerLegScaleRotation;

    // 3
    private GameObject _lowerLegObject;
    private Vector3 _lowerLegObjectPostion;
    private Quaternion _lowerLegObjectRotation;

    // 4
    private GameObject _foot;
    private Vector3 _footPosition;
    private Quaternion _footRotation;

    // 5
    private GameObject _footObject;
    private Vector3 _footObjectPosition;
    private Quaternion _footObjectRotation;

//    // 6
//    private GameObject _footFingers;
//    private Vector3 _footFingersPosition;
//    private Quaternion _footFingersRotation;
//
//    // 7
//    private GameObject _footFingersObject;
//    private Vector3 _footFingersObjectPosition;
//    private Quaternion _footFingersObjectRotation;

    // Joints
    private Vector3 _lowerFootHingeJointAnchor;

    protected override void Start()
    {
        base.Start();

        OriginalPosition = transform.localPosition;
        OriginalRotation = transform.localRotation;

        _leftUpperLeg = GameObject.Find($"{SideString}UpperLeg");

        // 1
        _lowerLeg         = GameObject.Find($"{SideString}LowerLeg");
        _lowerLegPosition = _lowerLeg.transform.localPosition;
        _lowerLegRotation = _lowerLeg.transform.localRotation;

        // 2
        _lowerLegScale         = GameObject.Find($"{SideString}LowerLegScale");
        _lowerLegScalePosition = _lowerLegScale.transform.localPosition;
        _lowerLegScaleRotation = _lowerLegScale.transform.localRotation;

        // 3
        _lowerLegObject         = GameObject.Find($"{SideString}LowerLegObject");
        _lowerLegObjectPostion  = _lowerLegObject.transform.localPosition;
        _lowerLegObjectRotation = _lowerLegObject.transform.localRotation;

        // 4
        _foot         = GameObject.Find($"{SideString}Foot");
        _footPosition = _foot.transform.localPosition;
        _footRotation = _foot.transform.localRotation;

        // 5
        _footObject         = GameObject.Find($"{SideString}FootObject");
        _footObjectPosition = _footObject.transform.localPosition;
        _footObjectRotation = _footObject.transform.localRotation;

//        // 6
//        _footFingers         = GameObject.Find($"{SideString}FootFingers");
//        _footFingersPosition = _footFingers.transform.localPosition;
//        _footFingersRotation = _footFingers.transform.localRotation;
//
//        // 7
//        _footFingersObject         = GameObject.Find($"{SideString}FootFingersObject");
//        _footFingersObjectPosition = _footFingersObject.transform.localPosition;
//        _footFingersObjectRotation = _footFingersObject.transform.localRotation;


        _lowerFootHingeJointAnchor = _lowerLeg.GetComponent<HingeJoint>().connectedAnchor;
    }

    public override void RestoreOriginalConfiguration()
    {
        var lowerBody = _lowerLeg.AddComponent<Rigidbody>();
        lowerBody.isKinematic = true;

        // 1
        _lowerLeg.transform.localPosition = _lowerLegPosition;
        _lowerLeg.transform.localRotation = _lowerLegRotation;

        // 2
        _lowerLegScale.transform.localPosition = _lowerLegScalePosition;
        _lowerLegScale.transform.localRotation = _lowerLegScaleRotation;

        // 3
        _lowerLegObject.transform.localPosition = _lowerLegObjectPostion;
        _lowerLegObject.transform.localRotation = _lowerLegObjectRotation;

        // 4
        _foot.transform.localPosition = _footPosition;
        _foot.transform.localRotation = _footRotation;

        // 5
        _footObject.transform.localPosition = _footObjectPosition;
        _footObject.transform.localRotation = _footObjectRotation;

//        // 6
//        _footFingers.transform.localPosition = _footFingersPosition;
//        _footFingers.transform.localRotation = _footFingersRotation;
//
//        // 7
//        _footFingersObject.transform.localPosition = _footFingersObjectPosition;
//        _footFingersObject.transform.localRotation = _footFingersObjectRotation;

        var lowerJoint = _lowerLeg.AddComponent<HingeJoint>();
        lowerJoint.autoConfigureConnectedAnchor = false;
        lowerJoint.connectedBody = _leftUpperLeg.GetComponent<Rigidbody>();
        lowerJoint.connectedAnchor = _lowerFootHingeJointAnchor;

        lowerBody.isKinematic = false;
    }

    public override void DestroyHingeJoints()
    {
        Destroy(_lowerLeg.GetComponent<HingeJoint>());
        Destroy(_lowerLeg.GetComponent<Rigidbody>());
    }

    public override void ModifyRigidBodies(bool isKinematic)
    {
        _lowerLeg.GetComponent<Rigidbody>().isKinematic = isKinematic;
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
