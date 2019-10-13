using System;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

namespace HiFi
{
    public class TankSpawnEvent : MonoBehaviour
    {
        public static Action<GameObject> TankSpawned;
        public static Action<GameObject> TankDespawned;

        void Start()
        {
            TankSpawned(gameObject);
        }

        private void OnDisable()
        {
            TankDespawned(gameObject);
        }
    }
}
