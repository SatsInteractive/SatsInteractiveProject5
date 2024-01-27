using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class Cutscenes : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    private GameObject loadingScreen;
    public UIManager uiManager;
    // Start is called before the first frame update

    private void Start()
    {
        uiManager.jammerCardInfoUpRight.SetActive(false);
        StartCoroutine(LoadingScreen());
    }
    private IEnumerator LoadingScreen()
    {
        yield return new WaitForSeconds(1f);
        videoPlayer.Play();
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds((float)videoPlayer.clip.length - 0.1f);
        uiManager.jammerCardInfoUpRight.SetActive(true);
        gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
