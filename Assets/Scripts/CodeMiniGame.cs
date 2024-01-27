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
    private bool inputLocked = false;
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
    private string userInput = "";
    private int currentCharacterIndex = 0;

    private void Awake()
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
                EndMiniGame();
            }
        }
    }

    public override void EndMiniGame()
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
                UpdatePromptDisplay();
            }
            else if(currentCharacterIndex < currentPrompt.Length)
            {
                // Incorrect character
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
        int points = CalculatePoints(timeTaken);
        Debug.Log("Correct! Points: " + points);
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
}
