using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopping : MonoBehaviour
{
    public PunktideJaTundideHaldaja PunktideJaTundideHaldaja;
    public Dialogue Dialogue;

    private void OnEnable()
    {
        //PunktideJaTundideHaldaja.TriggerAction(PunktideJaTundideHaldaja.ActionType.coop);
        Dialogue.transform.parent.gameObject.SetActive(true);
        Dialogue.StartDialogue(Dialogue.dialoguePlaceOptions.Coop);
    }
}
