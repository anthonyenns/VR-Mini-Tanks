using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiFi
{
    namespace Platform
    {
        [System.Serializable]
        public class HiFi_PresetButtonInput
        {
            public HiFi_Platform.Method method;
            public HiFi_Platform.Hand hand;
            public HiFi_Platform.Button button;
        }
    }
}