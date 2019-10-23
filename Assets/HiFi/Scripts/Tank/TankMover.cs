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
            if(rb == null)
                rb = GetComponentInParent<Rigidbody>();
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
                Turn();
                Move();
            }
        }

        private void Turn()
        {
            /// Basic Turn Values
            leftTurn = (controller.leftSpeedNetworkReturn * turnSpeed * Time.deltaTime);
            rightTurn = (controller.rightSpeedNetworkReturn * turnSpeed * Time.deltaTime);

            /// Apply Turn
            turnSum = leftTurn - rightTurn;
            Quaternion turnRotation = Quaternion.Euler(0f, turnSum, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }

        private void Move()
        {
            sumThrottle = 0;

            sumThrottle = (controller.leftSpeedNetworkReturn + controller.rightSpeedNetworkReturn) * 0.5f;

            /// Apply Movement
            movement = rb.transform.forward * sumThrottle * forwardSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }

        public void Kickback(float strength)
        {
            Quaternion turnRotation = Quaternion.Euler(-strength, 0f, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }
}
