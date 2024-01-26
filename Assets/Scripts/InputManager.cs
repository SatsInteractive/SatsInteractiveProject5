using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private UIManager uiManager;
    private PointsManager pointsManager;
    private PlayerController playerController;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        pointsManager = GetComponent<PointsManager>();
        uiManager = FindObjectOfType<UIManager>();
    }
    
    public void HandleMovementInput()
    {
        playerController.moveInput.x = Input.GetAxis("Horizontal");
        playerController.moveInput.y = Input.GetAxis("Vertical");
    }

    public void HandleUIInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.HandleSettingsButtonPressed();
        }
    }
    
    public void HandlePointsInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pointsManager.AddPoints(10);
        }
    }
}
