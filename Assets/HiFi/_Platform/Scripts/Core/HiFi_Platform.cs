﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
///  class that holds references to XR player objects and info, methods to return XR input data
/// </summary>

namespace HiFi
{
    namespace Platform
    {
        public class HiFi_Platform : MonoBehaviour
        {
            /// Static instance
            public static HiFi_Platform instance = null;

            /// References - populated by PlatformInitializer
            public InputDevice leftHandDevice, rightHandDevice;
            public TrackingOriginMode trackingOriginMode;
            public TrackingSpaceType trackingSpaceType;

            /// <summary>
            /// Enum used for querying controller inputs. Use "none" to disable a preset. These generic names will be translated to the appropriate API-specific calls.
            /// </summary>
            public enum Hand { none, left, right, either };
            /// <summary>
            /// Enum used for querying controller inputs. These generic names will be translated to the appropriate API-specific calls.
            /// </summary>
            public enum Button { none, buttonOne, buttonTwo, axis2DUp, axis2DDown, axis2DLeft, axis2DRight, triggerTouch, trigger, grip, axis2DTouch, axis2DPress };
            /// <summary>
            /// Enum used for querying controller inputs. These generic names will be translated to the appropriate API-specific calls.
            /// </summary>
            public enum Axis { trigger, grip, axis2D_X, axis2D_Y };
            /// <summary>
            /// Enum used for querying controller inputs. Specifies button query method (.Press, .Release, .Hold). Use none to disable a preset.
            /// </summary>
            public enum Method { none, press, release, hold };

            /// ==============================================================================================================================================================
            /// Awake
            /// ==============================================================================================================================================================
            private void Awake()
            {
                if (instance == null)
                    instance = this;
            }

            /// <summary>
            /// Overloaded method to process and return info for an InputPreset (bool button type).
            /// </summary>
            /// <param name="preset"></param>
            /// <returns></returns>
            public bool Preset(HiFi_PresetButtonInput preset)
            {
                if (preset.method == Method.press)
                    return Press(preset.hand, preset.button);
                if (preset.method == Method.release)
                    return Release(preset.hand, preset.button);
                if (preset.method == Method.hold)
                    return Hold(preset.hand, preset.button);
                return false;
            }

            /// <summary>
            /// Overloaded method to process and return info for an InputPreset (float axisValue type). Note: using hand.either returns a summed value clamped between -1/1.
            /// </summary>
            /// <param name="preset"></param>
            /// <returns></returns>
            public float Preset(HiFi_PresetAxisInput preset)
            {
                return Value(preset.hand, preset.axis);
            }

            #region Input Query Methods

            /// ==============================================================================================================================================================
            /// PRESS
            /// ==============================================================================================================================================================
            /// <summary>
            /// Returns true for the first frame an input is pressed.
            /// </summary>
            /// <param name="hand"></param>
            /// <param name="input"></param>
            /// <returns></returns>
            public bool Press(Hand hand, Button button)
            {
                bool pressed = false;

                /// =========
                /// Left Hand
                /// =========
                if (hand == Hand.left || hand == Hand.either)
                {
                    /// Buttons
                    if (button == Button.trigger)
                        pressed = HiFi_InputProcessor.instance.IsPressed[14];
                    if (button == Button.grip)
                        pressed = HiFi_InputProcessor.instance.IsPressed[4];
                    if (button == Button.buttonOne)
                        pressed = HiFi_InputProcessor.instance.IsPressed[2];
                    if (button == Button.buttonTwo)
                        pressed = HiFi_InputProcessor.instance.IsPressed[3];
                    if (button == Button.axis2DPress)
                        pressed = HiFi_InputProcessor.instance.IsPressed[9];
                    if (button == Button.axis2DTouch)
                        pressed = HiFi_InputProcessor.instance.IsPressed[16];

                    /// Axis to button
                    if (button == Button.axis2DUp)
                        pressed = HiFi_InputProcessor.instance.IsPressed[2];
                    if (button == Button.axis2DDown)
                        pressed = HiFi_InputProcessor.instance.IsPressedNeg[2];
                    if (button == Button.axis2DLeft)
                        pressed = HiFi_InputProcessor.instance.IsPressedNeg[1];
                    if (button == Button.axis2DRight)
                        pressed = HiFi_InputProcessor.instance.IsPressed[1];

                    if (pressed)
                        return pressed;
                }

                /// ==========
                /// Right Hand
                /// ==========
                if (hand == Hand.right || hand == Hand.either)
                {
                    /// Button
                    if (button == Button.trigger)
                        pressed = HiFi_InputProcessor.instance.IsPressed[15];
                    if (button == Button.grip)
                        pressed = HiFi_InputProcessor.instance.IsPressed[5];
                    if (button == Button.buttonOne)
                        pressed = HiFi_InputProcessor.instance.IsPressed[0];
                    if (button == Button.buttonTwo)
                        pressed = HiFi_InputProcessor.instance.IsPressed[1];
                    if (button == Button.axis2DPress)
                        pressed = HiFi_InputProcessor.instance.IsPressed[9];
                    if (button == Button.axis2DTouch)
                        pressed = HiFi_InputProcessor.instance.IsPressed[17];

                    /// Axis to button
                    if (button == Button.axis2DUp)
                        pressed = HiFi_InputProcessor.instance.IsPressed[5];
                    if (button == Button.axis2DDown)
                        pressed = HiFi_InputProcessor.instance.IsPressedNeg[5];
                    if (button == Button.axis2DLeft)
                        pressed = HiFi_InputProcessor.instance.IsPressedNeg[4];
                    if (button == Button.axis2DRight)
                        pressed = HiFi_InputProcessor.instance.IsPressed[4];

                }

                return pressed;
            }

            /// ==============================================================================================================================================================
            /// RELEASE
            /// ==============================================================================================================================================================
            /// <summary>
            /// Returns true for the first frame an input is released.
            /// </summary>
            /// <param name="hand"></param>
            /// <param name="input"></param>
            /// <returns></returns>
            public  bool Release(Hand hand, Button button)
            {
                bool released = false;

                /// =========
                /// Left Hand
                /// =========
                if (hand == Hand.left || hand == Hand.either)
                {
                    /// Buttons
                    if (button == Button.trigger)
                        released = HiFi_InputProcessor.instance.IsReleased[14];
                    if (button == Button.grip)
                        released = HiFi_InputProcessor.instance.IsReleased[4];
                    if (button == Button.buttonOne)
                        released = HiFi_InputProcessor.instance.IsReleased[2];
                    if (button == Button.buttonTwo)
                        released = HiFi_InputProcessor.instance.IsReleased[3];
                    if (button == Button.axis2DPress)
                        released = HiFi_InputProcessor.instance.IsReleased[9];
                    if (button == Button.axis2DTouch)
                        released = HiFi_InputProcessor.instance.IsReleased[16];

                    /// Axis to button
                    if (button == Button.axis2DUp)
                        released = HiFi_InputProcessor.instance.IsReleased[2];
                    if (button == Button.axis2DDown)
                        released = HiFi_InputProcessor.instance.IsReleasedNeg[2];
                    if (button == Button.axis2DLeft)
                        released = HiFi_InputProcessor.instance.IsReleasedNeg[1];
                    if (button == Button.axis2DRight)
                        released = HiFi_InputProcessor.instance.IsReleased[1];

                    if (released)
                        return released;
                }

                /// ==========
                /// Right Hand
                /// ==========
                if (hand == Hand.right || hand == Hand.either)
                {
                    /// Button
                    if (button == Button.trigger)
                        released = HiFi_InputProcessor.instance.IsReleased[15];
                    if (button == Button.grip)
                        released = HiFi_InputProcessor.instance.IsReleased[5];
                    if (button == Button.buttonOne)
                        released = HiFi_InputProcessor.instance.IsReleased[0];
                    if (button == Button.buttonTwo)
                        released = HiFi_InputProcessor.instance.IsReleased[1];
                    if (button == Button.axis2DPress)
                        released = HiFi_InputProcessor.instance.IsReleased[9];
                    if (button == Button.axis2DTouch)
                        released = HiFi_InputProcessor.instance.IsReleased[17];

                    /// Axis to button
                    if (button == Button.axis2DUp)
                        released = HiFi_InputProcessor.instance.IsReleased[5];
                    if (button == Button.axis2DDown)
                        released = HiFi_InputProcessor.instance.IsReleasedNeg[5];
                    if (button == Button.axis2DLeft)
                        released = HiFi_InputProcessor.instance.IsReleasedNeg[4];
                    if (button == Button.axis2DRight)
                        released = HiFi_InputProcessor.instance.IsReleased[4];
                }

                return released;
            }

            /// ==============================================================================================================================================================
            /// HOLD
            /// ==============================================================================================================================================================
            /// <summary>
            /// Returns true for all frames a given input is held down
            /// </summary>
            /// <param name="hand"></param>
            /// <param name="input"></param>
            /// <returns></returns>
            public  bool Hold(Hand hand, Button button)
            {
                bool hold = false;

                /// =========
                /// Left Hand
                /// =========
                if (hand == Hand.left || hand == Hand.either)
                {
                    /// Buttons
                    if (button == Button.trigger)
                        hold = !HiFi_InputProcessor.instance.IsReset[14];
                    if (button == Button.grip)
                        hold = !HiFi_InputProcessor.instance.IsReset[4];
                    if (button == Button.buttonOne)
                        hold = !HiFi_InputProcessor.instance.IsReset[2];
                    if (button == Button.buttonTwo)
                        hold = !HiFi_InputProcessor.instance.IsReset[3];
                    if (button == Button.axis2DPress)
                        hold = !HiFi_InputProcessor.instance.IsReset[9];
                    if (button == Button.axis2DTouch)
                        hold = !HiFi_InputProcessor.instance.IsReset[16];

                    /// Axis to button
                    if (button == Button.axis2DUp)
                        hold = !HiFi_InputProcessor.instance.IsReset[2];
                    if (button == Button.axis2DDown)
                        hold = !HiFi_InputProcessor.instance.IsResetNeg[2];
                    if (button == Button.axis2DLeft)
                        hold = !HiFi_InputProcessor.instance.IsResetNeg[1];
                    if (button == Button.axis2DRight)
                        hold = !HiFi_InputProcessor.instance.IsReset[1];

                    if (hold)
                        return hold;
                }

                /// ==========
                /// Right Hand
                /// ==========
                if (hand == Hand.right || hand == Hand.either)
                {
                    /// Button
                    if (button == Button.trigger)
                        hold = !HiFi_InputProcessor.instance.IsReset[15];
                    if (button == Button.grip)
                        hold = !HiFi_InputProcessor.instance.IsReset[5];
                    if (button == Button.buttonOne)
                        hold = !HiFi_InputProcessor.instance.IsReset[0];
                    if (button == Button.buttonTwo)
                        hold = !HiFi_InputProcessor.instance.IsReset[1];
                    if (button == Button.axis2DPress)
                        hold = !HiFi_InputProcessor.instance.IsReset[9];
                    if (button == Button.axis2DTouch)
                        hold = !HiFi_InputProcessor.instance.IsReset[17];

                    if (button == Button.axis2DUp)
                        hold = !HiFi_InputProcessor.instance.IsReset[5];
                    if (button == Button.axis2DDown)
                        hold = !HiFi_InputProcessor.instance.IsResetNeg[5];
                    if (button == Button.axis2DLeft)
                        hold = !HiFi_InputProcessor.instance.IsResetNeg[4];
                    if (button == Button.axis2DRight)
                        hold = !HiFi_InputProcessor.instance.IsReset[4];
                }

                return hold;
            }

            /// ==============================================================================================================================================================
            /// VALUE
            /// ==============================================================================================================================================================
            /// <summary>
            /// Returns float values for a given controller axis
            /// </summary>
            /// <param name="hand"></param>
            /// <param name="axis"></param>
            /// <returns></returns>
            public  float Value(Hand hand, Axis axis)
            {
                float summedValues = 0f;

                /// =========
                /// Left Hand
                /// =========
                if (hand == Hand.left || hand == Hand.either)
                    {
                        if (axis == Axis.trigger)
                            summedValues += HiFi_InputProcessor.instance.Value[9];
                        if (axis == Axis.grip)
                            summedValues += HiFi_InputProcessor.instance.Value[11];
                        if (axis == Axis.axis2D_X)
                            summedValues += HiFi_InputProcessor.instance.Value[1];
                        if (axis == Axis.axis2D_Y)
                            summedValues += HiFi_InputProcessor.instance.Value[2];
                    }

                /// ==========
                /// Right Hand
                /// ==========
                if (hand == Hand.right || hand == Hand.either)
                    {
                        if (axis == Axis.trigger)
                            summedValues += HiFi_InputProcessor.instance.Value[10];
                        if (axis == Axis.grip)
                            summedValues += HiFi_InputProcessor.instance.Value[12];
                        if (axis == Axis.axis2D_X)
                            summedValues += HiFi_InputProcessor.instance.Value[4];
                        if (axis == Axis.axis2D_Y)
                            summedValues += HiFi_InputProcessor.instance.Value[5];
                    }

                return Mathf.Clamp(summedValues, -1.0f, 1.0f);
            }


            /// ==============================================================================================================================================================
            /// ANY [not implemented]
            /// ==============================================================================================================================================================
            /// <summary>
            /// Returns true if any button is pressed, excluding any touch/release/axis events. CURRENTLY DISABLED
            /// </summary>
            /// <returns></returns>
            public  bool AnyButtonPress()
            {
                return false;
            }

            #endregion
        }
    }
}
