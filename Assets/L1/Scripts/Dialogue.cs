using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float typingDelay;

    private int index;

    private void Awake()
    {
        textComponent.text = String.Empty;
    }

    private void Start()
    {
        StartDialogue();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = String.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingDelay);
        }
    }
}
