using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private UIManager uiManager;
    private PointsManager pointsManager;
    private InputManager inputManager;
    private SoundManager soundManager;
    
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
    }

    private void OnEnable()
    {
        uiManager.OnAudioSliderChanged += AdjustAudioLevel;
        uiManager.OnExitButtonPressed += ExitGame;
        uiManager.OnSettingsButtonPressed += StopGame;
        uiManager.OnBackButtonPressed += ResumeGame;
        uiManager.OnStartGameButtonPressed += StartGame;
        pointsManager.OnPointsUpdated += UpdatePoints;
    }
    
    private void OnDisable()
    {
        uiManager.OnAudioSliderChanged -= AdjustAudioLevel;
        uiManager.OnExitButtonPressed -= ExitGame;
        uiManager.OnSettingsButtonPressed -= StopGame;
        uiManager.OnBackButtonPressed -= ResumeGame;
    }

    private void Update()
    {
        inputManager.HandleUIInput();
        inputManager.HandlePointsInput();
    }

    private void StartGame()
    {
        Debug.Log("Starting game...");
        pointsManager.SetPoints(0);
        ChangeScene("Main");
    }
    
    private void StopGame()
    {
        Debug.Log("Stopping game...");
    }
    
    private void ResumeGame()
    {
        Debug.Log("Resuming game...");
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
