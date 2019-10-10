﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

namespace HiFi
{
    public class TankSpawnEvent : MonoBehaviour
    {
        public static Action<GameObject, int> TankSpawned;
        public static Action<GameObject, int> TankDespawned;
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

            TankSpawned(gameObject, clientID);
        }

        private void OnDisable()
        {
            TankDespawned(gameObject, clientID);
        }
    }
}
