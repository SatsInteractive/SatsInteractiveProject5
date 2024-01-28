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
    public TextMeshProUGUI timeText;
    public Dialogue Dialogue;
    public GameObject TiksuUIGameObject;
    public PlayerController PlayerController;

    private void Start()
    {
        PlayerController.isInMiniGame = true;
        uiManager.jammerCardInfoUpRight.SetActive(false);
        timeText.gameObject.SetActive(false);
        StartCoroutine(LoadingScreen());
    }
    private IEnumerator LoadingScreen()
    {
        yield return new WaitForSeconds(1f);
        videoPlayer.Play();
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds((float)videoPlayer.clip.length - 0.1f);
        DisableCutScene();
    }

    private void TiksuOpening()
    {
        Dialogue.transform.parent.gameObject.SetActive(true);
        TiksuUIGameObject.SetActive(true);
        Dialogue.StartDialogue(Dialogue.dialoguePlaceOptions.Tiksu);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DisableCutScene();
        }
    }

    private void DisableCutScene()
    {
        uiManager.jammerCardInfoUpRight.SetActive(true);
        uiManager.scoreBoard.SetActive(true);
        timeText.gameObject.SetActive(true);
        gameObject.SetActive(false);
        TiksuOpening();
    }
}
