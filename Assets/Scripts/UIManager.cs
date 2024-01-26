using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject ui;
    
    private GameObject menuScreen;
    private Button startButton;
    private Button settingsButton;
    
    private GameObject settingsScreen;
    private Button backButton;
    private Button exitButton;
    private Slider audioSlider;
    
    private GameObject scoreBoard;
    private Slider healthSlider;
    private TextMeshProUGUI healthText;
    
    public event Action OnStartGameButtonPressed; 
    public event Action OnSettingsButtonPressed; 
    public event Action OnBackButtonPressed;
    public event Action OnExitButtonPressed;
    public event Action<float> OnAudioSliderChanged; 
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        
        ui = GameObject.Find("UI");
        
        menuScreen = ui.transform.Find("MenuScreen").gameObject;
        startButton = menuScreen.transform.Find("StartGameBtn").GetComponent<Button>();
        settingsButton = menuScreen.transform.Find("SettingsBtn").GetComponent<Button>();
        
        settingsScreen = ui.transform.Find("SettingsScreen").gameObject;
        backButton = settingsScreen.transform.Find("BackBtn").GetComponent<Button>();
        exitButton = settingsScreen.transform.Find("ExitBtn").GetComponent<Button>();
        audioSlider = settingsScreen.transform.Find("AudioSlider").GetComponent<Slider>();
        
        scoreBoard = ui.transform.Find("ScoreBoard").gameObject;
        healthSlider = scoreBoard.transform.Find("HealthSlider").GetComponent<Slider>();
        healthText = scoreBoard.transform.Find("HealthText").GetComponent<TextMeshProUGUI>();
        
        audioSlider.onValueChanged.AddListener(HandleAudioLevelChange);
        backButton.onClick.AddListener(HandleBackButtonPressed);
        exitButton.onClick.AddListener(HandleExitButtonPressed);
        settingsButton.onClick.AddListener(HandleSettingsButtonPressed);
        startButton.onClick.AddListener(HandleStartGameButtonPressed);
    }

    private void Start()
    {
        settingsScreen.SetActive(false);
        scoreBoard.SetActive(false);
    }
    
    private void HandleStartGameButtonPressed()
    {
        menuScreen.SetActive(false);
        scoreBoard.SetActive(true);
        OnStartGameButtonPressed?.Invoke();
    }
    
    public void HandleSettingsButtonPressed()
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
    
    public void HandleHealthChange(float value)
    {
        healthSlider.value = value;
        healthText.text = $"{value}%";
    }
}
