using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

namespace HiFi
{
    public class GameControl : MonoBehaviour
    {
        public Realtime realtime;
        public int clientID = -1;
        public bool connectedToRoom;

        [SerializeField] GameObject playerTank;
        [SerializeField] GameObject playerAvatar;
        [SerializeField] bool playerIsBuilt;
        [SerializeField] List<GameObject> otherPlayers = new List<GameObject>();

        private void OnEnable()
        {
            TankEnabled.OnTankEnabled += SetTankRef;
            AvatarEnabled.OnAvatarEnabled += SetAvatarRef;
            realtime.didConnectToRoom += DidConnectToRoom;
        }

        private void OnDisable()
        {
            TankEnabled.OnTankEnabled -= SetTankRef;
            AvatarEnabled.OnAvatarEnabled -= SetAvatarRef;
        }

        void Update()
        {
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        }

        private void DidConnectToRoom(Realtime realtime)
        {
            clientID = realtime.clientID;
            connectedToRoom = true;
        }

        private void SetTankRef(GameObject tank)
        {
            Debug.Log("Received ref to " + tank.name);
            if (realtime.clientID == tank.GetComponent<RealtimeView>().ownerID)
            {
                playerTank = tank;
                SetAvatarParent();
            }
            else
            {
                otherPlayers.Add(tank);
            }
        }

        private void SetAvatarRef(GameObject avatar, Transform left, Transform right)
        {
            Debug.Log("Received ref to " + avatar.name);
            if (realtime.clientID == avatar.GetComponent<RealtimeView>().ownerID)
            {
                playerAvatar = avatar;
                SetAvatarParent();
            }
        }

        private void SetAvatarParent()
        {
            if (playerAvatar != null && playerTank != null && !playerIsBuilt)
            {
                playerAvatar.transform.parent = playerTank.transform;
                playerIsBuilt = true;
            }

            if (playerIsBuilt)
            {
                TankEnabled.OnTankEnabled -= SetTankRef;
                AvatarEnabled.OnAvatarEnabled -= SetAvatarRef;
            }
        }
    }
}
