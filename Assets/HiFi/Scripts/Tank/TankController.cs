using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using HiFi.Platform;

namespace HiFi
{
    public class TankController : MonoBehaviour
    {
        public HiFi_PresetAxisInput leftSpeedController;
        public HiFi_PresetAxisInput rightSpeedController;
        public HiFi_PresetButtonInput lockInput;

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
        }

        void Update()
        {
            /// Do we own this tank?
            tankIsLocal = _realtimeTransform.isOwnedLocally;
            if (!tankIsLocal)
                return;

            if (HiFi_Platform.instance.Preset(lockInput))
            {
                inputLocked = !inputLocked;
                Debug.Log("lockInput pressed");
            }

            if (!inputLocked)
            {
                speedInput.x = HiFi_Platform.instance.Preset(leftSpeedController);
                speedInput.y = HiFi_Platform.instance.Preset(rightSpeedController);
            }

            /// Send values to tank model
            _tankSync.SetSpeeds(speedInput);
        }
    }
}
