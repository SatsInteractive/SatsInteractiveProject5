using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private UIManager uiManager;
    private PointsManager pointsManager;
    private InputManager inputManager;
    private SoundManager soundManager;
    private CodeMiniGame codeMiniGame;
    private ColorMatchingMinigame colorMatchingMinigame;
    private PlayerController playerController;
    private Eating eating;
    private GameObject playerNameGO;
    private Cutscenes cutscenes;
    private PunktideJaTundideHaldaja punktideJaTundideHaldaja;
    
    private bool isGamePaused = false;
    private bool inMiniGame = false;
    
    public string playerName = "";
    public string strongestSkill = "None";
    public Sprite chosenSprite;
    
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null) gameObject.AddComponent<UIManager>();
        
        pointsManager = FindObjectOfType<PointsManager>();
        if (pointsManager == null) gameObject.AddComponent<PointsManager>();
        
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null) gameObject.AddComponent<SoundManager>();
        
        inputManager = FindObjectOfType<InputManager>();
        if (inputManager == null) gameObject.AddComponent<InputManager>();
        
        uiManager.maxScore = pointsManager.maxPoints;
    }

    private void OnEnable()
    {
        uiManager.OnAudioSliderChanged += AdjustAudioLevel;
        uiManager.OnExitButtonPressed += ExitGame;
        uiManager.OnSettingsButtonPressed += StopGame;
        uiManager.OnBackButtonPressed += ResumeGame;
        uiManager.OnStartGameButtonPressed += StartGame;
        pointsManager.OnPointsUpdated += UpdatePoints;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        uiManager.OnAudioSliderChanged -= AdjustAudioLevel;
        uiManager.OnExitButtonPressed -= ExitGame;
        uiManager.OnSettingsButtonPressed -= StopGame;
        uiManager.OnBackButtonPressed -= ResumeGame;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        inputManager.HandleUIInput();
        //inputManager.HandlePointsInput();
        if (isGamePaused) return;
        inputManager.HandleMovementInput();
        
        //if (codeMiniGame == null) return;
        //codeMiniGame.HandleCodeMiniGameInput();
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        if (scene.name == "Main")
        {
            codeMiniGame = FindObjectOfType<CodeMiniGame>();
            colorMatchingMinigame = FindObjectOfType<ColorMatchingMinigame>();
            playerController = FindObjectOfType<PlayerController>();
            punktideJaTundideHaldaja = FindObjectOfType<PunktideJaTundideHaldaja>();
            punktideJaTundideHaldaja.pointsManager = pointsManager;
            eating = FindObjectOfType<Eating>();
            cutscenes = FindObjectOfType<Cutscenes>();
            cutscenes.uiManager = uiManager;
            inputManager.playerController = playerController;
            playerController.FaceSpriteRenderer.sprite = chosenSprite;
            playerNameGO = GameObject.FindWithTag("PlayerName");
            if (playerName.Length == 0)
            {
                playerNameGO.GetComponent<TMP_Text>().text = "You";
            }
            else
            {
                playerNameGO.GetComponent<TMP_Text>().text = playerName;
            }
            AdjustAudioLevel(uiManager.audioSlider.value);
        }
    }

    private void StartGame()
    {
        Debug.Log("Starting game...");
        pointsManager.SetPoints(0);
        playerName = uiManager.playerNameInput.text;
        strongestSkill = uiManager.strongestSkillInput.text;
        chosenSprite = uiManager.chosenSprite.sprite;
        ChangeScene("Main");
    }
    
    private void StopGame()
    {
        Debug.Log("Stopping game...");
        isGamePaused = true;
    }
    
    private void ResumeGame()
    {
        Debug.Log("Resuming game...");
        isGamePaused = false;
    }
    
    private void ExitGame()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("Exiting game called, scene: " + currentScene + "...");

        if (currentScene == "Menu")
        {
            Debug.Log("Exiting application...");
            Application.Quit();
        }
        else if (currentScene == "Main")
        {
            Debug.Log("Exiting game...");
            pointsManager.SetPoints(0);
            SceneManager.LoadScene("Menu");
        }
    }
    
    private void AdjustAudioLevel(float value)
    {
        Debug.Log($"Audio level changed to {value}");
        soundManager.AdjustAudioLevel(value);
        if (codeMiniGame != null) codeMiniGame.CodeMiniGameChangeVolumeLevel(value);
        if (playerController != null) playerController.ChangeVolumeLevel(value);
        if (eating != null) eating.ChangeVolume(value);
        if (colorMatchingMinigame != null) colorMatchingMinigame.ColorMatchingMinigameChangeVolumeLevel(value);
    }
    
    private void ChangeScene(string sceneName)
    {
        // Load scene
        SceneManager.LoadScene(sceneName);
    }
    
    private void UpdatePoints(float points)
    {
        Debug.Log($"Points set to: {points}");
        uiManager.HandleHealthChange(points);
    }
    
}
