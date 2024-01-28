using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Bug : MonoBehaviour, IPointerClickHandler
{
    public System.Action<Bug> OnBugClicked;
    public RectTransform RectTransform;
    public RectTransform gameAreaRectTransform;

    private Vector3 targetPosition;
    public float bugSpeed = 50f;

    private void Start()
    {
        ChangeTargetPosition();
        InvokeRepeating("ChangeTargetPosition", 5f, 5f);
    }

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        RectTransform.position = Vector3.MoveTowards(RectTransform.position, targetPosition, bugSpeed * Time.deltaTime);
    }

    private void ChangeTargetPosition()
    {
        // Calculate the half-width and half-height of the game area
        float halfWidth = gameAreaRectTransform.rect.width / 2;
        float halfHeight = gameAreaRectTransform.rect.height / 2;

        // Calculate a random position within these bounds
        float x = Random.Range(-halfWidth + RectTransform.rect.width / 2, halfWidth - RectTransform.rect.width / 2);
        float y = Random.Range(-halfHeight + RectTransform.rect.height / 2, halfHeight - RectTransform.rect.height / 2);

        // Set the target position to this new position
        targetPosition = new Vector3(x, y, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnBugClicked?.Invoke(this);
    }
}
