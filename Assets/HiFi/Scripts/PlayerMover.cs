using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace HiFi
{
    namespace Platform
    {
        public class PlayerMover : MonoBehaviour
        {
            public HiFi_PresetAxisInput leftThrottleInput;
            public HiFi_PresetAxisInput rightThrottleInput;

            public float turnSpeed = 15.0f;
            public float forwardSpeed = 2.0f;

            float leftThrottle, rightThrottle, sumThrottle, leftThrottlePos, rightThrottlePos;
            Rigidbody rb;

            private void Awake()
            {
                rb = GetComponent<Rigidbody>();
            }

            private void Update()
            {
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
                // Determine the number of degrees to be turned based on the input, speed and time between frames.
                float turn = leftThrottle * turnSpeed * Time.deltaTime - rightThrottle * turnSpeed * Time.deltaTime;

                // Make this into a rotation in the y axis.
                Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

                // Apply this rotation to the rigidbody's rotation.
                rb.MoveRotation(rb.rotation * turnRotation);
            }

            private void Move()
            {
                sumThrottle = 0;

                if(leftThrottle < 0 && rightThrottle < 0)
                    sumThrottle = leftThrottle >= rightThrottle ? leftThrottle : rightThrottle;

                if (leftThrottle > 0 && rightThrottle > 0)
                    sumThrottle = leftThrottle <= rightThrottle ? leftThrottle : rightThrottle;

                // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
                Vector3 movement = transform.forward * sumThrottle * forwardSpeed * Time.deltaTime;

                // Apply this movement to the rigidbody's position.
                rb.MovePosition(rb.position + movement);
            }

        }
    }
}