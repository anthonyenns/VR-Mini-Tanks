using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Sets transform's local position and rotation to match an XRNode
/// </summary>

namespace HiFi
{
    namespace Platform
    {
        public class HiFi_NodeTracker : MonoBehaviour
        {
            /*
            public XRNode node;
            public bool unLock;

            private void Start()
            {
                InputTracking.trackingLost += Dock;
            }

            private void Update()
            {
                if (!unLock)
                {
                    transform.localPosition = InputTracking.GetLocalPosition(node);
                    transform.localRotation = InputTracking.GetLocalRotation(node);
                }
            }

            private void FixedUpdate()
            {
                if (!unLock)
                {
                    transform.localPosition = InputTracking.GetLocalPosition(node);
                    transform.localRotation = InputTracking.GetLocalRotation(node);
                }
            }

            private void Dock(XRNodeState nodeState)
            {
                if(nodeState.nodeType == node)
                {
                    //do something?
                }
            }*/
        }
    }
}
