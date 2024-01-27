using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Cutscenes : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    private GameObject loadingScreen;
    // Start is called before the first frame update

    private void Awake()
    {

    }

    void Start()
    {
        StartCoroutine(LoadingScreen());
    }
    private IEnumerator LoadingScreen()
    {
        yield return new WaitForSeconds(1f);
        videoPlayer.Play();
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
