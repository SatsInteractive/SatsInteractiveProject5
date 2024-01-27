using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMatchingMinigame : MonoBehaviour
{
    public Image[] colorPalette; // UI buttons for colors
    public Image[] bigGrid; // Larger grid of Image components for coloring
    public Button submitButton;
    public Text timerText;

    private int[] correctGridIds; // Correct grid IDs
    private int[] playerGridIds; // Player's current grid IDs
    private int selectedColorId; // Currently selected color ID
    private float timer = 60f; // Initial timer value

    private void Start()
    {
        // Initialize correct grid IDs (example IDs, replace with your own logic)
        correctGridIds = new int[] { 1, 2, 3, 4, 5 };
        

        // Initialize player grid IDs
        playerGridIds = new int[bigGrid.Length];

        // Set up color palette button callbacks
        for (int i = 0; i < colorPalette.Length; i++)
        {
            int colorId = i + 1; // Assuming color IDs start from 1
            colorPalette[i].GetComponent<Button>().onClick.AddListener(() => OnColorSelected(colorId));
        }

        // Set up submit button callback
        submitButton.onClick.AddListener(SubmitGrid);

        // Start the timer
        InvokeRepeating("UpdateTimer", 1f, 1f);
    }

    private void OnColorSelected(int colorId)
    {
        selectedColorId = colorId;
    }

    private void Update()
    {
        // Check for user input to color the big grid
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                Image clickedImage = hit.collider.GetComponent<Image>();
                if (clickedImage != null && clickedImage.tag == "BigGridSquare")
                {
                    int squareIndex = System.Array.IndexOf(bigGrid, clickedImage);
                    if (squareIndex != -1)
                    {
                        playerGridIds[squareIndex] = selectedColorId;
                        clickedImage.color = GetColor(selectedColorId);
                    }
                }
            }
        }
    }

    private void SubmitGrid()
    {
        // Check if player's grid IDs match the correct grid IDs
        bool isCorrect = CheckGrid();

        if (isCorrect)
        {
            Debug.Log("Minigame complete! Grid is correct.");
            // End the minigame, perhaps by transitioning to another scene or showing a completion message
        }
        else
        {
            Debug.Log("Incorrect grid. Keep trying!");
            // Optionally provide feedback to the player
        }
    }

    private bool CheckGrid()
    {
        for (int i = 0; i < correctGridIds.Length; i++)
        {
            if (playerGridIds[i] != correctGridIds[i])
            {
                return false;
            }
        }
        return true;
    }

    private Color GetColor(int colorId)
    {
        switch (colorId)
        {
            case 1:
                return Color.red;
            case 2:
                return Color.green;
            case 3:
                return Color.blue;
            case 4:
                return Color.yellow;
            case 5:
                return Color.magenta;
            default:
                return Color.white;
        }
    }

    private void UpdateTimer()
    {
        // Update and display the timer
        timer -= 1f;
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();

        // Check if time is up
        if (timer <= 0f)
        {
            Debug.Log("Time's up! Minigame incomplete.");
            // End the minigame, perhaps by transitioning to another scene or showing a failure message
        }
    }
}
