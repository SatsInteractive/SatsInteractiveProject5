using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject ui;
    private GameObject settingsScreen;
    private Button backButton;
    private Button exitButton;
    private Slider audioSlider;
    
    public event Action OnSettingsButtonPressed; 
    public event Action OnBackButtonPressed;
    public event Action OnExitButtonPressed;
    public event Action<float> OnAudioSliderChanged; 
    
    private void Awake()
    {
        ui = GameObject.Find("UI");
        settingsScreen = ui.transform.Find("SettingsScreen").gameObject;
        backButton = settingsScreen.transform.Find("BackBtn").GetComponent<Button>();
        exitButton = settingsScreen.transform.Find("ExitBtn").GetComponent<Button>();
        audioSlider = settingsScreen.transform.Find("AudioSlider").GetComponent<Slider>();
        
        audioSlider.onValueChanged.AddListener(HandleAudioLevelChange);
        backButton.onClick.AddListener(HandleBackButtonPressed);
        exitButton.onClick.AddListener(HandleExitButtonPressed);
    }

    private void Start()
    {
        settingsScreen.SetActive(false);
    }


    void Update()
    {
        CheckForUIInput();
    }
    
    private void CheckForUIInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleSettingsButtonPressed();
        }
    }
    
    private void HandleSettingsButtonPressed()
    {
        if (settingsScreen.activeSelf)
        {
            settingsScreen.SetActive(false);
            OnBackButtonPressed?.Invoke();
        }
        else
        {
            settingsScreen.SetActive(true);
            OnSettingsButtonPressed?.Invoke();
        }
    }
    
    private void HandleBackButtonPressed()
    {
        settingsScreen.SetActive(false);
        OnBackButtonPressed?.Invoke();
    }
    
    private void HandleExitButtonPressed()
    {
        settingsScreen.SetActive(false);
        OnExitButtonPressed?.Invoke();
    }
    
    private void HandleAudioLevelChange(float value)
    {
        OnAudioSliderChanged?.Invoke(value);
    }
}
