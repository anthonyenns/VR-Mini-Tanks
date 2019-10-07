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

        public float leftSpeed;
        public float rightSpeed;
        public bool tankIsLocal;

        private TankSync _tankSync;
        private RealtimeView _realtimeView;
        private RealtimeTransform _realtimeTransform;

        public bool inputLocked;
        private Vector2 speedInput;

        private void Start()
        {
            _realtimeView = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();
            _tankSync = GetComponent<TankSync>();

            cameraNull.transform.localPosition = avatarSeatPosition;
            cameraNull.transform.localRotation = Quaternion.identity;
        }

        void Update()
        {
            /// Do we own this tank?
            tankIsLocal = _realtimeTransform.isOwnedLocally;
            if (!tankIsLocal)
                return;

            speedInput.x = HiFi_Platform.instance.Preset(leftSpeedController);
            speedInput.y = HiFi_Platform.instance.Preset(rightSpeedController);

            /// Send values to tank model
            _tankSync.SetSpeeds(speedInput);
        }
    }
}
