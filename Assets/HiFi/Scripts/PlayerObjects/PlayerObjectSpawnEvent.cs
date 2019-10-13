using System;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

namespace HiFi
{
    public class PlayerObjectSpawnEvent : MonoBehaviour
    {
        public static Action<GameObject, bool> ObjectSpawned;
        public static Action<GameObject> ObjectDespawned;
        public bool parentToPlayerOnSpawn = true;

        void Start()
        {
            ObjectSpawned(gameObject, parentToPlayerOnSpawn);
        }

        private void OnDisable()
        {
            ObjectDespawned(gameObject);
        }
    }
}
