using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Search;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    private string[] linesToType;
    public float typingDelay;

    public string[] KitchenDialogue;
    public string[] WCDialogue;
    public string[] CoopDialogue;

    private int index;
    public List<GameObject> dialogueButtons;
    public List<TextMeshProUGUI> dialogueButtonsTexts;
    private bool dialogueSkippable=false;
    private int nextDialogueIndex;

    public enum dialoguePlaceOptions
    {
        Kitchen,
        WC,
        Coop
    }
    private void Awake()
    {
        textComponent.text = String.Empty;
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && dialogueSkippable)
        {
            if (textComponent.text == linesToType[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = linesToType[index];
            }
        }
    }

    public void StartDialogue(dialoguePlaceOptions dialoguePlace)
    {
        textComponent.text = String.Empty;
        index = 0;
        dialogueSkippable = true;
        if (dialoguePlace == dialoguePlaceOptions.Kitchen)
        {
            linesToType = KitchenDialogue;
        }
        else if (dialoguePlace == dialoguePlaceOptions.WC)
        {
            linesToType = WCDialogue;
        }
        else if (dialoguePlace == dialoguePlaceOptions.Coop)
        {
            linesToType = CoopDialogue;
        }

        StartCoroutine(TypeLine());
    }

    private void NextLine()
    {
        if (index < linesToType.Length - 1)
        {
            index++;
            textComponent.text = String.Empty;
            if (linesToType[index] == "PlayerOptions")
            {
                textComponent.text = linesToType[index - 1];
                index++;
                PlayerOptions(Int32.Parse(linesToType[index]));
            }
            else if (linesToType[index] == "END")
            {
                gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(TypeLine());
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void PlayerOptions(int playerOptions)
    {
        dialogueSkippable = false;
        for (int i = 0; i < playerOptions; i++)
        {
            index++;
            dialogueButtonsTexts[i].text = linesToType[index];
            dialogueButtons[i].SetActive(true);
        }
    }

    public void DisableDialogueOptions()
    {
        foreach (var varDialogueButton in dialogueButtons)
        {
            varDialogueButton.SetActive(false);
        }
    }

    public void DialogueOptionChosen(string buttonText)
    {
        DisableDialogueOptions();
        
        if (buttonText == dialogueButtonsTexts[0].text)
        {
            index += 1;
        }
        else if (buttonText == dialogueButtonsTexts[1].text)
        {
            index += 2;
        }
        else if (buttonText == dialogueButtonsTexts[2].text)
        {
            index += 3;
        }
        nextDialogueIndex = Int32.Parse(linesToType[index])-1;
        index = nextDialogueIndex;
        dialogueSkippable = true;
        NextLine();
    }

    private IEnumerator TypeLine()
    {
        foreach (char c in linesToType[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingDelay);
        }
    }
}
