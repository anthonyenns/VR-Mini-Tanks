using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiFi
{
    namespace Tools
    {
        [ExecuteAlways]
        public class ResetOnPlay : MonoBehaviour
        {
            [Header("***STORE TRANSFORM DATA***")]
            public bool storeTransform;
            [Header("***RESET TRANSFORM NOW***")]
            public bool resetNow;
            [Header("Stored Settings")]
            public Vector3 localPosition;
            public Vector3 scale = Vector3.one;
            public Transform originalParent;
            public Quaternion localRotation;

            private void Awake()
            {
                if (Application.IsPlaying(this))
                {
                    transform.parent = originalParent;
                    transform.localPosition = localPosition;
                    transform.localRotation = localRotation;
                    transform.localScale = scale;
                    Destroy(this);
                }
            }

            private void Update()
            {
                if (!Application.IsPlaying(this) && storeTransform)
                {
                    originalParent = transform.parent;
                    localPosition = transform.localPosition;
                    localRotation = transform.localRotation;
                    scale = transform.localScale;
                }

                if (!Application.IsPlaying(this) && resetNow)
                {
                    transform.parent = originalParent;
                    transform.localPosition = localPosition;
                    transform.localRotation = localRotation;
                    transform.localScale = scale;
                }

                storeTransform = false;
                resetNow = false;
            }
        }
    }
}

