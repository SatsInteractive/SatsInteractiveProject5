using UnityEngine;
using TMPro;

public class FadingText : MonoBehaviour
{
    private float fadeInDuration = 3.2f; // Duration for the fade-in effect
    private float currentFadeTime = 0f;

    private TMP_Text dayText;
    private Canvas canvas;

    void Start()
    {
        dayText = transform.GetChild(1).GetComponent<TMP_Text>();
        canvas = GetComponent<Canvas>();

        // Set intial alpha to 0 for a complete fade-in effect
        dayText.alpha = 0f;
    }

    void Update()
    {
        // Check if the current fade time is less than the specified duration
        if (currentFadeTime < fadeInDuration)
        {
            // Increment the fade time
            currentFadeTime += Time.deltaTime;

            // Calculate the normalized alpha value based on the current fade time
            float alpha = Mathf.Clamp01(currentFadeTime / fadeInDuration);

            // Apply the alpha to the CanvasGroup
            dayText.alpha = alpha;
        }
    }
}