using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tool helper to quickly align this objects parent transform to another object. Self-destructs on play.
/// </summary>

namespace HiFi
{
    namespace Tools
    {
        [ExecuteAlways]
        public class AlignPointHelper : MonoBehaviour
        {
            [Tooltip("This will instantly align parent object's position and/or rotation (per selection) to 'objectToMatch'")]public bool alignNow = false;
            public GameObject objectToMatch;
            public bool position = true, rotation = true;
            public Vector3 PositionOffset;
            public Vector3 RotationOffset;
            public bool persistentOnPlay = false;

            private void Awake()
            {
                if (Application.IsPlaying(this) && !persistentOnPlay)
                    DestroyImmediate(this.gameObject);
            }

            void Update()
            {
                if (alignNow && objectToMatch != null && !Application.IsPlaying(this))
                {
                    if (position)
                    {
                        transform.parent.position = objectToMatch.transform.position;
                    }
                    if (rotation)
                    {
                        transform.parent.rotation = objectToMatch.transform.rotation;
                    }
                }

                if (objectToMatch != null)
                {
                    PositionOffset = transform.position - objectToMatch.transform.position;
                    Vector3 rotOffset = transform.eulerAngles - objectToMatch.transform.eulerAngles;
                    RotationOffset = new Vector3(rotOffset.x, rotOffset.y, rotOffset.z);
                }

                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                alignNow = false;
            }
        }
    }
}
