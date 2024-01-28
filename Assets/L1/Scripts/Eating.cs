using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Eating : MonoBehaviour
{
    public PunktideJaTundideHaldaja PunktideJaTundideHaldaja;
    public float eatingNegativeMultiplier = 1f;
    private int timesPizzaClicked;
    public List<GameObject> PizzaButtons;
    public Dialogue Dialogue;
    public AudioClip[] eatingSounds;
    public AudioClip krooks;
    public AudioSource audioSource;
    public AudioSource danielAudioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        danielAudioSource.Play();
    }

    public void MumbleClicked()
    {
        //PunktideJaTundideHaldaja.TriggerAction(PunktideJaTundideHaldaja.ActionType.mumbling);
        Dialogue.transform.parent.gameObject.SetActive(true);
        Dialogue.StartDialogue(Dialogue.dialoguePlaceOptions.Kitchen);
    }

    public void PizzaClicked()
    {
        print(timesPizzaClicked);
        if (timesPizzaClicked < 1)
        {
            PunktideJaTundideHaldaja.TriggerAction(PunktideJaTundideHaldaja.ActionType.eating);
        }
        timesPizzaClicked += 1;
        if (timesPizzaClicked == 5)
        {
            audioSource.clip = krooks;
            audioSource.Play();
        }
        else
        {
            PlayEatingSound();
        }
    }

    private void OnEnable()
    {
        timesPizzaClicked = 0;
        foreach (var variablePizzaButton in PizzaButtons)
        {
            variablePizzaButton.SetActive(true);   
        }
    }
    
    private void PlayEatingSound()
    {
        audioSource.clip = eatingSounds[UnityEngine.Random.Range(0, eatingSounds.Length)];
        audioSource.Play();
    }
    
    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume * 0.25f;
    }
}
