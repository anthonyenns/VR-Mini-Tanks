using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiFi
{
    public class TankGunMover : MonoBehaviour
    {
        public TankController controller;
        public Transform gunHinge;

        private void Awake()
        {
            if(controller == null)
            controller = GetComponentInParent<TankController>();
        }

        void Update()
        {
            gunHinge.localEulerAngles = new Vector3(gunHinge.localEulerAngles.x, controller.gunAngleNetworkReturn, gunHinge.localEulerAngles.z);
        }
    }
}
