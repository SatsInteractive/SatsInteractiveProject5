using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class ColorMatchingMinigame : MiniGame
{
    public GameObject ColorPaletteGameObject;
    public GameObject BigGridGameObject;
    public GameObject ExampleGridGameObject;
    [SerializeField] private GameObject colorMatchingMinigameStartingScreen;

    private Button[] colorPalette; // UI buttons for colors
    private GameObject[] bigGrid; // Larger grid of Image components for coloring
    private Image[] exampleGrid;

    public Button submitButton;

    private int[] correctGridIds; // Correct grid IDs
    private int[] playerGridIds; // Player's current grid IDs
    private int selectedColorId; // Currently selected color ID
    private float timer = 60f; // Initial timer value

    [SerializeField] private int artCount = 0;
    [SerializeField] private int artsPerGame = 2;

    public GameObject colormatching_side_bg;

    //from Lauri's system
    private bool isColorMatchingMinigameActive = false;

    private void Awake()
    {
        // Find Image arrays from the specified GameObjects
        colorPalette = ColorPaletteGameObject.GetComponentsInChildren<Button>();
        bigGrid = BigGridGameObject.GetComponentsInChildren<Transform>() // Use Transform instead of GameObject
            .Where(t => t != BigGridGameObject.transform) // Exclude the parent object
            .Select(t => t.gameObject) // Convert back to GameObject
            .ToArray();
        exampleGrid = ExampleGridGameObject.GetComponentsInChildren<Image>();

        isColorMatchingMinigameActive = false;
    }

    private void Start()
    {
        // Initialize player grid IDs
        playerGridIds = new int[bigGrid.Length];

        // Set up color palette button callbacks
        for (int i = 0; i < colorPalette.Length; i++)
        {
            int colorId = i;
            colorPalette[i].GetComponent<Button>().onClick.AddListener(() => OnColorSelected(colorId));
            colorPalette[i].GetComponent<Outline>().enabled = false;
        }

        // Set up bigGrid button callbacks
        for (int i = 0; i < bigGrid.Length; i++)
        {
            int gridIndex = i;
            bigGrid[i].GetComponent<Button>().onClick.AddListener(() => OnBigGridElementClicked(gridIndex));
        }


        // The first color is selected
        colorPalette[0].GetComponent<Outline>().enabled = true;

        // Set up submit button callback
        submitButton.onClick.AddListener(SubmitGrid);

        // Start the timer
        //InvokeRepeating("UpdateTimer", 1f, 1f);

        // Generate random color IDs for the correct grid
        GenerateRandomGrid();

        colorMatchingMinigameStartingScreen.SetActive(true);
        colormatching_side_bg.SetActive(false);
    }

    public override void StartMiniGame()
    {
        isColorMatchingMinigameActive = false;
        gameObject.SetActive(true);
        inputLocked = true;
        colorMatchingMinigameStartingScreen.SetActive(true);
        StartCoroutine(StartGameAfterDelay(screenOpeningDelay));
    }

    private void Update()
    {
        if (isColorMatchingMinigameActive)
        {
            UpdateTimer();
        }
    }

    private void GenerateRandomGrid()
    {
        correctGridIds = new int[bigGrid.Length];


        for (int i = 0; i < exampleGrid.Length; i++)
        {
            correctGridIds[i] = Random.Range(0, colorPalette.Length);

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
        // Assuming color IDs start from 0
        int arrayIndex = colorId;

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

    }

    private void OnBigGridElementClicked(int gridIndex)
    {
        GameObject clickedObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        if (clickedObject != null && clickedObject.CompareTag("BigGridSquare"))
        {
            int squareIndex = System.Array.IndexOf(bigGrid, clickedObject);
            if (squareIndex != -1)
            {
                playerGridIds[squareIndex] = selectedColorId;
                clickedObject.GetComponent<Image>().color = GetColor(selectedColorId);
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
            EndMiniGame();
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
            case 0:
                return Color.green;
            case 1:
                return Color.red;
            case 2:
                return Color.blue;
            case 3:
                return Color.yellow;
            case 4:
                return Color.magenta;
            default:
                return Color.white;
        }
    }

    private IEnumerator StartGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        colorMatchingMinigameStartingScreen.SetActive(false);
        StartPaintings();
    }

    private void StartPaintings()
    {
        isColorMatchingMinigameActive = true;
        colormatching_side_bg.SetActive(true);
        inputLocked = false;
        
        if (artCount <= artsPerGame - 1)
        {

            artCount++;
            startTime = Time.time;
        }
        else
        {
            EndMiniGame();
        }
    }

    protected override void EndMiniGame()
    {
        isColorMatchingMinigameActive = false;
        colorMatchingMinigameStartingScreen.SetActive(true);
        base.EndMiniGame();
    }
}

