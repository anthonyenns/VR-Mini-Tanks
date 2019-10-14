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

        [Header("Game References")]
        [SerializeField] GameObject localTank;
        [SerializeField] List<GameObject> playerTanks = new List<GameObject>();
        public List<GameObject> myActiveProjectiles = new List<GameObject>();

        [Header("Utility Lists")]
        [SerializeField] List<PlayerObject> waitingForParent = new List<PlayerObject>();
        [SerializeField] List<GameObject> tanksWaitingList = new List<GameObject>();

        [Header("Settings")]
        public HiFi_PresetButtonInput recenterButton;

        public int PlayerID { get { return playerID; } }
        public Realtime RealtimeInstance { get { return realtime; } }

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
            Projectile.ProjectileHit += ProjectileHitHandler;
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
            Projectile.ProjectileHit -= ProjectileHitHandler;
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
                for (int i = waitingForParent.Count - 1; i >= 0; i--)
                {
                    if (playerTanks[waitingForParent[i].id] != null)
                    {
                        SetParent(playerTanks[waitingForParent[i].id], waitingForParent[i].obj);
                        HiFi_Utilities.DebugText("Removing " + waitingForParent[i].obj.name + " from waiting list.");
                        waitingForParent.RemoveAt(i);
                    }
                }
            }

            /// Tanks waiting for ID to update
            for (int i = tanksWaitingList.Count - 1; i >= 0; i--)
            {
                RealtimeView view = tanksWaitingList[i].GetComponent<RealtimeView>();
                if (view.ownerID != playerID)
                {
                    if (playerTanks[view.ownerID] == null)
                    {
                        tanksWaitingList[i].name = "Player_" + view.ownerID + "_Tank";
                        playerTanks[view.ownerID] = tanksWaitingList[i];
                        tanksWaitingList.RemoveAt(i);
                        HiFi_Utilities.DebugText("Tank on waiting list with ID " + view.ownerID + " has been added to playerTanks list.");
                    }
                    else
                    {
                        HiFi_Utilities.DebugText("PLAYER TANK WITH ID " + view.ownerID + " ALREADY EXISTS - ABORTING");
                    }
                }
            }
        }

        #region Public Methods
        /// <summary>
        /// Set local tank reference.
        /// </summary>
        /// <param name="tank"></param>
        public void AddLocalTank(GameObject tank, int id)
        {
            HiFi_Utilities.DebugText("Adding Local Tank reference at ID " + id);
            tank.name = "Player_" + id + "_Tank";
            localTank = tank;
            playerTanks[id] = tank;
        }

        /// <summary>
        /// Returns Player ID if a matching tank gameObject is found in list
        /// </summary>
        /// <param name="tank"></param>
        /// <returns></returns>
        public int IdentifyPlayer(GameObject tank)
        {
            int id = -1;

            if (tank != null)
            {
                for (int i = 0; i < playerTanks.Count; i++)
                {
                    if (playerTanks[i] = tank)
                        id = i;
                }
            }
            return id;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Adds spawned tank to GameControl list of tanks and renames.
        /// </summary>
        /// <param name="tank"></param>
        /// <param name="id"></param>
        private void AddTankToList(GameObject tank)
        {
            if (tank == localTank)
                return;

            RealtimeView view = tank.GetComponent<RealtimeView>();

            if (view != null)
            {
                if (view.ownerID == playerID)
                {
                    tanksWaitingList.Add(tank);
                    HiFi_Utilities.DebugText("Adding Tank to waiting list");
                }
                else if (playerTanks[view.ownerID] == null)
                {
                    tank.name = "Player_" + view.ownerID + "_Tank";
                    playerTanks[view.ownerID] = tank;
                }
                else
                {
                    HiFi_Utilities.DebugText("PLAYER TANK WITH ID " + view.ownerID + " ALREADY EXISTS - ABORTING");
                }
            }
            else
            {
                /// Offline default player 0
                AddLocalTank(tank, 0);
            }
        }

        /// <summary>
        /// Removes tank from GameControl tank list.
        /// </summary>
        /// <param name="tank"></param>
        /// <param name="id"></param>
        private void RemoveTankFromList(GameObject tank)
        {
            int id = 0;
            RealtimeView view = tank.GetComponent<RealtimeView>();
            if (view != null)
                id = view.ownerID;

            HiFi_Utilities.DebugText("Removing Tank ID " + id);
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
        private void PlayerObjectHandler(GameObject obj, bool parentToPlayer)
        {
            int id = 0;
            RealtimeView view = obj.GetComponent<RealtimeView>();

            if (view != null)
                id = view.ownerID;

            HiFi_Utilities.DebugText("PlayerObject has spawned with ID " + id);

            obj.name = "Player_" + id + "_" + obj.name;

            if (parentToPlayer)
            {
                if (playerTanks[id] == null)
                {
                    PlayerObject pObj = new PlayerObject();
                    pObj.id = id;
                    pObj.obj = obj;

                    waitingForParent.Add(pObj);

                    HiFi_Utilities.DebugText(obj.name + " is added to waiting list");
                }
                else
                {
                    SetParent(playerTanks[id], obj);
                }
            }
        }

        private void ProjectileHitHandler(GameObject projectile, GameObject hitObj, int projectileID)
        {
            /// See who/what we hit
            if (hitObj.CompareTag("Player"))
            {
                /// Identify the player hit
                int hitID = IdentifyPlayer(hitObj.GetComponentInParent<TankController>().gameObject);

                /// Messages
                if (hitID == PlayerID)
                    HiFi_Utilities.DebugText("You've been hit! By Player " + projectileID);
                else
                    HiFi_Utilities.DebugText("Player " + hitID + " has been hit! By Player " + projectileID);
            }

            /// If this is our projectile remove it from active list
            for (int i = 0; i < myActiveProjectiles.Count; i++)
            {
                if (projectile == myActiveProjectiles[i])
                    myActiveProjectiles.RemoveAt(i);
            }
        }

        /// <summary>
        /// Unused for now
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        private void RemovePlayerObject(GameObject obj)
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
            HiFi_Utilities.DebugText("Connected -- Realtime ClientID " + realtime.clientID);
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
            /// Special parenting instructions for VR Avatar (parent to Camera Null)
            if (child.CompareTag("VRAvatar"))
            {
                TankController tc = parent.GetComponent<TankController>();
                child.transform.parent = tc.cameraNull.transform;
                child.transform.localPosition = Vector3.zero;
                child.transform.localRotation = Quaternion.identity;
                InputTracking.Recenter();
            }
            else
            {   /// Normal centered parenting
                child.transform.parent = parent.transform;
                child.transform.localPosition = Vector3.zero;
                child.transform.localRotation = Quaternion.identity;
            }
        }

        /// <summary>
        /// What to do when shutting down the game
        /// </summary>
        private void ShutdownSequence()
        {
            Application.Quit();
        }

        #endregion
    }
}
