using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class DisableIfNotLocal : MonoBehaviour
{
    private void Start()
    {
        if (!GetComponentInParent<RealtimeView>().isOwnedLocally)
            gameObject.SetActive(false);
    }
}
