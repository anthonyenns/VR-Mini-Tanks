using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiFi
{
    public class TankEnabled : MonoBehaviour
    {
        public static Action<GameObject> OnTankEnabled = (tank) => { };

        void Start()
        {
            Debug.Log("Firing TankEnabled");
            OnTankEnabled(this.gameObject);
        }
    }
}
