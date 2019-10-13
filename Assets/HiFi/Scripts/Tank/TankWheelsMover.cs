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

        float leftTrackSpeed, rightTrackSpeed;
        public float trackMult = 0.5f;
        public float wheelsMult = 5.0f;
        private float avg;

        private void Awake()
        {
            controller = GetComponent<TankController>();
        }

        void Update()
        {
            if ((controller.leftSpeed != 0 || controller.rightSpeed != 0) && controller.ownedLocally)
            {
                MoveWheels();
            }
        }

        private void MoveWheels()
        {
            leftTrackSpeed = controller.leftSpeed + (controller.rightSpeed * 0.5f);
            rightTrackSpeed = controller.rightSpeed + (controller.leftSpeed * 0.5f);

            /// Left wheels rotate
            foreach (GameObject wheelL in LeftWheels)
                wheelL.transform.Rotate(new Vector3(leftTrackSpeed, 0f, 0f) * wheelsMult);

            /// Right wheels rotate
            foreach (GameObject wheelR in RightWheels)
                wheelR.transform.Rotate(new Vector3(-rightTrackSpeed, 0f, 0f) * wheelsMult);

            /// Tracks texture offsets
            LeftTrack.transform.GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0f, Time.deltaTime * leftTrackSpeed * trackMult);
            RightTrack.transform.GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0f, Time.deltaTime * rightTrackSpeed * trackMult);
        }
    }
}