using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunktideHaldaja : MonoBehaviour
{
    public float points;
    public float nextMultiplierTotal;
    private List<ActionType> actionHistory;
    public Action[] ActionInfo;

    public enum ActionType
    {
        eating,
        shitting,
        coding,
        art,
    }
    [System.Serializable]
    public struct Action
    {
        public string type;
        public float addedPoints;
        public float nextMultiplier;
        public int availability;
    }

    private void Awake()
    {
        actionHistory = new List<ActionType>();
    }
    
    private void TriggerAction(ActionType actionType)
    {
        Action actionTriggered = ActionInfo[(int)actionType];
        if (actionTriggered.availability > 0)
        {
            //Add additional error functionality, i.e what happens when the action cannot be triggered right now
            return;
        }
        if (actionTriggered.addedPoints!=0)
        {
            points += actionTriggered.addedPoints * nextMultiplierTotal;
            nextMultiplierTotal = 1f;
        }
        nextMultiplierTotal *= actionTriggered.nextMultiplier;
        actionHistory.Add(actionType);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerAction(ActionType.eating);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            TriggerAction(ActionType.coding);
        }
    }
}
