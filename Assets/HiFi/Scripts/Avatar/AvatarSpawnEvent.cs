using System;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

namespace HiFi
{
    public class AvatarSpawnEvent : MonoBehaviour
    {
        public static Action<GameObject> AvatarSpawned;
        public static Action<GameObject> AvatarDespawned;

        void Start()
        {
            AvatarSpawned(gameObject);
        }

        private void OnDisable()
        {
            AvatarDespawned(gameObject);
        }
    }
}
