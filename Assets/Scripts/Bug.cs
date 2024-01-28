using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bug : MonoBehaviour, IPointerClickHandler
{
    public System.Action<Bug> OnBugClicked;
    public RectTransform RectTransform;
    [SerializeField] public Color highlightColor = Color.green;
    [SerializeField] public Color normalColor = Color.white;
    public float highlightDuration = 0.2f;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnBugClicked?.Invoke(this);
        // Higlight the bug
        StartCoroutine(HighlightBug());
    }
    
    private IEnumerator HighlightBug()
    {
        RectTransform.gameObject.GetComponent<Image>().color = highlightColor;
        yield return new WaitForSeconds(highlightDuration);
        RectTransform.gameObject.GetComponent<Image>().color = normalColor;
    }
}
