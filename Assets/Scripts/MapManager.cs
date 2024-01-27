using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    private GameObject mapArt;
    [SerializeField] private Transform playerSpawnPoint;
    private PlayerController playerController;
    [SerializeField] private GameObject codeOrArtScreen;

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
        playerController = FindObjectOfType<PlayerController>();
    }

    public void HideMap()
    {
        HideAllChildSprites(mapArt);
        playerController.FaceSpriteRenderer.enabled = false;
    }
    
    private void HideAllChildSprites(GameObject parent)
    {
        SpriteRenderer[] childSprites = parent.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sprite in childSprites)
        {
            sprite.enabled = false;
        }
    }

    public void ShowMap()
    {
        ShowAllChildSprites(mapArt);
        playerController.FaceSpriteRenderer.enabled = true;
        codeOrArtScreen.SetActive(false);
    }
    
    private void ShowAllChildSprites(GameObject parent)
    {
        SpriteRenderer[] childSprites = parent.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sprite in childSprites)
        {
            sprite.enabled = true;
        }
    }
    
    public void TeleportPlayerToSpawnPoint1()
    {
        playerController.transform.position = playerSpawnPoint.position;
        playerController.isInMiniGame = false;
    }
}
