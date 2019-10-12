using System;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

namespace HiFi
{
    public class PlayerObjectSpawnEvent : MonoBehaviour
    {
        public static Action<GameObject, int, bool> ObjectSpawned;
        public static Action<GameObject, int> ObjectDespawned;
        public bool parentToPlayerOnSpawn = true;
        private int clientID;

        void Start()
        {
            RealtimeView realtimeView = GetComponent<RealtimeView>();
            if (realtimeView != null)
                clientID = realtimeView.ownerID;
            else
            {
                clientID = 0;
                Debug.Log(gameObject.name + " has no RealtimeView attached and has been assigned to player 0.");
            }

            ObjectSpawned(gameObject, clientID, parentToPlayerOnSpawn);
        }

        private void OnDisable()
        {
            ObjectDespawned(gameObject, clientID);
        }
    }
}
