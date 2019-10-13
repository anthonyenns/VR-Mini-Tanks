using UnityEngine;
using UnityEngine.XR;

namespace HiFi
{
    public class OfflineSpawner : MonoBehaviour
    {
        public GameObject obj;
        public bool randomPosition;
        public Vector2 randomRadiusHeight;
        public bool randomRotation;
        public bool vROnly;

        private Vector3 spawnPos;
        private Quaternion spawnRot;

        private void Awake()
        {
            if (vROnly && !XRSettings.enabled)
                obj = null;

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

            /// Add Terrain Height
            spawnPos.y += Terrain.activeTerrain.SampleHeight(spawnPos);
        }

        private void Start()
        {
            if (obj != null)
            {
                Instantiate(obj, spawnPos, spawnRot);
            }
        }
    }
}
