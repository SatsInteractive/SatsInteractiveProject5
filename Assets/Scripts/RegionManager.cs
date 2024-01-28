using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour
{
    public GameObject regionUI; // Prefab for the UI to be displayed in the region
    

    private void Start()
    {
        regionUI.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collision with: " + other.gameObject.name);

            regionUI.SetActive(true);

            // Disable player movement and map
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.isInMiniGame = true;
            MapManager.Instance.HideMap();

        }
    }
}
