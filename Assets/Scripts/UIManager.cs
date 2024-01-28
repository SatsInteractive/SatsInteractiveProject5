using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    private GameObject ui;
    
    public GameObject menuScreen;
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
    public GameObject jammerCard;
    public float scaleAnimationDuration = 1f;
    public float targetScaleFactor = 1.2f;
    
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
    public Button tutorialScreenNextButton;
    public Sprite tutorialScreenSprite1;
    public Sprite tutorialScreenSprite2;
    
    public bool credentialsEntered = false;
    
    protected override void Awake()
    {
        base.Awake();
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
        tutorialScreenNextButton.onClick.AddListener(HandleTutorialScreenNextButtonPressed);
    }

    private void Start()
    {
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        scoreBoard.SetActive(false);
        jammerCardInfoUpRight.SetActive(false);
        HandleAudioLevelChange(0.5f);
        audioSlider.value = 1f;
    }
    
    private void HandleStartGameButtonPressed()
    {
        if (string.IsNullOrEmpty(playerNameInput.text) || string.IsNullOrEmpty(strongestSkillInput.text))
        {
            StartCoroutine(ScaleJammerCard());
            return;
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
    
    private IEnumerator ScaleJammerCard()
    {
        float elapsed = 0f; 
        Vector3 initialScale = jammerCard.transform.localScale; // Initial scale of the GameObject
        Vector3 targetScale = initialScale * targetScaleFactor; // Target scale (20% larger than the initial scale)

        // Scale up
        while (elapsed < scaleAnimationDuration / 2)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (scaleAnimationDuration / 2);
            jammerCard.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }

        elapsed = 0f; // Reset the elapsed time

        // Scale down
        while (elapsed < scaleAnimationDuration / 2)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (scaleAnimationDuration / 2);
            jammerCard.transform.localScale = Vector3.Lerp(targetScale, initialScale, t);
            yield return null;
        }

        // Ensure the scale is reset to the initial scale
        jammerCard.transform.localScale = initialScale;
    }
    
    public void HandleSettingsButtonPressed()
    {
        audioSource.PlayOneShot(buttonClickSound);
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
        audioSource.PlayOneShot(buttonClickSound);
        OnBackButtonPressed?.Invoke();
    }
    
    public void HandleExitButtonPressed()
    {
        settingsScreen.SetActive(false);
        audioSource.PlayOneShot(buttonClickSound);
        OnExitButtonPressed?.Invoke();
    }
    
    private void HandleAudioLevelChange(float value)
    {
        audioSource.volume = value * 0.5f;
        OnAudioSliderChanged?.Invoke(value);
    }

    public void HandleCreditsButtonPressed()
    {
        audioSource.PlayOneShot(buttonClickSound);
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
        audioSource.PlayOneShot(buttonClickSound);
        tutorialScreen.SetActive(true);
    }
    
    public void HandleTutorialScreenNextButtonPressed()
    {
        audioSource.PlayOneShot(buttonClickSound);
        if (tutorialScreen.GetComponent<Image>().sprite == tutorialScreenSprite1)
        {
            tutorialScreen.GetComponent<Image>().sprite = tutorialScreenSprite2;
        }
        else
        {
            tutorialScreen.GetComponent<Image>().sprite = tutorialScreenSprite1;
        }
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
