using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaButton : MonoBehaviour
{
    public Eating Eating;

    public void PizzaSliceClicked()
    {
        Eating.PizzaClicked();
        print("pizza!");
        Eating.PizzaButtons.Add(gameObject);
        gameObject.SetActive(false);
    }
}
