using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bug : MonoBehaviour, IPointerClickHandler
{
    public System.Action<Bug> OnBugClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnBugClicked?.Invoke(this);
    }
}
