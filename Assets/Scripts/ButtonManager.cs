using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    public GameObject minigame;

    void Start()
    {
        Debug.Log("Button created!");
    }

    public void StartMinigame()
    {
        Debug.Log(minigame.gameObject + " is active!");
        minigame.gameObject.SetActive(true);
        minigame.GetComponent<MiniGame>().StartMiniGame();
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
