using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HiFi.Platform;

namespace HiFi
{
    public class TankProjectileSpawner : MonoBehaviour
    {
        public HiFi_PresetButtonInput fire;
        public GameObject ammoPrefab;
        public GameObject gunBarrel;
        public GameObject firePoint;
        public float force = 10;

        private Rigidbody rb;
        private GameObject projectile;
        private LineRenderer lineRenderer;
        private float h;

        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            h = Time.fixedDeltaTime;
        }

        void Update()
        {
            if(HiFi_Platform.instance.Preset(fire) || Input.GetKeyDown(KeyCode.M))
            {
                projectile = Instantiate(ammoPrefab, firePoint.transform.position, Quaternion.identity);
                rb = projectile.GetComponent<Rigidbody>();

                rb.AddForce(gunBarrel.transform.up * force, ForceMode.Impulse);
            }
        }

        /*
        //Display the trajectory path with a line renderer
        void DrawTrajectoryPath()
        {
            //How long did it take to hit the target?
            float timeToHitTarget = CalculateTimeToHitTarget();

            //How many segments we will have
            int maxIndex = Mathf.RoundToInt(timeToHitTarget / h);

            lineRenderer.SetVertexCount(maxIndex);

            //Start values
            Vector3 currentVelocity = gunBarrel.transform.up * bulletSpeed;
            Vector3 currentPosition = firePoint.transform.position;

            Vector3 newPosition = Vector3.zero;
            Vector3 newVelocity = Vector3.zero;

            //Build the trajectory line
            for (int index = 0; index < maxIndex; index++)
            {
                lineRenderer.SetPosition(index, currentPosition);

                //Calculate the new position of the bullet
                TutorialBallistics.CurrentIntegrationMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity);

                currentPosition = newPosition;
                currentVelocity = newVelocity;
            }
        }

        //How long did it take to reach the target (splash in artillery terms)?
        public float CalculateTimeToHitTarget()
        {
            //Init values
            Vector3 currentVelocity = gunBarrel.transform.up * bulletSpeed;
            Vector3 currentPosition = firePoint.transform.position;

            Vector3 newPosition = Vector3.zero;
            Vector3 newVelocity = Vector3.zero;

            //The total time it will take before we hit the target
            float time = 0f;

            //Limit to 30 seconds to avoid infinite loop if we never reach the target
            for (time = 0f; time < 30f; time += h)
            {
                TutorialBallistics.CurrentIntegrationMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity);

                //If we are moving downwards and are below the target, then we have hit
                if (newPosition.y < currentPosition.y && newPosition.y < targetObj.position.y)
                {
                    //Add 2 times to make sure we end up below the target when we display the path
                    time += h * 2f;

                    break;
                }

                currentPosition = newPosition;
                currentVelocity = newVelocity;
            }

            return time;
        }

        //Easier to change integration method once in this method
        public static void CurrentIntegrationMethod(
            float h,
            Vector3 currentPosition,
            Vector3 currentVelocity,
            out Vector3 newPosition,
            out Vector3 newVelocity)
        {
            //IntegrationMethods.EulerForward(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
            //IntegrationMethods.Heuns(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
            //IntegrationMethods.RungeKutta(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
            IntegrationMethods.BackwardEuler(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
        }
        */
    }
}