using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameEndManager : MonoBehaviour
{
    public GameObject leaderboard;
    public GameObject endingScreen;
    public Button restartButton;
    public Button leaveButton;

    private List<TeamEntry> teamEntries = new List<TeamEntry>();

    public List<float> teamScores = new List<float> { 3.0f, 2.2f, 4.6f };
    public List<string> teamNames = new List<string> { "Jobud", "Kovad mehed", "Wieners" };
    public PunktideJaTundideHaldaja PunktideJaTundideHaldaja;
    private Vector3 playerScoreXYZ;
    private float playerScore;

    public GameObject overallValue;
    public GameObject aestheticsValue;
    public GameObject innovationValue;
    public GameObject enjoymentValue;

    private GameManager gameManager;

    private void Awake()
    {
        restartButton.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        restartButton.GetComponent<Button>().onClick.AddListener(HandleRestartButtonPressed);
        leaveButton.GetComponent<Button>().onClick.AddListener(HandleQuitButtonPressed);
    }

    private void OnDisable()
    {
        //restartButton.GetComponent<Button>().onClick.RemoveListener(HandleRestartButtonPressed);
        //leaveButton.GetComponent<Button>().onClick.RemoveListener(HandleQuitButtonPressed);
    }

    public GameObject scoreBoard;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        // Initialize team entries from the leaderboard
        InitializeTeamEntries();
    }

    public void EndTheFuckingGame()
    {
        restartButton.gameObject.SetActive(true);
        AddPlayerEntry();

        // Sort the team entries based on scores in descending order
        teamEntries.Sort((a, b) => b.Score.CompareTo(a.Score));

        // Update the UI elements with placement, team name, and score
        UpdateUIElements();
        leaderboard.transform.parent.gameObject.SetActive(true);
    }

    private void AddPlayerEntry()
    {
        // Add the player's score to the list (replace this with the actual player's score)
        playerScoreXYZ = PunktideJaTundideHaldaja.points;
        playerScore = (playerScoreXYZ.x+playerScoreXYZ.y+playerScoreXYZ.z)/3f;
        string playerName = gameManager.GetComponent<GameManager>().playerName;
        teamScores.Add(playerScore);
        teamNames.Add(playerName);

        TeamEntry teamEntry = new TeamEntry
        {
            TeamName = playerName,
            Score = playerScore
        };
        teamEntries.Add(teamEntry);
    }

    private void InitializeTeamEntries()
    {
        // Iterate through each team entry in the leaderboard
        for (int i = 0; i < leaderboard.transform.childCount; i++)
        {
            TeamEntry teamEntry = new TeamEntry {
                TeamName = teamNames[i],
                Score = teamScores[i]
            };
            
            teamEntries.Add(teamEntry);

        }
    }

    private void UpdateScoreBoard()
    {
        overallValue.GetComponent<TMP_Text>().text = playerScore.ToString("0.000");
        aestheticsValue.GetComponent<TMP_Text>().text = playerScoreXYZ.x.ToString("0.000");
        innovationValue.GetComponent<TMP_Text>().text = playerScoreXYZ.y.ToString("0.000");
        enjoymentValue.GetComponent<TMP_Text>().text = playerScoreXYZ.z.ToString("0.000");
    }


    private void UpdateUIElements()
    {
        UpdateScoreBoard();
        // Iterate through each UI element and update its text components
        for (int i = 0; i < leaderboard.transform.childCount; i++)
        {
            Transform uiElement = leaderboard.transform.GetChild(i);
            TMP_Text teamPlaceText = uiElement.GetChild(0).GetComponent<TMP_Text>();
            TMP_Text teamNameText = uiElement.GetChild(1).GetComponent<TMP_Text>();
            TMP_Text teamScoreText = uiElement.GetChild(2).GetComponent<TMP_Text>();

            // Set the placement number, team name, and score
            teamPlaceText.text = (i + 1).ToString(); // Placement numbers start from 1
            teamNameText.text = teamEntries[i].TeamName;
            teamScoreText.text = teamEntries[i].Score.ToString("0.000");
        }
    }
    
    public void HandleRestartButtonPressed()
    {
        gameManager.RestartGame();
        endingScreen.SetActive(false);
    }
    
    public void HandleQuitButtonPressed()
    {
        gameManager.ExitGame();
    }

    // Class to represent a team entry
    private class TeamEntry
    {
        public string TeamName { get; set; }
        public float Score { get; set; }
    }
}
