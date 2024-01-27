using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkButton : MonoBehaviour
{
    public Eating Eating;

    public void HeadClicked()
    {
        Eating.MumbleClicked();
        print("psh-psh");
        Eating.PizzaButtons.Add(gameObject);
        gameObject.SetActive(false);
    }
}
