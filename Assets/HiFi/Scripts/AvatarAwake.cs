using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiFi
    {
    public class AvatarAwake : MonoBehaviour
    {
        public Transform leftHandAnchor;
        public Transform rightHandAnchor;

        public static Action<Transform, Transform> avatarAwake;

        void Start()
        {
            avatarAwake(leftHandAnchor, rightHandAnchor);
        }
    }
}
