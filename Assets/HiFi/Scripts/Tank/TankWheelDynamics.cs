using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiFi
{
    public class TankWheelDynamics : MonoBehaviour
    {
        public TankController controller;
        public List<Transform> leftWheels = new List<Transform>();
        public List<Transform> rightWheels = new List<Transform>();
        public bool debugGizmos;

        [Header("Wheel Dynamics Settings")]
        public LayerMask wheelsMask;
        public float castDistance;
        public float castRadius;
        public float maxTravel;
        public float minGroundDistance;
        public float lerpDownRate;
        public float lerpUpRate;

        [Header("Info")]
        public float sumLeft, sumRight;

        private List<WheelData> leftData = new List<WheelData>();
        private List<WheelData> rightData = new List<WheelData>();
        private RaycastHit hit;
        private Vector3 direction;
        private float zTarget;

        void Awake()
        {
            /// Initialize 
            for (int i = 0; i < leftWheels.Count; i++)
            {
                WheelData wheel = new WheelData();
                leftData.Add(wheel);
                leftData[i].zStart = leftWheels[i].localPosition.z;
                leftData[i].zPosition = leftData[i].zStart;
                leftData[i].top = leftData[i].zStart + maxTravel;
                leftData[i].bottom = leftData[i].zStart - maxTravel;
            }
            for (int i = 0; i < rightWheels.Count; i++)
            {
                WheelData wheel = new WheelData();
                rightData.Add(wheel);
                rightData[i].zStart = rightWheels[i].localPosition.z;
                rightData[i].zPosition = rightData[i].zStart;
                rightData[i].top = rightData[i].zStart + maxTravel;
                rightData[i].bottom = rightData[i].zStart - maxTravel;
            }

        }

        private void Update()
        {
            CalculateForcePool();

            FindGround(leftWheels, leftData);
            FindGround(rightWheels, rightData);

        }

        private void FindGround(List<Transform> wheels, List<WheelData> data)
        {
            for (int i = 0; i < wheels.Count; i++)
            {
                /// Reset
                data[i].hitTransform = null;

                /// Find normalized position (0 bottom, 1 top)
                data[i].normalizedPosition = (data[i].zPosition - data[i].bottom) / (data[i].top - data[i].bottom);

                /// Raycast for terrain
                direction = wheels[i].TransformDirection(Vector3.back);
                if (Physics.SphereCast(wheels[i].position, castRadius, direction, out hit, castDistance, wheelsMask))
                {
                    if (hit.distance <= minGroundDistance)
                        zTarget = data[i].zPosition + (hit.distance - castDistance);

                }


                /// Lerp toward target
                if (true) // need to go down
                {
                    data[i].zPosition = Mathf.Lerp(data[i].zPosition, zTarget, lerpDownRate);
                }
                else // need to go up
                {
                    data[i].zPosition = Mathf.Lerp(data[i].zPosition, zTarget, lerpUpRate);
                }

                /// Clamp position

                /// Apply new position
                wheels[i].localPosition = new Vector3(wheels[i].localPosition.x, wheels[i].localPosition.y, data[i].zPosition);




                /// Lerp toward ground, each
                /// 
                /// Calculate pool deficit +/-
                /// 
                /// Lerp to compensate pool, all




            }
        }

        private void CalculateForcePool()
        {
            sumLeft = 0;
            sumRight = 0;

            for (int i = 0; i < leftData.Count; i++)
                sumLeft += leftData[i].normalizedPosition;
            for (int i = 0; i < rightData.Count; i++)
                sumRight += rightData[i].normalizedPosition;

            sumLeft -= leftData.Count * 0.5f;
            sumRight -= rightWheels.Count * 0.5f;
        }

        private void OnDrawGizmos()
        {
            if (debugGizmos)
            {
                foreach (Transform wheel in leftWheels)
                {
                    Debug.DrawRay(transform.position, direction);
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(wheel.position, castRadius);
                    Gizmos.DrawWireSphere(wheel.position - (direction * castDistance), castRadius);
                }

                foreach (Transform wheel in rightWheels)
                {
                    Debug.DrawRay(transform.position, direction);
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(wheel.position, castRadius);
                    Gizmos.DrawWireSphere(wheel.position - (direction * castDistance), castRadius);
                }
            }
        }

    }
}
