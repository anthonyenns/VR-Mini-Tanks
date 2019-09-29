using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Always-executing (edit/play modes) logic to make this object match transform of another object, simulating parenting.
/// </summary>

namespace HiFi
{
    namespace Tools
    {
        [ExecuteAlways]
        public class VirtualParent : MonoBehaviour
        {
            public bool followOn;
            public GameObject objectToFollow;
            public bool position, rotation, scale;

            void Update()
            {
                if (followOn && objectToFollow != null)
                {
                    if (position)
                    {
                        transform.position = objectToFollow.transform.position;
                    }
                    if (rotation)
                    {
                        transform.rotation = objectToFollow.transform.rotation;
                    }
                    if (scale)
                    {
                        transform.localScale = objectToFollow.transform.localScale;
                    }
                }
            }
        }
    }
}
