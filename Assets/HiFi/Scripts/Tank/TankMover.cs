using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiFi
{
    public class TankMover : MonoBehaviour
    {
        public TankController controller;

        public float turnSpeed = 15.0f;
        public float forwardSpeed = 0.5f;

        float sumThrottle;
        Rigidbody rb;
        Vector3 movement;

        private void Start()
        {
            rb = GetComponentInChildren<Rigidbody>();
        }

        private void Update()
        {
            /// If this prefab is not owned by this client, bail.
            if (!controller.tankIsLocal)
                return;

            /// Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
            //_realtimeTransform.RequestOwnership();
        }

        private void FixedUpdate()
        {
            if (rb != null)
            {
                Turn();
                Move();
            }
        }

        private void Turn()
        {
            float turn = (controller.leftSpeed * turnSpeed * Time.deltaTime) - (controller.rightSpeed * turnSpeed * Time.deltaTime);

            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

            rb.MoveRotation(rb.rotation * turnRotation);
        }

        private void Move()
        {
            sumThrottle = 0;

            if (controller.leftSpeed < 0 && controller.rightSpeed < 0)
                sumThrottle = controller.leftSpeed >= controller.rightSpeed ? controller.leftSpeed : controller.rightSpeed;

            if (controller.leftSpeed > 0 && controller.rightSpeed > 0)
                sumThrottle = controller.leftSpeed <= controller.rightSpeed ? controller.leftSpeed : controller.rightSpeed;

            movement = rb.transform.forward * sumThrottle * forwardSpeed * Time.deltaTime;

            rb.MovePosition(rb.position + movement);
        }


    }
}
