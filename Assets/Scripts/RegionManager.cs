using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour
{
    public GameObject regionUI; // Prefab for the UI to be displayed in the region

    private bool playerInsideRegion = false;

    private void Start()
    {
        regionUI.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collision with: " + other.gameObject.name);
            playerInsideRegion = true;

            regionUI.SetActive(true);

            // Disable player movement and map
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.isInMiniGame = true;
            MapManager.Instance.HideMap();

        }
    }

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Debug.Log("Exit Collision with: " + other.gameObject.name);

    //        // Call the ExitRegion method when the player exits the region
    //        ExitRegion();

    //        playerInsideRegion = false;
    //        //MapManager.Instance.ShowMap();
    //    }
    //}

    // Call this method when the player completes the activity and leaves the region
    public void ExitRegion()
    {
            Debug.Log("ExitRegion gets called");

            regionUI.SetActive(false);

            // MapController.Instance.EnableMap();
    }
}
