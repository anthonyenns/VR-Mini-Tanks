using UnityEngine;
using Normal.Realtime;

namespace HiFi
{
    public class RealtimeSpawner : MonoBehaviour
    {

        public string resourceName;
        public bool randomPosition;
        public Vector2 randomRadiusHeight;
        public bool randomRotation;

        private Realtime _realtime;
        private Vector3 spawnPos;
        private Quaternion spawnRot;

        private void Awake()
        {
            // Get the Realtime component on this game object
            _realtime = GetComponent<Realtime>();

            // Notify us when Realtime successfully connects to the room
            _realtime.didConnectToRoom += DidConnectToRoom;

            /// Position
            if (randomPosition)
            {
                Vector2 rand = Random.insideUnitCircle * randomRadiusHeight.x;
                spawnPos = new Vector3(rand.x, randomRadiusHeight.y, rand.y);
            }
            else
                spawnPos = Vector3.up;

            /// Rotation
            if (randomRotation)
            {
                spawnRot = Quaternion.Euler(0, Random.rotation.eulerAngles.y, 0);
            }
            else
                spawnRot = Quaternion.identity;
        }

        private void DidConnectToRoom(Realtime realtime)
        {
            if (resourceName != null && resourceName != string.Empty)
                // Instantiate the CubePlayer for this client once we've successfully connected to the room
                Realtime.Instantiate(resourceName,                 // Prefab name
                                position: spawnPos,          
                                rotation: spawnRot, 
                           ownedByClient: true,                // Make sure the RealtimeView on this prefab is owned by this client
                preventOwnershipTakeover: true,                // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                             useInstance: realtime);           // Use the instance of Realtime that fired the didConnectToRoom event.

        }
    }
}
