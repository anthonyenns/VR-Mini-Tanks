﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using HiFi.Platform;
using UnityEngine.XR;

namespace HiFi
{
    /// <summary>
    /// Routes LOCAL user input to TankSyncModel for networking
    /// </summary>
    public class TankController : MonoBehaviour
    {
        public Vector3 avatarSeatPosition;
        public bool smoothCamera;
        public Vector3 seatPosition2D;
        public Vector3 seatRotation2D;
        public GameObject cameraNull;
        public GameObject camObject;
        public Vector2 gunAngleClamp;

        [Header("Temp Controls")]
        public HiFi_PresetAxisInput leftSpeedController;
        public HiFi_PresetAxisInput rightSpeedController;
        public HiFi_PresetButtonInput gunAxisToggle;

        [Header("Values returned from Model")]
        public float leftSpeedNetworkReturn; /// Value is returned from TankSync
        public float rightSpeedNetworkReturn; /// Value is returned from TankSync
        public float gunAngleNetworkReturn; /// Value is returned from TankSync
        public bool ownedLocally { get; private set; }

        private TankSync _tankSync;
        public RealtimeView realtimeView { get; private set; }
        private Vector2 localSpeedInput;
        private float localAngleInput;
        private Quaternion camOffset;
        private UserPresenceState userPresenceCache;

        private void Awake()
        {
            realtimeView = GetComponent<RealtimeView>();
            _tankSync = GetComponent<TankSync>();

            /// Set up offline mode
            if (realtimeView == null)
            {
                ownedLocally = true;
                camObject.SetActive(true);
            }

            /// Set camera position
            if (XRSettings.enabled)
            {
                cameraNull.transform.localPosition = avatarSeatPosition;
                cameraNull.transform.localRotation = Quaternion.identity;
            }
            else
            {
                cameraNull.transform.localPosition = seatPosition2D;
                cameraNull.transform.localRotation = Quaternion.Euler(seatRotation2D);
            }
        }

        private void Update()
        {
            /// Do we own this tank? ================================================
            if (realtimeView != null)
            {
                camObject.SetActive(realtimeView.isOwnedLocally);
                ownedLocally = realtimeView.isOwnedLocally;
                if (!ownedLocally)
                    return;
            }
            /// =====================================================================

            /// Get local input
            GetUserInput();

            /// Send values to tank model, or set directly if offline
            if (realtimeView != null)
            {
                _tankSync.SetSpeeds(localSpeedInput);
                _tankSync.SetGunAngle(localAngleInput);
            }
            else
            {
                leftSpeedNetworkReturn = localSpeedInput.x;
                rightSpeedNetworkReturn = localSpeedInput.y;
                gunAngleNetworkReturn = localAngleInput;
            }

            /// Recenter HMD if now present but wasn't before
            if (XRSettings.enabled)
            {
                if (userPresenceCache != UserPresenceState.Present && XRDevice.userPresence == UserPresenceState.Present)
                    InputTracking.Recenter();

                userPresenceCache = XRDevice.userPresence;
            }

            /// Camera rotation smoothing
            if (smoothCamera)
            {
                camOffset.eulerAngles = new Vector3(0, cameraNull.transform.rotation.eulerAngles.y, 0);
                cameraNull.transform.rotation = camOffset;
            }
        }

        /// Temporary input methods
        private void GetUserInput()
        {
            localSpeedInput = Vector2.zero;

            /// XR controller input
            if (XRSettings.enabled)
            {
                if (!HiFi_Platform.instance.Preset(gunAxisToggle))
                {
                    localSpeedInput.x = HiFi_Platform.instance.Preset(leftSpeedController);
                    localSpeedInput.y = HiFi_Platform.instance.Preset(rightSpeedController);
                }
                else
                {
                    localAngleInput += HiFi_Platform.instance.Preset(rightSpeedController) * 5.0f * Time.deltaTime;
                    localAngleInput += HiFi_Platform.instance.Preset(leftSpeedController) * 5.0f * Time.deltaTime;
                }
            }

            /// Keyboard Input
            if (Input.GetKey(KeyCode.W))
                localSpeedInput = new Vector2(1.0f, 1.0f);

            if (Input.GetKey(KeyCode.X))
                localSpeedInput = new Vector2(-1.0f, -1.0f);

            if (Input.GetKey(KeyCode.D))
                localSpeedInput = new Vector2(1.0f, -1.0f);

            if (Input.GetKey(KeyCode.A))
                localSpeedInput = new Vector2(-1.0f, 1.0f);

            if (Input.GetKey(KeyCode.Q))
                localSpeedInput += new Vector2(0.5f, 1.0f);

            if (Input.GetKey(KeyCode.E))
                localSpeedInput += new Vector2(1.0f, 0.5f);

            if (Input.GetKey(KeyCode.Z))
                localSpeedInput += new Vector2(-0.5f, -1.0f);

            if (Input.GetKey(KeyCode.C))
                localSpeedInput += new Vector2(-1.0f, -0.5f);

            if (Input.GetKey(KeyCode.DownArrow))
                localAngleInput += 5.0f * Time.deltaTime;
            if (Input.GetKey(KeyCode.UpArrow))
                localAngleInput -= 5.0f * Time.deltaTime;

            /// Clamp Gun Angle
            localAngleInput = Mathf.Clamp(localAngleInput, gunAngleClamp.x, gunAngleClamp.y);
        }
    }
}
