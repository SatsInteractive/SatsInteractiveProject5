using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public CodeMiniGameAction currentMiniGame;

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
    private List<string> tempCodingPrompts;
    [SerializeField] public Color normalColor = new Color(85, 85, 85, 1);
    [SerializeField] public Color errorColor = new Color(106, 106, 68, 1);
    [SerializeField] public Color placeHolderColor = new Color(85, 85, 85, 0.5f);
    public float newPromptDelay = 1f;
    public int promptsPerGame = 3;
    public float spaceFeedbackDuration = 0.2f;
    
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
    [SerializeField] private TMP_Text averagePromptSpeedTextTitle;
    [SerializeField] private TMP_Text averageCharacterSpeedTextTitle;
    [SerializeField] private TMP_Text mistakeCountTextTitle;
    [SerializeField] private TMP_Text characterCountTextTitle;
    
    [Header("Bug Finding Game")]
    public GameObject bugPrefab;
    private List<GameObject> bugs = new List<GameObject>();
    public RectTransform  codeBuggingScreen;
    [SerializeField] public Color highlightBugColor = Color.green;
    [SerializeField] public Color normalBugColor = Color.white;
    public float bugHighlightDuration = 0.2f;

    public enum CodeMiniGameAction 
    {
        SpeedTyping,
        BugFinding
    }

    protected override void Awake()
    {
        codeMiniGameAudioSource = GetComponent<AudioSource>();
        codeMiniGameAudioSource.volume = audioLevel;
        promptDisplay.text = "start typing immediately.. good luck!";
        promptDisplay.color = placeHolderColor;
        codeMiniGameActive = false;
        currentMiniGame = CodeMiniGameAction.BugFinding;
        codeBuggingScreen.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if (codeMiniGameActive)
        {
            UpdateTimer();

            if (!inputLocked)
            {
                if (currentMiniGame == CodeMiniGameAction.SpeedTyping)
                {
                    HandleCodeMiniGameInput();
                }
                else if (currentMiniGame == CodeMiniGameAction.BugFinding)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        CheckBugClick();
                    }
                }
            }
        }
    }
    
    private void CheckBugClick()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null && bugs.Contains(hit.collider.gameObject))
        {
            GameObject clickedBug = hit.collider.gameObject;
            StartCoroutine(HighlightBug(clickedBug));
            bugs.Remove(clickedBug);
            Destroy(clickedBug);
        }
    }
    
    
    private IEnumerator HighlightBug(GameObject clickedBug)
    {
        clickedBug.GetComponent<Image>().color = highlightBugColor;
        yield return new WaitForSeconds(bugHighlightDuration);
        clickedBug.GetComponent<Image>().color = normalBugColor;
    }
    
    public void EndBugFindingGame()
    {
        EndMiniGame();
    }

    private void Start()
    {
        codeStartingScreen.SetActive(true);
        gameObject.SetActive(false);
        endScreen.SetActive(false);
    }

    public override void StartMiniGame()
    {
        if (currentMiniGame == CodeMiniGameAction.SpeedTyping)
        {
            promptDisplay.text = "start typing immediately.. good luck!";
            promptDisplay.color = placeHolderColor;
            codeMiniGameActive = false;
            promptCount = 0;
            promptSet = false;
            userPressedFirstKey = false;
            currentCharacterIndex = 0;
            gameObject.SetActive(true);
            codeStartingScreen.SetActive(true);
            tempCodingPrompts = new List<string>(codingPrompts);
            StartCoroutine(StartGameAfterDelay(screenOpeningDelay));
        }
        else if (currentMiniGame == CodeMiniGameAction.BugFinding)
        {
            codeMiniGameActive = false;
            promptCount = 0;
            gameObject.SetActive(true);
            codeStartingScreen.SetActive(true);
            StartCoroutine(StartBugGameAfterDelay(screenOpeningDelay));
            
        }
        base.StartMiniGame();
    }
    
    private void InitializeBugFindingGame()
    {
        codeMiniGameActive = true;
        startTime = Time.time;
        userPressedFirstKey = true;
        for (int i = 0; i < promptsPerGame; i++)
        {
            GameObject bugObject = Instantiate(bugPrefab, codeBuggingScreen);
            Debug.Log("bugObject: " + bugObject.name);
            RectTransform bugRect = bugObject.GetComponent<RectTransform>();
            bugRect.anchoredPosition = GetRandomPosition();
            Bug bug = bugObject.AddComponent<Bug>();
            bug.OnBugClicked += BugClicked;
            
            bug.halfWidth = codeBuggingScreen.rect.width / 2;
            bug.halfHeight = codeBuggingScreen.rect.height / 2;
            bugs.Add(bugObject);
        }
    }
    
    private Vector2 GetRandomPosition()
    {
        // Calculate a random position within the game area's RectTransform
        float x = Random.Range(-codeBuggingScreen.rect.width / 2, codeBuggingScreen.rect.width / 2);
        float y = Random.Range(-codeBuggingScreen.rect.height / 2, codeBuggingScreen.rect.height / 2);
        return new Vector2(x, y);
    }
    
    private void BugClicked(Bug bug)
    {
        
        GameObject bugObject = bug.gameObject;
        bugs.Remove(bugObject);
        Destroy(bugObject);

        if (bugs.Count == 0)
        {
            // All bugs caught
            inputLocked = true;
            codeMiniGameAudioSource.PlayOneShot(successSound);
            ShowBugEndScreen();
        }
    }

    private IEnumerator StartGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        codeStartingScreen.SetActive(false);
        promptCount = 0;
        StartPrompts();
    }
    
    private IEnumerator StartBugGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        codeStartingScreen.SetActive(false);
        codeBuggingScreen.gameObject.SetActive(true);
        promptCount = 0;
        InitializeBugFindingGame();
    }
    
    private void StartPrompts()
    {
        codeMiniGameActive = true;
        inputLocked = false;
        startTime = Time.time;
        if (promptSet == false)
        {
            if (promptCount <= promptsPerGame - 1 && tempCodingPrompts.Count > 0)
            {
                int _index = Random.Range(0, tempCodingPrompts.Count);
                currentPrompt = tempCodingPrompts[_index];
                promptText.text = currentPrompt;
                tempCodingPrompts.RemoveAt(_index);
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
        timerText.text = "0";
        codeStartingScreen.SetActive(true);
        base.ShowEndScreen();
        punktideJaTundideHaldaja.TimeTakenForCodingOrArt(totalTimeTaken, PunktideJaTundideHaldaja.ActionType.coding);
        punktideJaTundideHaldaja.TriggerAction(PunktideJaTundideHaldaja.ActionType.coding);
        float averagePromptSpeed = totalTimeTaken / promptsPerGame;
        float averageCharacterSpeed = totalTimeTaken / totalCharactersTyped;
        averageCharacterSpeedTextTitle.text = "Average character speed:";
        averagePromptSpeedTextTitle.text = "Average line speed:";
        mistakeCountTextTitle.text = "Mistakes:";
        characterCountTextTitle.text = "Total characters:";
        averagePromptSpeedText.text = averagePromptSpeed.ToString("F2");
        averageCharacterSpeedText.text = averageCharacterSpeed.ToString("F2");
        mistakeCountText.text = totalMistakes.ToString();
        characterCountText.text = totalCharactersTyped.ToString();
    }

    private void ShowBugEndScreen()
    {
        codeMiniGameActive = false;
        codeBuggingScreen.gameObject.SetActive(false);
        timerText.text = "0";
        codeStartingScreen.SetActive(true);
        totalTimeTaken = Time.time - startTime;
        punktideJaTundideHaldaja.TimeTakenForCodingOrArt(totalTimeTaken, PunktideJaTundideHaldaja.ActionType.coding);
        punktideJaTundideHaldaja.TriggerAction(PunktideJaTundideHaldaja.ActionType.coding);
        float averageCatchSpeed = totalTimeTaken / promptsPerGame;
        averageCharacterSpeedTextTitle.text = "";
        mistakeCountText.text = "";
        characterCountText.text = "";
        mistakeCountTextTitle.text = "";
        characterCountTextTitle.text = "";
        averagePromptSpeedText.text = "";
        averageCharacterSpeedText.text = "";
        mistakeCountText.text = "";
        characterCountText.text = "";
        averagePromptSpeedTextTitle.text = "Average bugging speed:";
        averagePromptSpeedText.text = averageCatchSpeed.ToString("F2");
        
    }

    protected override void EndMiniGame()
    {
        codeMiniGameActive = false;
        codeStartingScreen.SetActive(true);
        completionTimes = new List<float>();
        punktideJaTundideHaldaja.TriggerAction(PunktideJaTundideHaldaja.ActionType.coding);
        if (currentMiniGame == CodeMiniGameAction.SpeedTyping)
        {
            currentMiniGame = CodeMiniGameAction.BugFinding;
        }
        else if (currentMiniGame == CodeMiniGameAction.BugFinding)
        {
            currentMiniGame = CodeMiniGameAction.SpeedTyping;
        }
        base.EndMiniGame();
    }

    public void HandleCodeMiniGameInput()
    {
        string input = Input.inputString;
        if (!string.IsNullOrEmpty(input))
        {
            if (input.Contains("\b"))
            {
                return;
            }
            
            // Handle first key press
            if (!userPressedFirstKey)
            {
                userPressedFirstKey = true;
                promptDisplay.text = "";
                promptDisplay.color = normalColor;
            }
            
            char lastInputChar = input[input.Length - 1];
            if (currentCharacterIndex < currentPrompt.Length && lastInputChar == currentPrompt[currentCharacterIndex])
            {
                // Correct character
                currentCharacterIndex++;
                totalCharactersTyped++;
                UpdatePromptDisplay();
                if (lastInputChar == ' ')
                {
                    StartCoroutine(ShowSpaceFeedback());
                }
            }
            else if(currentCharacterIndex < currentPrompt.Length)
            {
                // Incorrect character
                totalMistakes++;
                StartCoroutine(ShowErrorFeedback(lastInputChar));
            }
        }

        // Check for completion of the current prompt
        if (currentCharacterIndex == currentPrompt.Length && userPressedFirstKey == true)
        {
            // Handle prompt completion
            CompletePrompt();
        }
    }
    
    private void CompletePrompt()
    {
        codeMiniGameActive = false;
        inputLocked = true;
        promptDisplay.text = $"<color=green>{currentPrompt}</color>";
        codeMiniGameAudioSource.PlayOneShot(successSound);
        timeTaken = Time.time - startTime;
        completionTimes.Add(timeTaken);
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
    
    IEnumerator ShowSpaceFeedback()
    {
        // Temporarily show a green dot for the space
        string originalText = promptDisplay.text;
        promptDisplay.text = $"<color=green>{originalText}</color>";
        codeMiniGameAudioSource.PlayOneShot(correctSound);
        inputLocked = true;
        yield return new WaitForSeconds(spaceFeedbackDuration);  // Duration for showing the green dot

        // Revert back to the original text plus a space
        inputLocked = false;
        promptDisplay.text = originalText + " ";
    }
    
    IEnumerator WaitBeforeNewPrompt()
    {
        yield return new WaitForSeconds(newPromptDelay);
        promptSet = false;
        promptDisplay.color = normalColor;
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
        codeMiniGameAudioSource.volume = audioLevel * 0.25f;
    }
    
    public override void OnLeaveButtonClicked()
    {
        totalMistakes = 0;
        totalCharactersTyped = 0;
        averagePromptSpeedText.text = "0";
        averageCharacterSpeedText.text = "0";
        mistakeCountText.text = "0";
        characterCountText.text = "0";
        if (currentMiniGame == CodeMiniGameAction.SpeedTyping)
        {
            tempCodingPrompts.Clear();
        }
        else if (currentMiniGame == CodeMiniGameAction.BugFinding)
        {
            
        }
        base.OnLeaveButtonClicked();
    }
}
