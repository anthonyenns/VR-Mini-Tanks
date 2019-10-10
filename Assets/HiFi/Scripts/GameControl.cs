using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using HiFi.Platform;
using UnityEngine.XR;

namespace HiFi
{
    public class GameControl : MonoBehaviour
    {
        [Header("Newtorking")]
        public bool realtimeEnabled = true;
        [SerializeField] Realtime realtime;
        public int maxPlayersInRoom = 16;
        [SerializeField] int playerID = -1; /// from Realtime ClientID
        [SerializeField] bool connectedToRoom;
        [SerializeField] GameObject offline;

        [Header("References")]
        [SerializeField] GameObject localTank;
        [SerializeField] List<GameObject> playerTanks = new List<GameObject>();

        [Header("Settings")]
        public HiFi_PresetButtonInput recenterButton;

        private bool objectsWaitingForParent = false;
        private TankController clientTankNeedsAvatar;


        private void OnEnable()
        {
            /// Realtime
            if (realtimeEnabled)
            {
                realtime.didConnectToRoom += ConnectedToRoom;
                realtime.didDisconnectFromRoom += DisconnectedFromRoom;

                playerTanks.Capacity = maxPlayersInRoom;
            }
            else /// No networking
            {
                realtime.gameObject.SetActive(false);
                offline.SetActive(true);
                playerID = 0;

                playerTanks.Capacity = 1;
            }
            /// Player Objects Spawn Events
            TankSpawnEvent.TankSpawned += AddTankToList;
            TankSpawnEvent.TankDespawned += RemoveTankFromList;
            PlayerObjectSpawnEvent.ObjectSpawned += PlayerObjectHandler;
            PlayerObjectSpawnEvent.ObjectDespawned += RemovePlayerObject;

        }

        private void OnDisable()
        {
            /// Realtime
            if (realtimeEnabled)
            {
                realtime.didConnectToRoom -= ConnectedToRoom;
                realtime.didDisconnectFromRoom -= DisconnectedFromRoom;
            }

            /// Player Objects Spawn Events
            TankSpawnEvent.TankSpawned -= AddTankToList;
            TankSpawnEvent.TankDespawned -= RemoveTankFromList;
            PlayerObjectSpawnEvent.ObjectSpawned -= PlayerObjectHandler;
            PlayerObjectSpawnEvent.ObjectDespawned -= RemovePlayerObject;
        }

        void Update()
        {
            /// Exit game
            if (Input.GetKey("escape"))
                ShutdownSequence();

            /// HMD Recenter
            if (HiFi_Platform.instance.Preset(recenterButton))
                InputTracking.Recenter();

            /// Parent Local PlayerObject to Tank
            if (objectsWaitingForParent)
                ();

            /// Parent Network PlayerObject
            if (clientTankNeedsAvatar != null)
                ();
        }

        private void AddTankToList(GameObject tank, int id)
        {
            tank.name = "Player_" + id + "_Tank";
            playerTanks[id] = tank;

            if (id == playerID)
                localTank = tank;
        }

        private void RemoveTankFromList(GameObject tank, int id)
        {
            playerTanks[id] = null;
            if (id == playerID)
                localTank = null;
        }

        private void PlayerObjectHandler(GameObject obj, int id, bool parentToPlayer)
        {
            obj.name = "Player_" + id + obj.name;

            if (parentToPlayer)
                obj.transform.parent = playerTanks[id].transform;
        }

        private void RemovePlayerObject(GameObject obj, int id)
        {
            /// Unused
        }

        private void ConnectedToRoom(Realtime realtime)
        {
            playerID = realtime.clientID;
            connectedToRoom = true;
        }

        private void DisconnectedFromRoom(Realtime realtime)
        {
            playerID = realtime.clientID;
            connectedToRoom = true;
        }

        private void ShutdownSequence()
        {
            Application.Quit();
        }
    }
}
