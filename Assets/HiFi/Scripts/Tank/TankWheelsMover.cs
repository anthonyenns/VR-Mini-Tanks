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
        
        public Material tracksMat;
        //public Material RightTrack;

        public float trackMult = 0.5f;
        public float wheelsMult = 5.0f;
        private float leftTrackSpeed, rightTrackSpeed;
        private Rigidbody rb;

        private void Awake()
        {
            controller = GetComponent<TankController>();
            rb = GetComponentInChildren<Rigidbody>();
            if (rb == null)
                rb = GetComponentInParent<Rigidbody>();
        }

        void Update()
        {
            if ((controller.leftSpeedNetworkReturn != 0 || controller.rightSpeedNetworkReturn != 0) && controller.ownedLocally)
            {
                MoveWheels();
            }
        }

        private void MoveWheels()
        {
            leftTrackSpeed = controller.leftSpeedNetworkReturn + (controller.rightSpeedNetworkReturn * 0.5f);
            rightTrackSpeed = controller.rightSpeedNetworkReturn + (controller.leftSpeedNetworkReturn * 0.5f);

            /// Left wheels rotate
            foreach (GameObject wheelL in LeftWheels)
                wheelL.transform.Rotate(new Vector3(leftTrackSpeed, 0f, 0f) * wheelsMult);

            /// Right wheels rotate
            foreach (GameObject wheelR in RightWheels)
                wheelR.transform.Rotate(new Vector3(-rightTrackSpeed, 0f, 0f) * wheelsMult);

            /// Tracks texture offsets
            if (controller.leftSpeedNetworkReturn + controller.rightSpeedNetworkReturn < 0)
                tracksMat.mainTextureOffset += new Vector2(Time.deltaTime * trackMult, 0f);

            if (controller.leftSpeedNetworkReturn + controller.rightSpeedNetworkReturn > 0)
                tracksMat.mainTextureOffset -= new Vector2(Time.deltaTime * trackMult, 0f);

            //RightTrack.mainTextureOffset += new Vector2(0f, Time.deltaTime * rightTrackSpeed * trackMult);
        }
    }
}