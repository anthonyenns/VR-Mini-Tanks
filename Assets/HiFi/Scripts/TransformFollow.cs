using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollow : MonoBehaviour
{
    public Transform toFollow;
    Vector3 offset;

    void Start()
    {
        offset = transform.position - toFollow.position;  
    }

    void Update()
    {
        transform.position = toFollow.position + offset;       
    }
}
