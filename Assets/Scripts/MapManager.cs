using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    GameObject mapArt;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        mapArt = gameObject.transform.GetChild(0).gameObject;
    }

    public void HideMap()
    {
        mapArt.SetActive(false);
    }

    public void ShowMap()
    {
        mapArt.SetActive(true);
    }
}
