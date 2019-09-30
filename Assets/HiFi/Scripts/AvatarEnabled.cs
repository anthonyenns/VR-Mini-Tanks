using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiFi
    {
    public class AvatarEnabled : MonoBehaviour
    {
        public Transform leftHandAnchor;
        public Transform rightHandAnchor;

        public static Action<GameObject, Transform, Transform> OnAvatarEnabled = (tank, left, right) => { };

        void Start()
        {
            Debug.Log("Firing AvatarEnabled");
            OnAvatarEnabled(this.gameObject, leftHandAnchor, rightHandAnchor);
        }
    }
}
