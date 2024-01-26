using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CodeMiniGame : MonoBehaviour
{
    private TextMeshProUGUI promptText;
    private TMP_InputField inputField;
    private TextMeshProUGUI timerText;
    private AudioSource codeMiniGameAudioSource;
    
    private string currentPrompt;
    private float startTime;
    private float timeTaken;
    private bool codeMiniGameActive = false;

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
        "Camera.main.clearFlags = CameraClearFlags.SolidColor;",
        "void WhyAmIRunning() { }"
    };
    public Color normalColor = new Color(85, 85, 85, 0);
    public Color errorColor = new Color(106, 106, 68, 0);
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public float audioLevel;

    private void Awake()
    {
        promptText = transform.Find("code_prompt").GetComponent<TextMeshProUGUI>();
        inputField = transform.Find("code_input_field").GetComponent<TMP_InputField>();
        timerText = transform.Find("code_minigame_timer").GetComponent<TextMeshProUGUI>();
        codeMiniGameAudioSource = GetComponent<AudioSource>();
        codeMiniGameAudioSource.volume = audioLevel;
    }

    private void Start()
    {
        StartCodeMiniGame();
    }

    private void StartCodeMiniGame()
    {
        codeMiniGameActive = true;
        SetRandomPrompt();
    }

    private void Update()
    {
        if (codeMiniGameActive)
        {
            UpdateTimer();
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                CheckInput();
            }
        }
    }

    private void SetRandomPrompt()
    {
        currentPrompt = codingPrompts[Random.Range(0, codingPrompts.Count)];
        promptText.text = currentPrompt;
        inputField.text = "";
        inputField.ActivateInputField();
        startTime = Time.time;
    }
    
    private void UpdateTimer()
    {
        float timeElapsed = Time.time - startTime;
        timerText.text = "Time: " + timeElapsed.ToString("F2");
    }


    private void CheckInput()
    {
        if (inputField.text.Equals(currentPrompt))
        {
            codeMiniGameAudioSource.PlayOneShot(correctSound);
            codeMiniGameActive = false;
            timeTaken = Time.time - startTime;
            int points = CalculatePoints(timeTaken);
            Debug.Log("Correct! Points: " + points);
        }
        else
        {
            StartCoroutine(ShowErrorFeedback());
        }
    }
    
    IEnumerator ShowErrorFeedback()
    {
        codeMiniGameAudioSource.PlayOneShot(wrongSound);
        inputField.image.color = errorColor;
        yield return new WaitForSeconds(1);
        inputField.image.color = normalColor;
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
