using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEndManager : MonoBehaviour
{
    public GameObject leaderboard; // Reference to the leaderboard GameObject containing team entries

    private List<TeamEntry> teamEntries = new List<TeamEntry>();

    public List<float> teamScores = new List<float> { 3.0f, 2.2f, 4.6f };
    public List<string> teamNames = new List<string> { "Jobud", "Kovad mehed", "Wieners" };

    private GameObject gameManager;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        // Initialize team entries from the leaderboard
        InitializeTeamEntries();

        AddPlayerEntry();

        // Sort the team entries based on scores in descending order
        teamEntries.Sort((a, b) => b.Score.CompareTo(a.Score));

        // Update the UI elements with placement, team name, and score
        UpdateUIElements();
    }

    private void AddPlayerEntry()
    {
        // Add the player's score to the list (replace this with the actual player's score)
        float playerScore = 3.6f;
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
        for (int i = 0; i < leaderboard.transform.childCount - 1; i++)
        {
            TeamEntry teamEntry = new TeamEntry {
                TeamName = teamNames[i],
                Score = teamScores[i]
            };
            
            teamEntries.Add(teamEntry);

        }
    }


    private void UpdateUIElements()
    {
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
            teamScoreText.text = teamEntries[i].Score.ToString();
        }
    }

    // Class to represent a team entry
    private class TeamEntry
    {
        public string TeamName { get; set; }
        public float Score { get; set; }
    }
}
