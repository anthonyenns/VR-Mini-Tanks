using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

namespace HiFi
{
    public class TankPlayerManager : MonoBehaviour
    {
        private Realtime _realtime;
        private GameObject tank;

        private void Awake()
        {
            // Get the Realtime component on this game object
            _realtime = GetComponent<Realtime>();

            // Notify us when Realtime successfully connects to the room
            if (_realtime != null)
                _realtime.didConnectToRoom += DidConnectToRoom;
        }

        private void DidConnectToRoom(Realtime realtime)
        {
            // Instantiate the TankPlayer for this client once we've successfully connected to the room
            tank = Realtime.Instantiate("TankPlayer",          // Prefab name
                                position: Vector3.zero,        // Start position
                                rotation: Quaternion.identity, // No rotation
                           ownedByClient: true,                // Make sure the RealtimeView on this prefab is owned by this client
                preventOwnershipTakeover: true,                // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                             useInstance: realtime);           // Use the instance of Realtime that fired the didConnectToRoom event.

            tank.name = "Tank_" + Mathf.RoundToInt(Random.Range(0, 100));
        }
    }
}
