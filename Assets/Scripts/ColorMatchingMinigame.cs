using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ColorMatchingMinigame : MonoBehaviour
{
    public GameObject ColorPaletteGameObject;
    public GameObject BigGridGameObject;
    public GameObject ExampleGridGameObject;

    private Button[] colorPalette; // UI buttons for colors
    private Image[] bigGrid; // Larger grid of Image components for coloring
    private Image[] exampleGrid;

    public Button submitButton;
    public Text timerText;

    private int[] correctGridIds; // Correct grid IDs
    private int[] playerGridIds; // Player's current grid IDs
    private int selectedColorId; // Currently selected color ID
    private float timer = 60f; // Initial timer value

    private void Awake()
    {
        // Find Image arrays from the specified GameObjects
        colorPalette = ColorPaletteGameObject.GetComponentsInChildren<Button>();
        bigGrid = BigGridGameObject.GetComponentsInChildren<Image>();
        exampleGrid = ExampleGridGameObject.GetComponentsInChildren<Image>();

    }

    private void Start()
    {
        // Initialize player grid IDs
        playerGridIds = new int[bigGrid.Length];

        // Set up color palette button callbacks
        for (int i = 0; i < colorPalette.Length; i++)
        {
            int colorId = i + 1; // Assuming color IDs start from 1
            colorPalette[i].GetComponent<Button>().onClick.AddListener(() => OnColorSelected(colorId));
            colorPalette[i].GetComponent<Outline>().enabled = false;
        }

        // The first color is selected
        colorPalette[0].GetComponent<Outline>().enabled = true;

        // Set up submit button callback
        submitButton.onClick.AddListener(SubmitGrid);

        // Start the timer
        //InvokeRepeating("UpdateTimer", 1f, 1f);

        // Generate random color IDs for the correct grid
        GenerateRandomGrid();
    }

    //private void GenerateRandomGrid()
    //{
    //    correctGridIds = new int[bigGrid.Length];
    //    for (int i = 0; i < correctGridIds.Length; i++)
    //    {
    //        correctGridIds[i] = Random.Range(1, colorPalette.Length + 1);
    //        exampleGrid[i].color = GetColor(i);
    //        bigGrid[i].color = Color.white; // Set big grid squares to white initially
    //    }
    //}
    private void GenerateRandomGrid()
    {
        correctGridIds = new int[bigGrid.Length];

        Debug.Log($"Example Grid Length: {exampleGrid.Length}");

        for (int i = 0; i < exampleGrid.Length; i++)
        {
            correctGridIds[i] = Random.Range(1, colorPalette.Length + 1);

            if (exampleGrid[i] != null)
            {
                exampleGrid[i].color = GetColor(correctGridIds[i]);
            }
            else
            {
                Debug.LogError($"Example Grid Image at index {i} is null!");
            }
        }
    }



    public void OnColorSelected(int colorId)
    {
        // Assuming color IDs start from 1
        int arrayIndex = colorId - 1;

        // Disable outline for all colors
        foreach (Button colorImage in colorPalette)
        {
            Outline outline = colorImage.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }

        // Enable outline for the selected color
        if (arrayIndex >= 0 && arrayIndex < colorPalette.Length)
        {
            Outline selectedOutline = colorPalette[arrayIndex].GetComponent<Outline>();
            if (selectedOutline != null)
            {
                selectedOutline.enabled = true;
            }
        }

        selectedColorId = colorId;

        Debug.Log("New color selected. The selected colorId: " + selectedColorId);
    }

    private void Update()
    {
        // Check for user input to color the big grid
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                if (clickedObject != null && clickedObject.CompareTag("BigGridSquare"))
                {
                    Debug.Log("BigGridSquare clicked: " + clickedObject.tag);
                    int squareIndex = System.Array.IndexOf(bigGrid, clickedObject.GetComponent<Image>());
                    if (squareIndex != -1)
                    {
                        playerGridIds[squareIndex] = selectedColorId;
                        clickedObject.GetComponent<Image>().color = GetColor(selectedColorId);
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

    //private void UpdateTimer()
    //{
    //    // Update and display the timer
    //    timer -= 1f;
    //    timerText.text = "Time: " + Mathf.Ceil(timer).ToString();

    //    // Check if time is up
    //    if (timer <= 0f)
    //    {
    //        Debug.Log("Time's up! Minigame incomplete.");
    //        // End the minigame, perhaps by transitioning to another scene or showing a failure message
    //    }
    //}
}

