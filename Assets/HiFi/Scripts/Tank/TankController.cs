using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using HiFi.Platform;

namespace HiFi
{
    public class TankController : MonoBehaviour
    {
        public Vector3 avatarSeatPosition;
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

            cameraNull.transform.localPosition = avatarSeatPosition;
            cameraNull.transform.localRotation = Quaternion.identity;
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

        private void GetUserInput()
        {
            speedInput.x = HiFi_Platform.instance.Preset(leftSpeedController);
            speedInput.y = HiFi_Platform.instance.Preset(rightSpeedController);
        }
    }
}
