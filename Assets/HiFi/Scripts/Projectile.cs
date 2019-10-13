using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    ParticleSystem ps;
    Renderer rend;

    private bool canDestroy;

    private void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        rend = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        rend.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        /// Trigger Explosion
        /// 

        canDestroy = true;
    }

    private void Update()
    {
        if (ps.particleCount <= 1 && canDestroy)
            Destroy(gameObject);
    }
}
