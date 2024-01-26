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
        Debug.Log("Button Clicked!" + this.gameObject.transform.parent.transform.gameObject);
        minigame.SetActive(true);
        minigame.GetComponent<MiniGame>().StartCodeMiniGame();

        this.gameObject.transform.parent.transform.gameObject.SetActive(false);
    }
}
