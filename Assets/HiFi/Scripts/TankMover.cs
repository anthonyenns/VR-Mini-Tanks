using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using HiFi.Platform;

namespace HiFi
{
    public class TankMover : MonoBehaviour
    {
        public HiFi_PresetAxisInput leftThrottleInput;
        public HiFi_PresetAxisInput rightThrottleInput;

        public float turnSpeed = 15.0f;
        public float forwardSpeed = 0.5f;

        float leftThrottle, rightThrottle, sumThrottle, leftThrottlePos, rightThrottlePos;
        Rigidbody rb;

        private RealtimeView _realtimeView;
        private RealtimeTransform _realtimeTransform;

        private void Awake()
        {
            _realtimeView = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();

            rb = GetComponentInChildren<Rigidbody>();

            Camera.main.transform.parent = transform;
            //HiFi_Platform.instance.SetTrackingSpace(1, true);

           // ObjectEnableEvent.objectEnabled += 
        }

        private void Update()
        {
            /// If this prefab is not owned by this client, bail.
            if (!_realtimeView.isOwnedLocally)
                return;

            /// Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
            _realtimeTransform.RequestOwnership();

            leftThrottle = HiFi_Platform.instance.Preset(leftThrottleInput);
            rightThrottle = HiFi_Platform.instance.Preset(rightThrottleInput);
        }

        private void FixedUpdate()
        {
            Turn();
            Move();
        }

        private void Turn()
        {
            float turn = leftThrottle * turnSpeed * Time.deltaTime - rightThrottle * turnSpeed * Time.deltaTime;

            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

            rb.MoveRotation(rb.rotation * turnRotation);
        }

        private void Move()
        {
            sumThrottle = 0;

            if (leftThrottle < 0 && rightThrottle < 0)
                sumThrottle = leftThrottle >= rightThrottle ? leftThrottle : rightThrottle;

            if (leftThrottle > 0 && rightThrottle > 0)
                sumThrottle = leftThrottle <= rightThrottle ? leftThrottle : rightThrottle;

            Vector3 movement = rb.transform.forward * sumThrottle * forwardSpeed * Time.deltaTime;

            rb.MovePosition(rb.position + movement);
        }


    }
}
