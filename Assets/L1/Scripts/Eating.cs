using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Eating : MonoBehaviour
{
    public PunktideJaTundideHaldaja PunktideJaTundideHaldaja;
    public float eatingNegativeMultiplier = 0.8f;
    private int timesPizzaClicked;
    private GameObject mouseFollowingColliderGameObject;
    private Collider2D mouseFollowingCollider;
    public GameObject mouseFollowingColliderPrefab;
    public List<GameObject> PizzaButtons;
    private Vector3 mousePosition;

    public void StartEatingAction()
    {

    }

    public void MumbleClicked()
    {
        PunktideJaTundideHaldaja.TriggerAction(PunktideJaTundideHaldaja.ActionType.mumbling);
        //code conversation!
    }

    public void PizzaClicked()
    {
        if (timesPizzaClicked < 1)
        {
            PunktideJaTundideHaldaja.TriggerAction(PunktideJaTundideHaldaja.ActionType.eating);
        }
        else if (timesPizzaClicked == 5)
        {
            PunktideJaTundideHaldaja.ActionInfo[(int)PunktideJaTundideHaldaja.ActionType.eating].nextMultiplier = eatingNegativeMultiplier;
            PunktideJaTundideHaldaja.TriggerAction(PunktideJaTundideHaldaja.ActionType.eating);
        }
        else if (timesPizzaClicked > 5)
        { 
            PunktideJaTundideHaldaja.TriggerAction(PunktideJaTundideHaldaja.ActionType.eating);
        }
        timesPizzaClicked += 1;
    }

    private void Awake()
    {
        timesPizzaClicked = 0;
        foreach (var variablePizzaButton in PizzaButtons)
        {
            variablePizzaButton.SetActive(true);   
        }
    }
}
