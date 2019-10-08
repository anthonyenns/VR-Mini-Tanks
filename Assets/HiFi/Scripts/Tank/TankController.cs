using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using HiFi.Platform;
using UnityEngine.XR;

namespace HiFi
{
    public class TankController : MonoBehaviour
    {
        public Vector3 avatarSeatPosition;
        public Vector3 seatPosition2D;
        public Vector3 seatRotation2D;
        public GameObject cameraNull;

        /// Temporary controls
        public HiFi_PresetAxisInput leftSpeedController;
        public HiFi_PresetAxisInput rightSpeedController;

        public float leftSpeed; /// Value is returned from TankSync
        public float rightSpeed; /// Value is returned from TankSync
        public bool ownedLocally { get; private set; }

        private TankSync _tankSync;
        public RealtimeView _realtimeView { get; private set; }
        private Vector2 speedInput;

        private void Awake()
        {
            _realtimeView = GetComponent<RealtimeView>();
            _tankSync = GetComponent<TankSync>();

            /// Set ownedLocally to true if offline
            if (_realtimeView == null)
                ownedLocally = true;

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
            /// Do we own this tank?
            if (_realtimeView != null)
            {
                ownedLocally = _realtimeView.isOwnedLocally;
                if (!ownedLocally)
                    return;
            }

            /// Get user input
            GetUserInput();

            /// Send values to tank model, or set directly if offline
            if (_realtimeView != null)
                _tankSync.SetSpeeds(speedInput);
            else
            {
                leftSpeed = speedInput.x;
                rightSpeed = speedInput.y;
            }
        }

        /// Temporary input methods
        private void GetUserInput()
        {
            speedInput = Vector2.zero;
            /// XR ccontroller input
            if (XRSettings.enabled)
            {
                speedInput.x = HiFi_Platform.instance.Preset(leftSpeedController);
                speedInput.y = HiFi_Platform.instance.Preset(rightSpeedController);
            }

            /// Keyboard Input
            if (Input.GetKey(KeyCode.W))
                speedInput = new Vector2(1.0f, 1.0f);

            if (Input.GetKey(KeyCode.X))
                speedInput = new Vector2(-1.0f, -1.0f);

            if (Input.GetKey(KeyCode.D))
                speedInput = new Vector2(1.0f, -1.0f);

            if (Input.GetKey(KeyCode.A))
                speedInput = new Vector2(-1.0f, 1.0f);

            if (Input.GetKey(KeyCode.Q))
                speedInput += new Vector2(0.5f, 1.0f);

            if (Input.GetKey(KeyCode.E))
                speedInput += new Vector2(1.0f, 0.5f);

            if (Input.GetKey(KeyCode.Z))
                speedInput += new Vector2(-0.5f, -1.0f);

            if (Input.GetKey(KeyCode.C))
                speedInput += new Vector2(-1.0f, -0.5f);

        }
    }
}
