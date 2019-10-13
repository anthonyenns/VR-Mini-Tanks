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

        private float sumThrottle, leftTurn, rightTurn, turnSum;
        private Rigidbody rb;
        private Vector3 movement;
        private bool sameDirection, leftDominant, rightDominant;

        private void Awake()
        {
            rb = GetComponentInChildren<Rigidbody>();
            controller = GetComponent<TankController>();
        }

        private void Start()
        {
            if (controller.realtimeView != null)
                controller.realtimeView.RequestOwnership();
        }

        private void FixedUpdate()
        {
            if (rb != null && controller.ownedLocally)
            {
                //Tests();

                Turn();
                Move();
            }
        }

        private void Turn()
        {
            /// Basic Turn Values
            leftTurn = (controller.leftSpeed * turnSpeed * Time.deltaTime);
            rightTurn = (controller.rightSpeed * turnSpeed * Time.deltaTime);

            /// Apply Turn
            turnSum = leftTurn - rightTurn;
            Quaternion turnRotation = Quaternion.Euler(0f, turnSum, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }

        private void Move()
        {
            sumThrottle = 0;

            sumThrottle = (controller.leftSpeed + controller.rightSpeed) * 0.5f;

            /// Apply Movement
            movement = rb.transform.forward * sumThrottle * forwardSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }

        private void Tests()
        {
            sameDirection = false;
            leftDominant = false;
            rightDominant = false;

            /// Both Tracks Same Direction ?
            if ((controller.leftSpeed > 0 && controller.rightSpeed > 0) || (controller.leftSpeed < 0 && controller.rightSpeed < 0))
                sameDirection = true;

            /// Left or Right Dominant ?
            if (Mathf.Abs(controller.leftSpeed) > Mathf.Abs(controller.rightSpeed))
                leftDominant = true;
            if (Mathf.Abs(controller.leftSpeed) < Mathf.Abs(controller.rightSpeed))
                rightDominant = true;
        }
    }
}
