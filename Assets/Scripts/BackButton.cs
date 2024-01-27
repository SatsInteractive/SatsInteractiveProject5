using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public bool isKitchen;

    public void OnButtonClicked()
    {
        if (isKitchen)
        {
            gameObject.transform.parent.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        MapManager.Instance.TeleportPlayerToSpawnPoint1();
        MapManager.Instance.ShowMap();
    }
}
