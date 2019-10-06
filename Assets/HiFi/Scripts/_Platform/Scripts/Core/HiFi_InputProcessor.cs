using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Frame by frame processing of all active input axes, testing for press/release.
/// </summary>
namespace HiFi
{
    namespace Platform
    {
        public class HiFi_InputProcessor : MonoBehaviour
        {
            /// Static instance
            public static HiFi_InputProcessor instance = null;

            [Header("Settings:")]
            [Range(0f, 1f)] public float pressThreshold = 0.5f;
            [Range(0f, 1f)] public float releaseThreshold = 0.5f;
            [Range(0f, 1f)] public float deadzone = 0.2f;

            public int[] activeButtonIndex = { 0, 1, 2, 3, 8, 9, 16, 17 }; /// Primary, 
            public int[] activeAxisIndex = { 1,-2,4,-5,9,10,11,12,21,22,23,24,25,26,27,28}; /// Use negative value to invert axis

            public float[] Value = new float[29];
            public bool[] Button = new bool[19];

            public bool[] IsPressed = new bool[29];
            public bool[] IsPressedNeg = new bool[29];
            public bool[] IsReleased = new bool[29];
            public bool[] IsReleasedNeg = new bool[29];
            public bool[] IsReset = new bool[29];
            public bool[] IsResetNeg = new bool[29];

            public bool[] IsPressedButton = new bool[19];
            public bool[] IsReleasedButton = new bool[19];
            public bool[] IsResetButton = new bool[19];

            private bool invertFlag, bInput;
            private Vector2 vInput, joystickL, joystickR;
            private float fInput, axisValue;
            private InputDevice left, right;

            private void Awake()
            {
                if (instance == null)
                    instance = this;

                left = HiFi_Platform.instance.leftHandDevice;
                right = HiFi_Platform.instance.rightHandDevice;
            }

            /// <summary>
            /// Update calls correct version of axis sets
            /// </summary>
            void Update()
            {
                /// 2D Axis Value [1,2][4,5]
                vInput = HiFi_Platform.instance.leftHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out vInput) ? vInput : Vector2.zero;
                Value[1] = vInput.x;
                Value[2] = vInput.y;
                vInput = HiFi_Platform.instance.rightHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out vInput) ? vInput : Vector2.zero;
                Value[4] = vInput.x;
                Value[5] = vInput.y;
                /// (+ WMR touchpad)
                vInput = HiFi_Platform.instance.leftHandDevice.TryGetFeatureValue(CommonUsages.secondary2DAxis, out vInput) ? vInput : Vector2.zero;
                Value[1] += vInput.x;
                Value[2] += vInput.y;
                vInput = HiFi_Platform.instance.rightHandDevice.TryGetFeatureValue(CommonUsages.secondary2DAxis, out vInput) ? vInput : Vector2.zero;
                Value[4] += vInput.x;
                Value[5] += vInput.y;

                /// Trigger Axis Value
                Value[9] = HiFi_Platform.instance.leftHandDevice.TryGetFeatureValue(CommonUsages.trigger, out fInput) ? fInput : 0;
                Value[10] = HiFi_Platform.instance.rightHandDevice.TryGetFeatureValue(CommonUsages.trigger, out fInput) ? fInput : 0;

                /// Grip Axis Value
                Value[11] = HiFi_Platform.instance.leftHandDevice.TryGetFeatureValue(CommonUsages.grip, out fInput) ? fInput : 0;
                Value[12] = HiFi_Platform.instance.rightHandDevice.TryGetFeatureValue(CommonUsages.grip, out fInput) ? fInput : 0;

                /// Fingers
                Value[21] = HiFi_Platform.instance.leftHandDevice.TryGetFeatureValue(CommonUsages.indexFinger, out fInput) ? fInput : 0;
                Value[22] = HiFi_Platform.instance.rightHandDevice.TryGetFeatureValue(CommonUsages.indexFinger, out fInput) ? fInput : 0;
                Value[23] = HiFi_Platform.instance.leftHandDevice.TryGetFeatureValue(CommonUsages.middleFinger, out fInput) ? fInput : 0;
                Value[24] = HiFi_Platform.instance.rightHandDevice.TryGetFeatureValue(CommonUsages.middleFinger, out fInput) ? fInput : 0;
                Value[25] = HiFi_Platform.instance.leftHandDevice.TryGetFeatureValue(CommonUsages.ringFinger, out fInput) ? fInput : 0;
                Value[26] = HiFi_Platform.instance.rightHandDevice.TryGetFeatureValue(CommonUsages.ringFinger, out fInput) ? fInput : 0;
                Value[27] = HiFi_Platform.instance.leftHandDevice.TryGetFeatureValue(CommonUsages.pinkyFinger, out fInput) ? fInput : 0;
                Value[28] = HiFi_Platform.instance.rightHandDevice.TryGetFeatureValue(CommonUsages.pinkyFinger, out fInput) ? fInput : 0;

                /// Primary Button
                Button[0] = HiFi_Platform.instance.leftHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bInput) ? bInput : false;
                Button[2] = HiFi_Platform.instance.rightHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bInput) ? bInput : false;

                /// Secondary Button
                //Button[3] = HiFi_Platform.instance.leftHandDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bInput) ? bInput : false;
                //Button[1] = HiFi_Platform.instance.rightHandDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bInput) ? bInput : false;

                /// 2D Axis Press
                Button[8] = HiFi_Platform.instance.leftHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bInput) ? bInput : false;
                Button[9] = HiFi_Platform.instance.rightHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bInput) ? bInput : false;
                
                /// 2D Axis Touch
                Button[16] = HiFi_Platform.instance.leftHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bInput) ? bInput : false;
                Button[17] = HiFi_Platform.instance.rightHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bInput) ? bInput : false;

                /// Oculus only additional touches 
                Button[16] = HiFi_Platform.instance.leftHandDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out bInput) ? bInput | Button[16] : Button[16];
                Button[17] = HiFi_Platform.instance.rightHandDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out bInput) ? bInput | Button[17] : Button[17];
                Button[16] = HiFi_Platform.instance.leftHandDevice.TryGetFeatureValue(CommonUsages.secondaryTouch, out bInput) ? bInput | Button[16] : Button[16];
                Button[17] = HiFi_Platform.instance.rightHandDevice.TryGetFeatureValue(CommonUsages.secondaryTouch, out bInput) ? bInput | Button[17] : Button[17];
                Button[16] = HiFi_Platform.instance.leftHandDevice.TryGetFeatureValue(CommonUsages.thumbrest, out bInput) ? bInput | Button[16] : Button[16];
                Button[17] = HiFi_Platform.instance.rightHandDevice.TryGetFeatureValue(CommonUsages.thumbrest, out bInput) ? bInput | Button[17] : Button[17];

                for (int i = 0; i < activeButtonIndex.Length; i++)
                {
                    ResetLatchesButtons(activeButtonIndex[i]);
                    CheckButtons(activeButtonIndex[i]);
                }

                for (int i = 0; i < activeAxisIndex.Length; i++)
                {
                    Value[i] = (Value[i] < -deadzone || Value[i] > deadzone) ? Value[i] : 0; /// Deadzone cleanup
                    ResetLatches(activeAxisIndex[i]);
                    CheckAxis(activeAxisIndex[i]);
                }
            }

            /// <summary>
            /// Does all bool latch checks
            /// </summary>
            /// <param name="a"></param>
            void CheckAxis(int axis)
            {
                if (axis < 0)
                {
                    axis *= -1;
                    invertFlag = true;
                }

                axisValue = Value[axis];

                ///apply deadzone
                if (axisValue < deadzone && axisValue > -deadzone)
                    axisValue = 0;

                ///invert axes as needed
                if (invertFlag)
                    axisValue *= -1f;

                ///trigger PRESS event & latch POS
                if (axisValue >= pressThreshold)
                {
                    if (IsReset[axis])
                    {
                        IsPressed[axis] = true;
                        IsReset[axis] = false;
                    }
                }

                ///trigger RELEASE event & reset POS
                else if (axisValue <= releaseThreshold)
                {
                    if (!IsReset[axis])
                    {
                        IsReleased[axis] = true;
                        IsReset[axis] = true;
                    }
                }

                ///trigger PRESS event & latch NEG
                if (axisValue <= -pressThreshold)
                {
                    if (IsResetNeg[axis])
                    {
                        IsPressedNeg[axis] = true;
                        IsResetNeg[axis] = false;
                    }
                }

                ///trigger RELEASE event & reset NEG
                else if (axisValue >= -releaseThreshold)
                {
                    if (!IsResetNeg[axis])
                    {
                        IsReleasedNeg[axis] = true;
                        IsResetNeg[axis] = true;
                    }
                }

                invertFlag = false;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="button"></param>
            void CheckButtons(int b)
            {
                ///trigger PRESS event & latch POS
                if (Button[b])
                {
                    if (IsResetButton[b])
                    {
                        IsPressedButton[b] = true;
                        IsResetButton[b] = false;
                    }
                }

                ///trigger RELEASE event & reset POS
                else if (!Button[b])
                {
                    if (!IsResetButton[b])
                    {
                        IsReleasedButton[b] = true;
                        IsResetButton[b] = true;
                    }
                }
            }

            /// <summary>
            /// Resets all boolean array values to false
            /// </summary>
            /// <param name="axis"></param>
            void ResetLatches(int axis)
            {
                axis = Mathf.Abs(axis);

                IsPressed[axis] = false;
                IsPressedNeg[axis] = false;
                IsReleased[axis] = false;
                IsReleasedNeg[axis] = false;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="b"></param>
            void ResetLatchesButtons(int b)
            {
                IsPressedButton[b] = false;
                IsReleasedButton[b] = false;
            }
        }
    }
}

