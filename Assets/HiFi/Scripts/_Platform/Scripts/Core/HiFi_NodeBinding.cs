using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Sets transform's local position and rotation to match an XRNode
/// </summary>

namespace HiFi
{
    public class HiFi_NodeBinding : MonoBehaviour
    {
        [Header("Settings")]
        public Transform head;
        public Transform leftHand;
        public Transform rightHand;
        public bool runInUpdate = true;
        public bool runInFixedUpdate = false;
        public bool runInLateUpdate = false;

        private List<XRNodeState> nodeStates = new List<XRNodeState>();
        private bool headActive, leftHandActive, rightHandActive, headPreviousActive;
        private Vector3 position;
        private Quaternion rotation;
        private Transform geo;

        private void Start()
        {
            geo = transform.GetChild(0);
        }

        private void Update()
        {
            nodeStates.Clear();
            InputTracking.GetNodeStates(nodeStates);

            if (runInUpdate)
            {
                for (int i = 0; i < nodeStates.Count; i++)
                {
                    /// Head
                    if (nodeStates[i].nodeType == XRNode.Head)
                    {
                        headActive = nodeStates[i].tracked;

                        if (nodeStates[i].TryGetPosition(out position))
                            head.localPosition = position;

                        if (nodeStates[i].TryGetRotation(out rotation))
                            head.localRotation = rotation;
                    }

                    /// Left Hand
                    else if (nodeStates[i].nodeType == XRNode.LeftHand)
                    {
                        leftHandActive = nodeStates[i].tracked;

                        if (nodeStates[i].TryGetPosition(out position))
                            leftHand.localPosition = position;

                        if (nodeStates[i].TryGetRotation(out rotation))
                            leftHand.localRotation = rotation;
                    }

                    /// Right Hand
                    else if (nodeStates[i].nodeType == XRNode.RightHand)
                    {
                        rightHandActive = nodeStates[i].tracked;

                        if (nodeStates[i].TryGetPosition(out position))
                            rightHand.localPosition = position;

                        if (nodeStates[i].TryGetRotation(out rotation))
                            rightHand.localRotation = rotation;
                    }
                }

                /// Toggle visibility
                head.gameObject.SetActive(headActive);
                leftHand.gameObject.SetActive(leftHandActive);
                rightHand.gameObject.SetActive(rightHandActive);

                if (headActive && !headPreviousActive)
                    InputTracking.Recenter();

                headPreviousActive = headActive;
            }
        }

        private void FixedUpdate()
        {
            if (runInFixedUpdate)
            {
                for (int i = 0; i < nodeStates.Count; i++)
                {
                    /// Head
                    if (nodeStates[i].nodeType == XRNode.Head)
                    {
                        if (nodeStates[i].TryGetPosition(out position))
                            head.localPosition = position;

                        if (nodeStates[i].TryGetRotation(out rotation))
                            head.localRotation = rotation;
                    }

                    /// Left Hand
                    else if (nodeStates[i].nodeType == XRNode.LeftHand)
                    {
                        if (nodeStates[i].TryGetPosition(out position))
                            leftHand.localPosition = position;

                        if (nodeStates[i].TryGetRotation(out rotation))
                            leftHand.localRotation = rotation;
                    }

                    /// Right Hand
                    else if (nodeStates[i].nodeType == XRNode.RightHand)
                    {
                        if (nodeStates[i].TryGetPosition(out position))
                            rightHand.localPosition = position;

                        if (nodeStates[i].TryGetRotation(out rotation))
                            rightHand.localRotation = rotation;
                    }
                }
            }
        }

        private void LateUpdate()
        {
            if (runInLateUpdate)
            {
                for (int i = 0; i < nodeStates.Count; i++)
                {
                    /// Head
                    if (nodeStates[i].nodeType == XRNode.Head)
                    {
                        if (nodeStates[i].TryGetPosition(out position))
                            head.localPosition = position;

                        if (nodeStates[i].TryGetRotation(out rotation))
                            head.localRotation = rotation;
                    }

                    /// Left Hand
                    else if (nodeStates[i].nodeType == XRNode.LeftHand)
                    {
                        if (nodeStates[i].TryGetPosition(out position))
                            leftHand.localPosition = position;

                        if (nodeStates[i].TryGetRotation(out rotation))
                            leftHand.localRotation = rotation;
                    }

                    /// Right Hand
                    else if (nodeStates[i].nodeType == XRNode.RightHand)
                    {
                        if (nodeStates[i].TryGetPosition(out position))
                            rightHand.localPosition = position;

                        if (nodeStates[i].TryGetRotation(out rotation))
                            rightHand.localRotation = rotation;
                    }
                }
            }
        }
    }
}
