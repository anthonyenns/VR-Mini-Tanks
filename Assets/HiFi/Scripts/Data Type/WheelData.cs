using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiFi
{
    public class WheelData
    {
        public float zPosition;
        public float zStart;
        //public float zForce;
        public float normalizedPosition;
        public Transform hitTransform;
        public float top; /// travel local position
        public float bottom; /// travel local position
    }
}
