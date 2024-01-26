using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    public GameObject minigame;

    public void InstantiateMinigame()
    {
        Debug.Log("Button Clicked!");
        Instantiate(minigame);

        Debug.Log(this.gameObject.transform.parent.transform.gameObject);
        Destroy(this.gameObject.transform.parent.transform.gameObject);
    }
}
