using System;
using System.Collections.Generic;
using UnityEngine;
using HiFi.Platform;
using Normal.Realtime;

/// <summary>
/// Base Class for projectiles!
/// </summary>
namespace HiFi
{
    public class Projectile : MonoBehaviour
    {
        public ParticleSystem ps;
        public Renderer rend;

        public static Action<GameObject, GameObject, int> ProjectileHit;

        [Header("Light Setting")]
        public Light flash;
        public float flashDuration = 0.1f;
        public float intensityOnFire = 2;
        public float rangeOnFire = 3;
        public float intensityOnTravel = 0.5f;
        public float intensityOnHit = 4;
        public float rangeOnHit = 6;

        protected bool canDestroy;
        protected float startTime;
        protected RealtimeView view;
        protected int projectileID = -1;

        protected virtual void Start()
        {
            /// Online or offline setup
            if (GameControl.instance.realtimeEnabled)
            {
                view = GetComponent<RealtimeView>();
            }
            else
            {
                Destroy(GetComponent<RealtimeTransform>());
                Destroy(GetComponent<RealtimeView>());
                projectileID = 0;
            }

            flash.intensity = intensityOnFire;
            startTime = Time.time;
        }

        protected virtual void Update()
        {
            /// Set light intensity
            if (Time.time > startTime + flashDuration)
                flash.intensity = intensityOnTravel;

            /// Wait for effects to end before destroying
            if (ps.particleCount <= 1 && canDestroy)
            {
                if (view != null) /// Network Destroy
                {
                    Realtime.Destroy(gameObject);
                }
                else
                    Destroy(gameObject); /// Offline Destroy
            }
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            /// Hide & stop projectile geometry
            rend.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;

            /// Reset light flash intensity
            flash.intensity = intensityOnHit;
            flash.range = rangeOnHit;
            startTime = Time.time;

            /// Get projectile ID if networked
            if (view != null)
                projectileID = view.ownerID;

            /// Invoke Event
            ProjectileHit(gameObject, collision.gameObject, projectileID);            

            /// Trigger Explosion

            /// Will destroy gameObject when all effects have ended
            canDestroy = true;
        }
    }
}
