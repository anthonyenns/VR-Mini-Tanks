using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data class for XR controller offset setttings
/// </summary>

namespace HiFi
{
    namespace Platform
    {
        [System.Serializable]
        public class HiFi_ControllerOffsets
        {
            public Vector3 leftOrigin, rightOrigin, leftAimDirection, rightAimDirection;
        }
    }
}
