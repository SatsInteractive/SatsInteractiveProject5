using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunktideJaTundideHaldaja : MonoBehaviour
{
    public float points;
    public int hours;
    public int day;
    public float nextMultiplierTotal;
    private int[] endhours;
    private List<ActionType> actionHistory;
    public Action[] ActionInfo;

    public enum ActionType
    {
        eating,
        shitting,
        coding,
        art,
        mumbling,
    }
    [System.Serializable]
    public struct Action
    {
        public string type;
        public float addedPoints;
        public float nextMultiplier;
        public int availability;
        public int hoursUsed;
    }

    private void Awake()
    {
        actionHistory = new List<ActionType>();
        endhours = new[] { 2400, 2400, 1600};
    }
    
    public void TriggerAction(ActionType actionType)
    {
        Action actionTriggered = ActionInfo[(int)actionType];
        if (actionTriggered.availability < 1)
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
        hours += 200;
        actionHistory.Add(actionType);

        if (hours == endhours[day])
        {
            NextDay();
        }
    }

    private void NextDay()
    {
        day += 1;
        hours = 0800;
        if (day == 1)
        {
            //Trigger next part of ending cutscene
            
        }
        else if (day == 2)
        {
            //Trigger another end ceremony cutscene
        }
        else if (day == 3)
        {
            // END THE FUCKING GAME!!!!!!!!!!!!!!!!!!!!!!
        }
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
