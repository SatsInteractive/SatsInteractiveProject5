using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{
    protected float startTime;
    [SerializeField] protected TextMeshProUGUI timerText;
    [SerializeField] protected AudioClip correctSound;
    [SerializeField] protected AudioClip wrongSound;
    [SerializeField] protected float audioLevel;
    [SerializeField] protected float colorFeedbackDelay = 0.5f;
    [SerializeField] protected float screenOpeningDelay = 5f;
    [SerializeField] protected List<float> completionTimes;
    [SerializeField] protected float totalTimeTaken;
    protected bool inputLocked = false;
    
    [Header("End Screen Settings")]
    [SerializeField] protected GameObject endScreen;
    [SerializeField] private TMP_Text totalTimeTakenText;
    
    protected virtual void Awake()
    {
        completionTimes = new List<float>();
        endScreen.SetActive(false);
    }

    public virtual void StartMiniGame()
    {
        
    }
    
    protected virtual void EndMiniGame()
    {
        gameObject.SetActive(false);
        MapManager.Instance.TeleportPlayerToSpawnPoint1();
        MapManager.Instance.ShowMap();
    }

    protected virtual void ShowEndScreen()
    {
        for (int i = 0; i < completionTimes.Count; i++)
        {
            totalTimeTaken += completionTimes[i];
        }
        endScreen.SetActive(true);
        totalTimeTakenText.text = totalTimeTaken.ToString("F2");
    }
    
    public virtual void OnLeaveButtonClicked()
    {
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
 