using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using HiFi.Interactions;

namespace HiFi
{
    namespace Tools
    {
        [ExecuteAlways]
        public class AlignAttachEditRun : MonoBehaviour
        { /*
            public bool runInEdit;
            public HiFi_AttachPointConfig alignAndAttacher;
            public string loadedConfig;
            public Transform attacher;
            public Transform handle;

            private void OnEnable()
            {
                runInEdit = false;
            }

            private void Awake()
            {
                runInEdit = false;
                if (Application.IsPlaying(this))
                    DestroyImmediate(this);
            }

            private void Update()
            {
                if (alignAndAttacher != null)
                {
                    loadedConfig = alignAndAttacher.configName;
                    if (!Application.IsPlaying(this) && runInEdit)
                    {
                        if (attacher != null && handle != null)
                            alignAndAttacher.CreateTrackingPoint(attacher, handle);
                        else
                        {
                            Debug.LogError(gameObject.name + ": " + alignAndAttacher.name + " needs both attacher and handle transforms to run in Edit Mode");
                            runInEdit = false;
                        }
                    }
                }
                else
                    loadedConfig = System.String.Empty;
            }*/
        }
    }
}
