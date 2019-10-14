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

            if (gunHinge == null)
                gunHinge = transform;
        }

        void Update()
        {
            gunHinge.localEulerAngles = new Vector3(controller.gunAngle, gunHinge.localEulerAngles.y, gunHinge.localEulerAngles.z);
        }
    }
}
