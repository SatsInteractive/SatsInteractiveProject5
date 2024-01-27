using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueButton : MonoBehaviour
{
    public Dialogue Dialogue;
    private TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        textMeshProUGUI = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void DialogueButtonClicked()
    {
        Dialogue.DialogueOptionChosen(textMeshProUGUI.text);
    }
}
