using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public bool isKitchen;
    public bool isCoop;
    public Dialogue dialogue;

    public void OnButtonClicked()
    {
        if (isKitchen)
        {
            gameObject.transform.parent.transform.parent.gameObject.SetActive(false);
        }
        else if (isCoop)
        {
            if (dialogue != null)
            {
                dialogue.gameObject.SetActive(false);
                gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        MapManager.Instance.TeleportPlayerToSpawnPoint1();
        MapManager.Instance.ShowMap();
    }
}
