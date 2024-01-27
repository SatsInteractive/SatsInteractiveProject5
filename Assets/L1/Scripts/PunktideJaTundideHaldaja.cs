using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PunktideJaTundideHaldaja : MonoBehaviour
{
    public float points;
    public int hours;
    public int day;
    public float nextMultiplierTotal;
    private int[] endhours;
    private List<ActionType> actionHistory;
    private float timeTaken;
    public Action[] ActionInfo;
    public TextMeshProUGUI TimeAndDateTextMeshProUGUI;

    public enum ActionType
    {
        eating,
        shitting,
        coding,
        art,
        mumbling,
        coop
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
        TimeAndDateTextMeshProUGUI = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void TimeTakenForCodingOrArt(float timeTaken, ActionType actionType)
    {
        if (timeTaken < 15f)
        {
            nextMultiplierTotal *= 1f;
        }
        else if (timeTaken < 25f)
        {
            nextMultiplierTotal *= 0.8f;
        }
        else if (timeTaken < 35f)
        {
            nextMultiplierTotal *= 0.7f;
        }
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
        TimeAndDateTextMeshProUGUI.text = String.Empty;
        TimeAndDateTextMeshProUGUI.text += (hours / 100).ToString() + ":00" + "\n";
        if (day == 0)
        {
            TimeAndDateTextMeshProUGUI.text += "January 26";
        }
        else if (day == 1)
        {
            TimeAndDateTextMeshProUGUI.text += "January 27";
        }
        else if (day == 2)
        {
            TimeAndDateTextMeshProUGUI.text += "January 28";
        }
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
}
