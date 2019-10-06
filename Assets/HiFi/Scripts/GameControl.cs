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
        [SerializeField] Realtime realtime;
        [SerializeField] int clientID = -1;
        [SerializeField] bool connectedToRoom;

        [SerializeField] GameObject localPlayer;
        [SerializeField] List<GameObject> otherPlayers = new List<GameObject>();

        public HiFi_PresetButtonInput recenterButton;

        private void OnEnable()
        {
            realtime.didConnectToRoom += ConnectedToRoom;
            realtime.didDisconnectFromRoom += DisconnectedFromRoom; 
        }

        private void OnDisable()
        {
            realtime.didConnectToRoom -= ConnectedToRoom;
            realtime.didDisconnectFromRoom -= DisconnectedFromRoom;
        }

        void Update()
        {
            if (Input.GetKey("escape"))
            {
                ShutdownSequence();
            }
            if(HiFi_Platform.instance.Preset(recenterButton))
            {
                InputTracking.Recenter();
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
