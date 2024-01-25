using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UIManager uiManager;
    
    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null) gameObject.AddComponent<UIManager>();
        uiManager.OnAudioSliderChanged += AdjustAudioLevel;
        uiManager.OnExitButtonPressed += ExitGame;
        uiManager.OnSettingsButtonPressed += StopGame;
        uiManager.OnBackButtonPressed += ResumeGame;
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
        Debug.Log("Exiting game...");
    }
    
    private void AdjustAudioLevel(float value)
    {
        Debug.Log($"Audio level changed to {value}");
    }
    
    private void OnDestroy()
    {
        uiManager.OnAudioSliderChanged -= AdjustAudioLevel;
        uiManager.OnExitButtonPressed -= ExitGame;
        uiManager.OnSettingsButtonPressed -= StopGame;
        uiManager.OnBackButtonPressed -= ResumeGame;
    }
    
}
