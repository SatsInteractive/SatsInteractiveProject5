using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DisableMap()
    {
        // Implement logic to disable the map
    }

    public void EnableMap()
    {
        // Implement logic to enable the map
    }
}
