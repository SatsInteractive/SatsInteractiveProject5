using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WC : MonoBehaviour
{
    public PunktideJaTundideHaldaja PunktideJaTundideHaldaja;
    public Dialogue Dialogue;
    public float demonWait = 3f;
    public Image currentCard;
    public Sprite normalCard;
    public Sprite demonCard;

    private void OnEnable()
    {
        //PunktideJaTundideHaldaja.TriggerAction(PunktideJaTundideHaldaja.ActionType.shitting);
        Dialogue.transform.parent.gameObject.SetActive(true);
        currentCard.sprite = normalCard;
        Dialogue.StartDialogue(Dialogue.dialoguePlaceOptions.WC);
        StartCoroutine(WaitForDemonCard());
    }
    
    private IEnumerator WaitForDemonCard()
    {
        yield return new WaitForSeconds(demonWait);
        currentCard.sprite = demonCard;
    }
}
