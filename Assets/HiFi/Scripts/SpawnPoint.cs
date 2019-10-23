using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parameters for a spawn point with public method to return Pose.
/// </summary>
namespace HiFi
{
    public class SpawnPoint : MonoBehaviour
    {
        public float variationRadius;
        public float height;
        public bool randomRotation;
        
        private Pose spawnPose;

        public Pose GetPose()
        {
            /// Position
            Vector2 rand = Random.insideUnitCircle * variationRadius;
            spawnPose.position = new Vector3(rand.x + transform.position.x, height, rand.y + transform.position.z);

            /// Rotation
            if (randomRotation)
            {
                spawnPose.rotation = Quaternion.Euler(0, Random.rotation.eulerAngles.y, 0);
            }
            else
                spawnPose.rotation = Quaternion.identity;

            /// Add Terrain Height
            if (Terrain.activeTerrain != null)
                spawnPose.position.y += Terrain.activeTerrain.SampleHeight(spawnPose.position);

            return spawnPose;
        }
    }
}
