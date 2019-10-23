using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HiFi.Platform;
using Normal.Realtime;

namespace HiFi
{
    public class TankProjectileSpawner : MonoBehaviour
    {
        public TankController controller;
        public TankMover tankMover;
        public HiFi_PresetButtonInput fire;
        public string loadedAmmo;
        public GameObject gunBarrel;
        public GameObject firePoint;
        public float force = 10;
        public float kickBackStrength;

        private Rigidbody rb;
        private GameObject projectile;
        private bool ownedLocally;

        void Update()
        {
            /// Do we own this tank? ================================================
            if (controller.realtimeView != null)
            {              
                ownedLocally = controller.realtimeView.isOwnedLocally;
                if (!ownedLocally)
                    return;
            }
            /// =====================================================================
            
            if (HiFi_Platform.instance.Preset(fire) || Input.GetKeyDown(KeyCode.M))
            {
                /// Networked spawn
                if (GameControl.instance.realtimeEnabled)
                {
                    if (loadedAmmo != null && loadedAmmo != string.Empty)
                        /// Instantiate the CubePlayer for this client once we've successfully connected to the room
                 projectile = Realtime.Instantiate(loadedAmmo,           // Prefab name
                                         position: firePoint.transform.position,  // Position        
                                         rotation: Quaternion.identity,  // Rotation
                                    ownedByClient: true,                // Make sure the RealtimeView on this prefab is owned by this client
                         preventOwnershipTakeover: true,                // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                                      useInstance: GameControl.instance.RealtimeInstance);           // Use the instance of Realtime that fired the didConnectToRoom event.
                }
                /// Offline spawn
                else
                {
                    projectile = Instantiate(Resources.Load(loadedAmmo, typeof(GameObject)) as GameObject, firePoint.transform.position, Quaternion.identity);
                }

                GameControl.instance.myActiveProjectiles.Add(projectile);
                rb = projectile.GetComponent<Rigidbody>();
                rb.AddForce(-gunBarrel.transform.right * force, ForceMode.Impulse);
                tankMover.Kickback(kickBackStrength);
            }
        }   
    }
}