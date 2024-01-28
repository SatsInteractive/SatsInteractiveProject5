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
    public float bugPathFollowDuration = 5f;
    public float bugPathFindRepeatRate = 5f;
    public bool bugIsAlive = true;
    
    private Vector3 targetPosition;
    public float bugSpeed = 50f;
    
    public float halfWidth;
    public float halfHeight;
    private float movement_start_time;
    private float passed_time;

    private void Start()
    {
        bugIsAlive = true;
        movement_start_time = Time.time; 
    }

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!bugIsAlive) return;
        RectTransform.position = Vector3.MoveTowards(RectTransform.position, targetPosition, bugSpeed * Time.deltaTime);
        if (passed_time > bugPathFollowDuration)
        {
            passed_time = 0;
            movement_start_time = Time.time;
            ChangeTargetPosition();
        }
        else
        {
            passed_time = Time.time - movement_start_time;
        }
    }

    private void ChangeTargetPosition()
    {

        float minX = -100; // replace with your value
        float maxX = 100; // replace with your value
        float minY = -100; // replace with your value
        float maxY = 100; // replace with your value

        // Calculate a random position within these borders
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        // Set the target position to this new position
        targetPosition = new Vector3(x, y, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnBugClicked?.Invoke(this);
        bugIsAlive = false;
    }
}
