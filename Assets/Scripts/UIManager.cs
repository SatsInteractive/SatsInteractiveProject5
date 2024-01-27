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

    private Button creditsButton;
    
    private GameObject settingsScreen;
    public Button backButton;
    public Button exitButton;
    public Slider audioSlider;
    
    private GameObject creditsScreen;
    public Button backButtonCredits;

    private GameObject scoreBoard;
    private Slider healthSlider;
    private TextMeshProUGUI healthText;
    
    public event Action OnStartGameButtonPressed; 
    public event Action OnSettingsButtonPressed; 
    public event Action OnBackButtonPressed;
    public event Action OnExitButtonPressed;
    public event Action<float> OnAudioSliderChanged; 

    public event Action OnCreditsButtonPressed;
    public event Action OnBackButtonCreditsPressed;
    
    public TMP_InputField playerNameInput;
    public TMP_InputField strongestSkillInput;
    public Image chosenSprite;
    public Sprite[] possibleSprites;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        
        ui = gameObject;
        
        menuScreen = ui.transform.Find("MenuScreen").gameObject;
        startButton = menuScreen.transform.Find("StartGameBtn").GetComponent<Button>();
        settingsButton = menuScreen.transform.Find("SettingsBtn").GetComponent<Button>();

        creditsButton = menuScreen.transform.Find("CreditsBtn").GetComponent<Button>();
        
        settingsScreen = ui.transform.Find("SettingsScreen").gameObject;
        backButton = settingsScreen.transform.Find("BackBtn").GetComponent<Button>();
        exitButton = settingsScreen.transform.Find("ExitBtn").GetComponent<Button>();
        audioSlider = settingsScreen.transform.Find("AudioSlider").GetComponent<Slider>();

        creditsScreen = ui.transform.Find("CreditsScreen").gameObject;
        backButtonCredits = creditsScreen.transform.Find("BackBtnC").GetComponent<Button>();
        
        scoreBoard = ui.transform.Find("ScoreBoard").gameObject;
        healthSlider = scoreBoard.transform.Find("HealthSlider").GetComponent<Slider>();
        healthText = scoreBoard.transform.Find("HealthText").GetComponent<TextMeshProUGUI>();
        
        audioSlider.onValueChanged.AddListener(HandleAudioLevelChange);
        //backButton.onClick.AddListener(HandleBackButtonPressed);
        //exitButton.onClick.AddListener(HandleExitButtonPressed);
        settingsButton.onClick.AddListener(HandleSettingsButtonPressed);
        creditsButton.onClick.AddListener(HandleCreditsButtonPressed);
        //backButtonCredits.onClick.AddListener(HandleBackButtonCreditsPressed);
        startButton.onClick.AddListener(HandleStartGameButtonPressed);
    }

    private void Start()
    {
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);
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
    
    public void HandleBackButtonPressed()
    {
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        OnBackButtonPressed?.Invoke();
    }
    
    public void HandleExitButtonPressed()
    {
        settingsScreen.SetActive(false);
        OnExitButtonPressed?.Invoke();
    }
    
    private void HandleAudioLevelChange(float value)
    {
        OnAudioSliderChanged?.Invoke(value);
    }

    public void HandleCreditsButtonPressed()
    {
        if (creditsScreen.activeSelf)
        {
            creditsScreen.SetActive(false);
            OnBackButtonCreditsPressed?.Invoke();
        }
        else
        {
            creditsScreen.SetActive(true);
            OnCreditsButtonPressed?.Invoke();
        }
    }

    public void HandleBackButtonCreditsPressed()
    {
        settingsScreen.SetActive(false);
        OnBackButtonCreditsPressed?.Invoke();
    }
    

    public void HandleHealthChange(float value)
    {
        healthSlider.value = value / 100f;
        healthText.text = $"{value}%";
    }
    
    public void HandleCharacterChosen()
    {
        
    }
}
