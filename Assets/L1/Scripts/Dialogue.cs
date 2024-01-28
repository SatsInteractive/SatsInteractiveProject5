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
    public string[] HexDialogue;
    public string[] TiksuDialogue;

    private int index;
    public List<GameObject> dialogueButtons;
    public List<TextMeshProUGUI> dialogueButtonsTexts;
    private bool dialogueSkippable=false;
    private int nextDialogueIndex;
    public PunktideJaTundideHaldaja PunktideJaTundideHaldaja;
    private PunktideJaTundideHaldaja.ActionType currentDialoguePlaceActionType;
    private bool tiksuAppearAfterButtonClick=false;
    public GameObject TiksuUIGameObject;

    public enum dialoguePlaceOptions
    {
        Kitchen,
        WC,
        Coop,
        Hex,
        Tiksu
    }
    private void Awake()
    {
        textComponent.text = String.Empty;
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) & dialogueSkippable)
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
            currentDialoguePlaceActionType = PunktideJaTundideHaldaja.ActionType.mumbling;
        }
        else if (dialoguePlace == dialoguePlaceOptions.WC)
        {
            linesToType = WCDialogue;
            linesToType[8] = "Innovation: " + PunktideJaTundideHaldaja.points.x + " Aesthetics: " + PunktideJaTundideHaldaja.points.y
                             + " Enjoyment: " + PunktideJaTundideHaldaja.points.z + ".";
            linesToType[11] = "Innovation: " + PunktideJaTundideHaldaja.points.x + " Aesthetics: " + PunktideJaTundideHaldaja.points.y
                              + " Enjoyment: " + PunktideJaTundideHaldaja.points.z + ".";
            tiksuAppearAfterButtonClick = true;
            currentDialoguePlaceActionType = PunktideJaTundideHaldaja.ActionType.shitting;
        }
        else if (dialoguePlace == dialoguePlaceOptions.Coop)
        {
            linesToType = CoopDialogue;
            currentDialoguePlaceActionType = PunktideJaTundideHaldaja.ActionType.coop;
        }
        else if (dialoguePlace == dialoguePlaceOptions.Hex)
        {
            linesToType = HexDialogue;
            currentDialoguePlaceActionType = PunktideJaTundideHaldaja.ActionType.hex;
        }
        else if (dialoguePlace == dialoguePlaceOptions.Tiksu)
        {
            linesToType = TiksuDialogue;
            currentDialoguePlaceActionType = PunktideJaTundideHaldaja.ActionType.tiksu;
        }
        PunktideJaTundideHaldaja.TriggerAction(currentDialoguePlaceActionType);
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
                TiksuUIGameObject.SetActive(false);
                gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(TypeLine());
            }
        }
        else
        {
            TiksuUIGameObject.SetActive(false);
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
        if (tiksuAppearAfterButtonClick)
        {
            tiksuAppearAfterButtonClick = false;
            TiksuUIGameObject.SetActive(true);
        }
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

    private void OnDisable()
    {
        TiksuUIGameObject.SetActive(false);
    }
}
