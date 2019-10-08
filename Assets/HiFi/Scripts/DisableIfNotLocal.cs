using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class DisableIfNotLocal : MonoBehaviour
{
    private void Start()
    {
        RealtimeView view = GetComponentInParent<RealtimeView>();
        if (view != null)
        {
            if (!view.isOwnedLocally)
                gameObject.SetActive(false);
        }
    }
}
