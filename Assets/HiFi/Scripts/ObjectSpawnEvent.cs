using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiFi
{
    public class ObjectSpawnEvent : MonoBehaviour
    {
        public static Action<GameObject> Spawned = (obj) => { };

        void Start()
        {
            Debug.Log("Firing ObjectSpawnEvent for " + gameObject.name);
            Spawned(this.gameObject);
        }
    }
}
