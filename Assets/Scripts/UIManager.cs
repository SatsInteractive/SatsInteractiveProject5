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
    private AudioSource audioSource;
    public AudioClip buttonClickSound;

    private Button creditsButton;
    
    private GameObject settingsScreen;
    public GameObject mainMenuEndScreen;
    public Button backButton;
    public Button exitButton;
    public Slider audioSlider;
    
    private GameObject creditsScreen;
    public Button backButtonCredits;

    public GameObject scoreBoard;
    private Slider healthSlider;
    public float maxScore = 180f;
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
    public TextMeshProUGUI jammername;
    public TextMeshProUGUI jammerstrongestskill;
    public GameObject jammerCardInfoUpRight;
    public Button tutorialButton;
    public GameObject tutorialScreen;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        
        ui = gameObject;
        audioSource = GetComponent<AudioSource>();
        
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
        
        mainMenuEndScreen.SetActive(false);
        
        audioSlider.onValueChanged.AddListener(HandleAudioLevelChange);
        //backButton.onClick.AddListener(HandleBackButtonPressed);
        //exitButton.onClick.AddListener(HandleExitButtonPressed);
        settingsButton.onClick.AddListener(HandleSettingsButtonPressed);
        creditsButton.onClick.AddListener(HandleCreditsButtonPressed);
        //backButtonCredits.onClick.AddListener(HandleBackButtonCreditsPressed);
        startButton.onClick.AddListener(HandleStartGameButtonPressed);
        tutorialButton.onClick.AddListener(HandleTutorialButtonPressed);
    }

    private void Start()
    {
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        scoreBoard.SetActive(false);
        jammerCardInfoUpRight.SetActive(false);
        HandleAudioLevelChange(0.5f);
        audioSlider.value = 0.5f;
    }
    
    private void HandleStartGameButtonPressed()
    {
        if (jammername.text == "")
        {
            jammername.text = "JammerX";
        }
        if (jammerstrongestskill.text == "")
        {
            jammerstrongestskill.text = "Sleep";
        }
        
        audioSource.PlayOneShot(buttonClickSound);
        jammerCardInfoUpRight.SetActive(true);
        menuScreen.SetActive(false);
        OnStartGameButtonPressed?.Invoke();
        jammerCardInfoUpRight.SetActive(true);
        jammername.text = playerNameInput.text;
        jammerstrongestskill.text = strongestSkillInput.text;
        jammerCardInfoUpRight.SetActive(false);
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
        tutorialScreen.SetActive(false);
        OnBackButtonPressed?.Invoke();
    }
    
    public void HandleExitButtonPressed()
    {
        settingsScreen.SetActive(false);
        OnExitButtonPressed?.Invoke();
    }
    
    private void HandleAudioLevelChange(float value)
    {
        audioSource.volume = value * 0.25f;
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
        healthSlider.value = value / maxScore;
        healthText.text = $"{value.ToString("F1")} points";
    }
    
    public void HandleTutorialButtonPressed()
    {
        tutorialScreen.SetActive(true);
    }
    
    public void HandleNextCharacterButton()
    {
        int currentIndex = Array.IndexOf(possibleSprites, chosenSprite.sprite);
        if (currentIndex == possibleSprites.Length - 1)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }

        chosenSprite.sprite = possibleSprites[currentIndex];
    }
}
