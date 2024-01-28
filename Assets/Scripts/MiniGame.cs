using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{
    protected float startTime;
    [SerializeField] protected PunktideJaTundideHaldaja punktideJaTundideHaldaja;
    [SerializeField] protected TextMeshProUGUI timerText;
    [SerializeField] protected AudioClip correctSound;
    [SerializeField] protected AudioClip successSound;
    [SerializeField] protected AudioClip wrongSound;
    [SerializeField] protected float audioLevel;
    [SerializeField] protected float colorFeedbackDelay = 0.5f;
    [SerializeField] protected List<float> completionTimes;
    [SerializeField] protected float totalTimeTaken;
    protected float screenOpeningDelay = 3f;
    protected bool inputLocked = false;
    
    [Header("End Screen Settings")]
    [SerializeField] protected GameObject endScreen;
    [SerializeField] protected TMP_Text totalTimeTakenText;
    
    protected virtual void Awake()
    {
        completionTimes = new List<float>();
        endScreen.SetActive(false);
    }

    public virtual void StartMiniGame()
    {
        Debug.Log("I was called: " + gameObject);
    }
    
    protected virtual void EndMiniGame()
    {
        
        gameObject.SetActive(false);
        MapManager.Instance.TeleportPlayerToSpawnPoint1();
        MapManager.Instance.ShowMap();
    }

    protected virtual void ShowEndScreen()
    {
        Debug.Log("ShowEndScreen() called");
        for (int i = 0; i < completionTimes.Count; i++)
        {
            totalTimeTaken += completionTimes[i];
        }
        endScreen.SetActive(true);
        totalTimeTakenText.text = totalTimeTaken.ToString("F2");
    }
    
    public virtual void OnLeaveButtonClicked()
    {
        Debug.Log("Leave button clicked");
        endScreen.SetActive(false);
        totalTimeTaken = 0f;
        
        completionTimes = new List<float>();
        EndMiniGame();
    }
    
    protected virtual void UpdateTimer()
    {
        float timeElapsed = Time.time - startTime;
        timerText.text = "Time: " + timeElapsed.ToString("F2");
    }
    
    
}
 