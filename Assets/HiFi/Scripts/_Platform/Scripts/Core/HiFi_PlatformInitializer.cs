using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// This class obtains XR device information and enables XR, gets/sets references to gameobjects, sets up offsets
/// </summary>

namespace HiFi
{
    namespace Platform
    {
        public class HiFi_PlatformInitializer : MonoBehaviour
        {
            public bool XREnabled;
            public TrackingSpaceType targetTrackingSpace;
            public float refreshRate = 2.0f;
            public HiFi_PresetButtonInput recenterButton;

            [Header("Hand Controllers Setup:")]
            public HiFi_ControllerOffsets setupOculusTouch;
            public HiFi_ControllerOffsets setupOculusTouchOpenVR;
            public HiFi_ControllerOffsets setupViveWands;
            public HiFi_ControllerOffsets setupWMR;
            public HiFi_ControllerOffsets setupWMROpenVR;

            [Range(1.0f, 2.0f)] public float renderScale = 1.0f;

            [Header("XR System Info (Auto):")]
            public string family = "none";
            public string model = "none";
            public string[] supportedDevices;
            [SerializeField] private List<InputDevice> inputDevices = new List<InputDevice>();
            [SerializeField] private List<XRNodeState> nodeStates = new List<XRNodeState>();
            List<XRNodeState> nodes = new List<XRNodeState>();
            public UserPresenceState userPresence;
            public List<InputDevice> leftHandDevices = new List<InputDevice>();
            public List<InputDevice> rightHandDevices = new List<InputDevice>();

            private string allNodes, prevNodes, devices, inputs, prevInputs;
            private HiFi_ControllerOffsets detectedOffsets;
            private IEnumerator updateSystem;

            /// ====================================================================================
            /// OnEnable
            /// ====================================================================================
            private void OnEnable()
            {
                /// Enable XR
                XRSettings.enabled = XREnabled;
            }

            private void Awake()
            {
                if (XREnabled)
                {
                    /// List Build Supported Devices
                    supportedDevices = XRSettings.supportedDevices;
                    foreach (string device in supportedDevices)
                        devices += device + ", ";
                    HiFi_Utilities.DebugText($"This Build Supports XR Devices: ({devices})");

                    /// Get XR System info and set tracking space
                    SetupXREnvironment();
                }
            }

            /// ====================================================================================
            /// Set Environment Parameters
            /// ====================================================================================
            /// <summary> 
            /// Populates static Platform references with XRNodes and applies controller offsets
            /// </summary> 
            private void SetupXREnvironment()
            {
                /// Display XRSettings Device Name + Model
                family = XRSettings.loadedDeviceName;
                model = XRDevice.model;
                HiFi_Utilities.DebugText($"Found XR System - ({family}) running a ({model}) HMD. ");

                /// Coroutine
                updateSystem = UpdateSystem();
                StartCoroutine(updateSystem);
            
                /// Haptics... seems broken
                /*InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                HapticCapabilities capabilities;
                if (rightHandDevice.TryGetHapticCapabilities(out capabilities))
                {
                    if (capabilities.supportsImpulse)
                    {
                        uint channel = 0;
                        float amplitude = 0.5f;
                        float duration = 1.0f;
                        rightHandDevice.SendHapticImpulse(channel, amplitude, duration);
                    }
                }*/

                /// Set Initial Supersampling
                XRSettings.eyeTextureResolutionScale = renderScale;

                /// Set Initial Tracking Space
                HiFi_Platform.instance.SetTrackingSpace(targetTrackingSpace, true);
            }

            /// ====================================================================================
            /// Determine and apply controller offsets
            /// ====================================================================================
            /// <summary>
            /// Assigns user set offsets to tracked hand origin/aim helper objects
            /// </summary>
            /// <param name="setup"></param>
            void SetControllers(Transform left, Transform right)
            {
                string appliedOffsets = "None";

                /// Oculus Family
                /// =============
                if (family.ToLower().Contains("oculus"))
                {
                    appliedOffsets = "Oculus Touch Native";
                    detectedOffsets = setupOculusTouch;
                }

                /// OpenVR Family
                /// =============
                else if (family.ToLower().Contains("openvr") || family.ToLower().Contains("open") || family.ToLower().Contains("vive"))
                {
                    /// Default - WMR
                    appliedOffsets = "WMR for OpenVR";
                    detectedOffsets = setupWMROpenVR;

                    /// Check other models

                    /// Vive - OpenVR 
                    if (model.ToLower().Contains("vive"))
                    {
                        appliedOffsets = "Vive for OpenVR";
                        detectedOffsets = setupViveWands;
                    }
                    /// Oculus - OpenVR 
                    if (model.ToLower().Contains("oculus"))
                    {
                        appliedOffsets = "Oculus Touch for OpenVR";
                        detectedOffsets = setupOculusTouchOpenVR;
                    }
                }

                /// No XR - pancake mode
                /// ====================
                else if (family == ("none") || model == ("none") || string.IsNullOrEmpty(family) || string.IsNullOrEmpty(model))
                {
                    return;
                }

                /// Windows Mixed Reality Family (Other)
                /// ====================================
                else
                {
                    appliedOffsets = "WMR Native";
                    detectedOffsets = setupWMR;
                }

                /// Apply the offsets
                /// ====================================
                HiFi_Utilities.DebugText($"Applying XR Controller Offsets: ({appliedOffsets})");

                left.ResetLocalIdentity();
                left.localPosition = detectedOffsets.leftOrigin;
                left.localEulerAngles = detectedOffsets.leftAimDirection;

                right.ResetLocalIdentity();
                right.localPosition = detectedOffsets.rightOrigin;
                right.localEulerAngles = detectedOffsets.rightAimDirection;
            }

            /// ====================================================================================
            /// Helper - Polls XRNode devices for hand controllers
            /// ====================================================================================
            /// <summary>
            /// Polls Left and Right XRNodes for device info
            /// </summary>
            private void GetHandsDevices()
            {
                InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
                if (leftHandDevices.Count > 0)
                {
                    if (HiFi_Platform.instance.leftHandDevice != leftHandDevices[0])
                    {
                        HiFi_Utilities.DebugText($"Device name ({leftHandDevices[0].name}) with role ({leftHandDevices[0].role.ToString()})");
                        HiFi_Platform.instance.leftHandDevice = leftHandDevices[0];
                    }
                }

                InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
                if (rightHandDevices.Count > 0)
                {
                    if (HiFi_Platform.instance.rightHandDevice != rightHandDevices[0])
                    {
                        HiFi_Utilities.DebugText($"Device name ({rightHandDevices[0].name}) with role ({rightHandDevices[0].role.ToString()})");
                        HiFi_Platform.instance.rightHandDevice = rightHandDevices[0];
                    }
                }
            }

            private IEnumerator UpdateSystem()
            {
                while (true)
                {
                    allNodes = string.Empty;
                    inputs = string.Empty;

                    /// Get device info from XR Nodes
                    GetHandsDevices();

                    /// Display Input Devices
                    InputDevices.GetDevices(inputDevices);
                    foreach (InputDevice device in inputDevices)
                        inputs += $"InputDevices: Device found with name ({device.name}) and role ({device.role.ToString()})\n";

                    /// XR nodes, get and list
                    InputTracking.GetNodeStates(nodeStates);
                    foreach (XRNodeState node in nodeStates)
                        allNodes += node.nodeType.ToString() + ", ";
                    
                    if(allNodes != prevNodes)
                        HiFi_Utilities.DebugText($"XRNodes present: ({allNodes})");
                    prevNodes = allNodes;

                    if (inputs != prevInputs)
                        HiFi_Utilities.DebugText(inputs);
                    prevInputs = inputs;

                    userPresence = XRDevice.userPresence;

                    yield return new WaitForSeconds(refreshRate);
                }
            }
        }
    }
}


