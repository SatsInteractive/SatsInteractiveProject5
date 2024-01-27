using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CodeMiniGame : MiniGame
{
    [SerializeField] private TextMeshProUGUI promptText;
    private AudioSource codeMiniGameAudioSource;
    [SerializeField] private GameObject codeStartingScreen;
    
    private float timeTaken;
    private bool codeMiniGameActive = false;
    private int promptCount = 0;
    private bool promptSet = false;
    private bool userPressedFirstKey = false;

    [Header("Settings")]
    [SerializeField] private List<string> codingPrompts = new List<string>()
    {
        "Debug.Log(\"I'm trapped in a computer!\");",
        "transform.Rotate(Vector3.up * danceSpeed * Time.deltaTime);",
        "if(Input.GetButtonDown(\"PartyMode\")) StartParty();",
        "public int numberOfCats = 999;",
        "Rigidbody2D gravityIsALie;",
        "bool isPlayerAsleep = false;",
        "Animator discoDancer = GetComponent<Animator>();",
        "float coffeeLevel = 0.0f;",
        "void WhyAmIRunning() { }"
    };
    [SerializeField] public Color normalColor = new Color(85, 85, 85, 1);
    [SerializeField] public Color errorColor = new Color(106, 106, 68, 1);
    public float newPromptDelay = 1f;
    public int promptsPerGame = 3;
    
    [Header("UI References")]
    [SerializeField] private TMP_Text promptDisplay;
    [SerializeField] private string currentPrompt;
    private int currentCharacterIndex = 0;
    
    [Header("Stats")]
    private int totalMistakes = 0;
    private int totalCharactersTyped = 0;
    [SerializeField] private TMP_Text averagePromptSpeedText;
    [SerializeField] private TMP_Text averageCharacterSpeedText;
    [SerializeField] private TMP_Text mistakeCountText;
    [SerializeField] private TMP_Text characterCountText;

    protected override void Awake()
    {
        codeMiniGameAudioSource = GetComponent<AudioSource>();
        codeMiniGameAudioSource.volume = audioLevel;
        promptDisplay.text = "";
        codeMiniGameActive = false;
    }
    
    private void Update()
    {
        if (codeMiniGameActive)
        {
            UpdateTimer();
            if (!inputLocked)
            {
                HandleCodeMiniGameInput();
            }
        }
    }

    private void Start()
    {
        codeStartingScreen.SetActive(true);
        gameObject.SetActive(false);
        endScreen.SetActive(false);
    }

    public override void StartMiniGame()
    {
        promptDisplay.text = "";
        codeMiniGameActive = false;
        promptCount = 0;
        promptSet = false;
        userPressedFirstKey = false;
        currentCharacterIndex = 0;
        gameObject.SetActive(true);
        codeStartingScreen.SetActive(true);
        StartCoroutine(StartGameAfterDelay(screenOpeningDelay));
        base.StartMiniGame();
    }

    private IEnumerator StartGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        codeStartingScreen.SetActive(false);
        promptCount = 0;
        StartPrompts();
    }
    
    private void StartPrompts()
    {
        codeMiniGameActive = true;
        inputLocked = false;
        if (promptSet == false)
        {
            if (promptCount <= promptsPerGame - 1)
            {
                currentPrompt = codingPrompts[Random.Range(0, codingPrompts.Count)];
                promptText.text = currentPrompt;
                promptSet = true;
                promptCount++;
                startTime = Time.time;
            }
            else
            {
                ShowEndScreen();
            }
        }
    }
    
    protected override void ShowEndScreen()
    {
        codeMiniGameActive = false;
        codeStartingScreen.SetActive(true);
        base.ShowEndScreen();
        float averagePromptSpeed = totalTimeTaken / promptsPerGame;
        float averageCharacterSpeed = totalTimeTaken / totalCharactersTyped;
        averagePromptSpeedText.text = averagePromptSpeed.ToString("F2");
        averageCharacterSpeedText.text = averageCharacterSpeed.ToString("F2");
        mistakeCountText.text = totalMistakes.ToString();
        characterCountText.text = totalCharactersTyped.ToString();
    }

    protected override void EndMiniGame()
    {
        codeMiniGameActive = false;
        codeStartingScreen.SetActive(true);
        base.EndMiniGame();
    }

    public void HandleCodeMiniGameInput()
    {
        string input = Input.inputString;
        if (!string.IsNullOrEmpty(input))
        {
            // Handle first key press
            if (!userPressedFirstKey)
            {
                userPressedFirstKey = true;
                startTime = Time.time;
            }
            
            char lastInputChar = input[input.Length - 1];
            if (currentCharacterIndex < currentPrompt.Length && lastInputChar == currentPrompt[currentCharacterIndex])
            {
                // Correct character
                currentCharacterIndex++;
                totalCharactersTyped++;
                UpdatePromptDisplay();
            }
            else if(currentCharacterIndex < currentPrompt.Length)
            {
                // Incorrect character
                totalMistakes++;
                StartCoroutine(ShowErrorFeedback(lastInputChar));
            }
        }

        // Check for completion of the current prompt
        if (currentCharacterIndex == currentPrompt.Length)
        {
            // Handle prompt completion
            CompletePrompt();
        }
    }
    
    private void CompletePrompt()
    {
        // Completion logic, such as scoring
        codeMiniGameActive = false;
        inputLocked = true;
        codeMiniGameAudioSource.PlayOneShot(correctSound);
        timeTaken = Time.time - startTime;
        completionTimes.Add(timeTaken);
        punktideJaTundideHaldaja.TriggerAction(PunktideJaTundideHaldaja.ActionType.coding);
        StartCoroutine(WaitBeforeNewPrompt());
    }
    
    private void UpdatePromptDisplay()
    {
        // Update the displayed text based on the current index
        promptDisplay.text = currentPrompt.Substring(0, currentCharacterIndex);
    }
    
     protected override void UpdateTimer()
    {
        if (!userPressedFirstKey) return;
        base.UpdateTimer();
    }
    
    IEnumerator ShowErrorFeedback(char wrongChar)
    {
        inputLocked = true;
        codeMiniGameAudioSource.PlayOneShot(wrongSound);

        // Show the wrong character temporarily
        string errorText = $"<color=#{ColorUtility.ToHtmlStringRGB(errorColor)}>{wrongChar}</color>";
        promptDisplay.text += errorText;
        yield return new WaitForSeconds(colorFeedbackDelay);

        // Remove the wrong character and unlock input
        promptDisplay.text = promptDisplay.text.Replace(errorText, string.Empty);
        inputLocked = false;
    }
    
    IEnumerator WaitBeforeNewPrompt()
    {
        yield return new WaitForSeconds(newPromptDelay);
        promptSet = false;
        promptDisplay.text = "";
        currentCharacterIndex = 0;
        StartPrompts();
    }

    private int CalculatePoints(float time)
    {
        return Mathf.Max(0, 100 - (int)(time * 10)); // Example calculation
    }
    
    public void CodeMiniGameChangeVolumeLevel(float audioLevel)
    {
        this.audioLevel = audioLevel;
        codeMiniGameAudioSource.volume = audioLevel;
    }
    
    public override void OnLeaveButtonClicked()
    {
        totalMistakes = 0;
        totalCharactersTyped = 0;
        averagePromptSpeedText.text = "0";
        averageCharacterSpeedText.text = "0";
        mistakeCountText.text = "0";
        characterCountText.text = "0";
        base.OnLeaveButtonClicked();
    }
}
