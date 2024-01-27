using UnityEngine;
using System.Collections;

public class MapIconAnimator : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float moveAmount = 0.1f;
    public float scaleAmount = 0.01f;
    private Vector3 originalPosition;
    private Vector3 originalScale;

    void Start()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;
        StartCoroutine(AnimateIcon());
    }

    IEnumerator AnimateIcon()
    {
        while (true)
        {
            // Calculate the new position
            float newY = originalPosition.y + moveAmount * Mathf.Sin(Time.time * moveSpeed);
            transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);

            // Scale up when moving up, scale down when moving down
            float scale = 1 + scaleAmount * Mathf.Sin(Time.time * moveSpeed);
            transform.localScale = originalScale * scale;

            yield return null;
        }
    }
}

