using UnityEngine;
using Normal.Realtime;

/// <summary>
/// Instantiates Realtime Networked Player Tank
/// </summary>
namespace HiFi
{
    public class RealtimeTankSpawner : MonoBehaviour
    {
        public string resourceName;
        public SpawnPoint[] spawnPoints;

        private Pose spawnPose;
        private Realtime realtime;
        private Quaternion spawnRot;
        private GameObject tank;
        private RealtimeView view;

        private void Awake()
        {
            /// Get the Realtime component on this game object
            realtime = GetComponent<Realtime>();

            /// Notify us when Realtime successfully connects to the room
            realtime.didConnectToRoom += DidConnectToRoom;

            /// Choose spawn point
            if (spawnPoints.Length > 0)
            {
                int point = Random.Range(0, spawnPoints.Length - 1);
                spawnPose = spawnPoints[point].GetPose();
            }
        }

        private void DidConnectToRoom(Realtime realtime)
        {
            if (resourceName != null && resourceName != string.Empty)
                /// Instantiate the CubePlayer for this client once we've successfully connected to the room
               tank = Realtime.Instantiate(resourceName,       // Prefab name
                                position: spawnPose.position,  // Position        
                                rotation: spawnPose.rotation,  // Rotation
                           ownedByClient: true,                // Make sure the RealtimeView on this prefab is owned by this client
                preventOwnershipTakeover: true,                // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                             useInstance: realtime);           // Use the instance of Realtime that fired the didConnectToRoom event.

            /// Set up tank ref in GameControl
            view = tank.GetComponent<RealtimeView>();
            view.RequestOwnership();
            HiFi_Utilities.DebugText("Local Realtime Triggered Tank Spawn ID " + view.ownerID);
            GameControl.instance.AddLocalTank(tank, view.ownerID);
        }
    }
}
