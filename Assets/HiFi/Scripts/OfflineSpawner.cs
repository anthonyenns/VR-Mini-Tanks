﻿using UnityEngine;
using Normal.Realtime;

namespace Normal.Realtime.Examples
{
    public class OfflineSpawner : MonoBehaviour
    {
        public GameObject obj;
        public bool randomPosition;
        public Vector2 randomRadiusHeight;
        public bool randomRotation;

        private Vector3 spawnPos;
        private Quaternion spawnRot;

        private void Awake()
        {
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

        private void Start()
        {
            Instantiate(obj, spawnPos, spawnRot);
        }
    }
}