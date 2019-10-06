using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiFi
{
    public class TankWheelsMover : MonoBehaviour
    {
        public TankController controller;

        public GameObject[] LeftWheels;
        public GameObject[] RightWheels;

        public GameObject LeftTrack;
        public GameObject RightTrack;

        float wheelsSpeedLeft, tracksSpeedLeft, wheelsSpeedRight, tracksSpeedRight;
        public float speedMult = 0.5f;

        private void Start()
        {
            controller = GetComponent<TankController>();
        }

        void Update()
        {

            if (controller.leftSpeed != 0 || controller.rightSpeed != 0)
            {
                MoveWheels();
            }
        }

        private void MoveWheels()
        {
            wheelsSpeedLeft = controller.leftSpeed * speedMult;
            tracksSpeedLeft = controller.leftSpeed * speedMult;
            wheelsSpeedRight = controller.rightSpeed * speedMult;
            tracksSpeedRight = controller.rightSpeed * speedMult;

            //Left wheels rotate
            foreach (GameObject wheelL in LeftWheels)
            {
                wheelL.transform.Rotate(new Vector3(wheelsSpeedLeft, 0f, 0f));
            }
            //Right wheels rotate
            foreach (GameObject wheelR in RightWheels)
            {
                wheelR.transform.Rotate(new Vector3(-wheelsSpeedRight, 0f, 0f));
            }
            //left track texture offset
            LeftTrack.transform.GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0f, Time.deltaTime * tracksSpeedLeft);
            //right track texture offset
            RightTrack.transform.GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0f, Time.deltaTime * tracksSpeedRight);
        }
    }
}