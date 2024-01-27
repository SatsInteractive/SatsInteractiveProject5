using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public PunktideJaTundideHaldaja PunktideJaTundideHaldaja;
    public Dialogue Dialogue;

    private void OnEnable()
    {
        //PunktideJaTundideHaldaja.TriggerAction(PunktideJaTundideHaldaja.ActionType.shitting);
        Dialogue.transform.parent.gameObject.SetActive(true);
        Dialogue.StartDialogue(Dialogue.dialoguePlaceOptions.Hex);
    }
}
