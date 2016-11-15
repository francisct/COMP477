﻿using UnityEngine;

// ReSharper disable once CheckNamespace
public class Draggable : MonoBehaviour
{
    public bool Attached;

    public Transform OriginalParent;

    // The time we added the rigid-body (in order to allow some time for it
    //      to pick up velocity before trying to remove it because of zero velocity)
    private float _rigidBodyAddedTime;

    // ReSharper disable once UnusedMember.Local
    private void Update()
    {
        var rigidBody = GetComponent<Rigidbody>();

        if (rigidBody == null)
            return;

        if (rigidBody.velocity == Vector3.zero && Time.time > _rigidBodyAddedTime + 0.5f)
            Destroy(rigidBody);
    }

    // Add a rigid-body to this limb so that it falls on the ground
    public void AddRigidBody()
    {
        gameObject.AddComponent<Rigidbody>();
        _rigidBodyAddedTime = Time.time;
    }
}
