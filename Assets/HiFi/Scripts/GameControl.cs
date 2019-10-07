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
        public bool realtimeEnabled = true;
        [SerializeField] Realtime realtime;
        [SerializeField] int clientID = -1;
        [SerializeField] bool connectedToRoom;

        [SerializeField] GameObject localTank;
        [SerializeField] GameObject localAvatar;
        [SerializeField] List<GameObject> networkTanks = new List<GameObject>();
        [SerializeField] List<GameObject> networkAvatars = new List<GameObject>();

        public HiFi_PresetButtonInput recenterButton;

        private bool needToParentLocalAvatar = true;
        private TankController clientTankNeedsAvatar;
        private int clientTankNeedsAvatarID = -1;

        private void OnEnable()
        {
            /// Realtime Events
            realtime.didConnectToRoom += ConnectedToRoom;
            realtime.didDisconnectFromRoom += DisconnectedFromRoom;

            /// Player Objects Spawn Events
            TankSpawnEvent.TankSpawned += AddTankToList;
            TankSpawnEvent.TankDespawned += RemoveTankFromList;
            AvatarSpawnEvent.AvatarSpawned += AddAvatarToList;
            AvatarSpawnEvent.AvatarDespawned += RemoveAvatarFromList;

        }

        private void OnDisable()
        {
            realtime.didConnectToRoom -= ConnectedToRoom;
            realtime.didDisconnectFromRoom -= DisconnectedFromRoom;

            TankSpawnEvent.TankSpawned -= AddTankToList;
            TankSpawnEvent.TankDespawned -= RemoveTankFromList;
            AvatarSpawnEvent.AvatarSpawned -= AddAvatarToList;
            AvatarSpawnEvent.AvatarDespawned -= RemoveAvatarFromList;
        }

        void Update()
        {
            /// Exit game
            if (Input.GetKey("escape"))
                ShutdownSequence();

            /// HMD Recenter
            if (HiFi_Platform.instance.Preset(recenterButton))
                InputTracking.Recenter();

            /// Parent Local Avatar to Tank
            if (needToParentLocalAvatar)
                ParentLocalAvatar();

            /// Parent Network Avatar
            if (clientTankNeedsAvatar != null)
                ParentClientAvatar();
        }

        private void AddTankToList(GameObject tank)
        {
            if (tank.GetComponent<RealtimeView>().isOwnedLocally)
            {
                tank.name = "local_" + tank.name;
                localTank = tank;
            }
            else
            {
                clientTankNeedsAvatar = tank.GetComponent<TankController>();
                clientTankNeedsAvatarID = tank.GetComponent<RealtimeView>().ownerID;

                tank.name = "Client" + clientTankNeedsAvatarID + "_" + tank.name;
                networkTanks.Add(tank);              
            }
        }

        private void RemoveTankFromList(GameObject tank)
        {
            if (tank != localTank)
                networkTanks.Remove(tank);
        }

        private void AddAvatarToList(GameObject avatar)
        {
            if (avatar.GetComponent<RealtimeView>().isOwnedLocally)
            {
                avatar.name = "local_" + avatar.name;
                localAvatar = avatar;
            }
            else
            {
                avatar.name = "Client" + avatar.GetComponent<RealtimeView>().ownerID + "_" + avatar.name;
                networkAvatars.Add(avatar);
            }
        }

        private void RemoveAvatarFromList(GameObject avatar)
        {
            if (avatar != localAvatar)
                networkAvatars.Remove(avatar);
        }

        private void ParentLocalAvatar()
        {
            if (localTank != null && localAvatar != null)
            {
                TankController tankController = localTank.GetComponent<TankController>();
                localAvatar.transform.parent = localTank.transform;

                localAvatar.transform.localPosition = tankController.avatarSeatPosition;
                localAvatar.transform.localRotation = Quaternion.identity;

                InputTracking.Recenter();

                needToParentLocalAvatar = false;
            }
        }

        private void ParentClientAvatar()
        {
            foreach(GameObject avatar in networkAvatars)
            {
                if (avatar.name.Contains("Client" + clientTankNeedsAvatarID))
                {
                    avatar.transform.parent = clientTankNeedsAvatar.gameObject.transform;
                    avatar.transform.localPosition = clientTankNeedsAvatar.avatarSeatPosition;
                    avatar.transform.localRotation = Quaternion.identity;

                    /// Clean up
                    clientTankNeedsAvatar = null;
                    clientTankNeedsAvatarID = -1;
                }
                
            }
        }

        private void ConnectedToRoom(Realtime realtime)
        {
            clientID = realtime.clientID;
            connectedToRoom = true;
        }

        private void DisconnectedFromRoom(Realtime realtime)
        {
            clientID = realtime.clientID;
            connectedToRoom = true;
        }

        private void ShutdownSequence()
        {
            Application.Quit();
        }
    }
}
