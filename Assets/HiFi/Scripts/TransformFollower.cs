using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollower : MonoBehaviour
{
    public Transform leader;
    public bool useOffset = true;

    Vector3 posOffset;
    Quaternion rotOffset;
    bool follow = false;

    void Update()
    {
        if (follow && leader != null)
            FollowTransform();
        else
        {
            if(leader != null)
            {
                posOffset = leader.position - transform.position;
                rotOffset = transform.rotation;
                follow = true;
            }
            else
            {
                follow = false;
            }
        }
    }

    private void FollowTransform()
    {
        if (useOffset)
        {
            transform.position = leader.position - posOffset;
            transform.rotation = leader.rotation * rotOffset;
        }
        else
        {
            transform.position = leader.position;
            transform.rotation = leader.rotation;
        }
    }
}
