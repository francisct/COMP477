﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Side
{
    Left,
    Right
}

// ReSharper disable once CheckNamespace
public abstract class Draggable : MonoBehaviour
{
    public bool Attached;

    public Transform OriginalParent;

    public float AttachDistance = 1;

    public Side Side;

    internal Vector3 OriginalPosition;
    internal Quaternion OriginalRotation;

    protected Camera MainCamera;
    protected Rigidbody[] Rigidbodies;
    protected HingeJoint[] HingeJoints;
    protected Transform[] ObjectChildren;
    protected string SideString;

    protected virtual void Start()
    {
        MainCamera = Camera.main;
        SideString = Side == Side.Left ? "Left" : "Right";
    }

    protected virtual void Update() { }

    public abstract void RestoreOriginalConfiguration();

    public abstract void DestroyHingeJoints();

    public abstract void ModifyRigidBodies(bool isKinematic);

    public virtual void CreateDetachedConfiguration()
    {
        ObjectChildren =
                GetComponentsInChildren<Transform>()
                           .Where(child => child.name.Contains("Object")).ToArray();
        var rigidBodies = new List<Rigidbody>();
        var hingeJoints = new List<HingeJoint>();
        
        DestroyHingeJoints();

        for (var i = 0; i < ObjectChildren.Length; i++)
        {
            rigidBodies.Add(ObjectChildren[i].gameObject.AddComponent<Rigidbody>());
            
            if (i != 1)
                hingeJoints.Add(ObjectChildren[i].gameObject.AddComponent<HingeJoint>());
        }

        Rigidbodies = rigidBodies.ToArray();
        HingeJoints = hingeJoints.ToArray();
    }

    public Vector3 CalculateMousePosition()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = transform.position.z - Camera.main.transform.position.z;
        var worldMousePosition = MainCamera.ScreenToWorldPoint(mousePosition);
        worldMousePosition.z = transform.position.z;
        return worldMousePosition;
    }
}
