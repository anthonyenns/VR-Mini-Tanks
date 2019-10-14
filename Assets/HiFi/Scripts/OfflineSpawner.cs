using UnityEngine;
using UnityEngine.XR;

namespace HiFi
{
    public class OfflineSpawner : MonoBehaviour
    {
        public GameObject resourcesPrefab;
        public bool vROnly;
        public SpawnPoint[] spawnPoints;

        private Pose spawnPose;
        private Quaternion spawnRot;

        private void Awake()
        {
            /// Disable if VR only
            if (vROnly && !XRSettings.enabled)
                resourcesPrefab = null;

            /// Choose spawn point
            if (spawnPoints.Length > 0)
            {
                int point = Random.Range(0, spawnPoints.Length - 1);
                spawnPose = spawnPoints[point].GetPose();
            }
        }

        private void Start()
        {
            if (resourcesPrefab != null)
            {
                Instantiate(resourcesPrefab, spawnPose.position, spawnPose.rotation);
            }
        }
    }
}
