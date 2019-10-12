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
        public static GameControl instance = null;

        [Header("Networking")]
        public bool realtimeEnabled = true;
        public int maxPlayers = 16;
        [SerializeField] int playerID = -1; /// fetched from Realtime ClientID
        [SerializeField] bool connectedToRoom;
        [SerializeField] GameObject realtimeSetup;
        [SerializeField] GameObject realtimeNoVRSetup;
        [SerializeField] GameObject offlineSetup;
        private Realtime realtime;

        [Header("References")]
        [SerializeField] GameObject localTank;
        [SerializeField] List<GameObject> playerTanks = new List<GameObject>();
        [SerializeField] List<PlayerObject> waitingForParent = new List<PlayerObject>();

        [Header("Settings")]
        public HiFi_PresetButtonInput recenterButton;


        private void OnEnable()
        {
            if (instance == null)
                instance = this;

            /// Realtime
            if (realtimeEnabled && XRSettings.enabled)
            {
                realtimeSetup.SetActive(true);
                realtimeNoVRSetup.SetActive(false);
                offlineSetup.SetActive(false);

                realtime = realtimeSetup.GetComponent<Realtime>();
                realtime.didConnectToRoom += ConnectedToRoom;
                realtime.didDisconnectFromRoom += DisconnectedFromRoom;

                playerTanks.Capacity = maxPlayers;
                for (int i = 0; i < maxPlayers; i++)
                {
                    playerTanks.Add(null);
                }
            }
            /// Realtime no VR
            else if (realtimeEnabled && !XRSettings.enabled)
            {
                realtimeSetup.SetActive(false);
                realtimeNoVRSetup.SetActive(true);
                offlineSetup.SetActive(false);

                realtime = realtimeNoVRSetup.GetComponent<Realtime>();
                realtime.didConnectToRoom += ConnectedToRoom;
                realtime.didDisconnectFromRoom += DisconnectedFromRoom;

                playerTanks.Capacity = maxPlayers;
                for (int i = 0; i < maxPlayers; i++)
                {
                    playerTanks.Add(null);
                }

            }
            /// No Realtime (offline)
            else if (!realtimeEnabled)
            {
                realtimeSetup.SetActive(false);
                realtimeNoVRSetup.SetActive(false);
                offlineSetup.SetActive(true);

                playerID = 0;

                playerTanks.Add(null);
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
            if (waitingForParent.Count > 0)
            {
                Debug.Log("Attempting to parent player items on waiting list.");
                for (int i = waitingForParent.Count - 1; i >= 0; i--)
                {
                    if(playerTanks[waitingForParent[i].id] != null)
                    {
                        SetParent(playerTanks[waitingForParent[i].id], waitingForParent[i].obj);
                        Debug.Log("Removing " + waitingForParent[i].obj.name + " from waiting list.");
                        waitingForParent.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// Adds spawned tank to GameControl list of tanks and renames. Also caches local reference.
        /// </summary>
        /// <param name="tank"></param>
        /// <param name="id"></param>
        private void AddTankToList(GameObject tank, int id)
        {
            tank.name = "Player_" + id + "_Tank";
            playerTanks[id] = tank;

            if (id == playerID)
                localTank = tank;
        }

        /// <summary>
        /// Removes tank from GameControl tank list.
        /// </summary>
        /// <param name="tank"></param>
        /// <param name="id"></param>
        private void RemoveTankFromList(GameObject tank, int id)
        {
            playerTanks[id] = null;
            if (id == playerID)
                localTank = null;
        }

        /// <summary>
        /// Renames spawned object with player ID and parents to player tank if necessary
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <param name="parentToPlayer"></param>
        private void PlayerObjectHandler(GameObject obj, int id, bool parentToPlayer)
        {
            obj.name = "Player_" + id + "_" + obj.name;

            if (parentToPlayer)
            {
                if (playerTanks[id] == null)
                {
                    PlayerObject pObj = new PlayerObject();
                    pObj.id = id;
                    pObj.obj = obj;

                    waitingForParent.Add(pObj);

                    Debug.Log(obj.name + " is added to waiting list");
                }
                else
                {
                    SetParent(playerTanks[id], obj);
                }
            }
        }

        /// <summary>
        /// Unused for now
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        private void RemovePlayerObject(GameObject obj, int id)
        {
            /// Unused
        }

        /// <summary>
        /// What to do when connected to Realtime room
        /// </summary>
        /// <param name="realtime"></param>
        private void ConnectedToRoom(Realtime realtime)
        {
            playerID = realtime.clientID;
            connectedToRoom = true;
        }

        /// <summary>
        /// What do to when disconnected from Realtime room
        /// </summary>
        /// <param name="realtime"></param>
        private void DisconnectedFromRoom(Realtime realtime)
        {
            playerID = -1;
            connectedToRoom = false;
        }

        /// <summary>
        /// Parents and aligns objects. Checks for VRAvatar tag and sets to TankController camera position.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        private void SetParent(GameObject parent, GameObject child)
        {
            child.transform.parent = parent.transform;
            child.transform.localPosition = Vector3.zero;
            child.transform.localRotation = Quaternion.identity;

            if(child.CompareTag("VRAvatar"))
            {
                TankController tc = parent.GetComponent<TankController>();
                child.transform.localPosition = tc.avatarSeatPosition;
                InputTracking.Recenter();
            }
        }

        /// <summary>
        /// What to do when shutting down the game
        /// </summary>
        private void ShutdownSequence()
        {
            Application.Quit();
        }
    }
}
