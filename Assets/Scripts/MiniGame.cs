using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGame : MonoBehaviour
{
    protected float startTime;
    [SerializeField] protected TextMeshProUGUI timerText;
    [SerializeField] protected AudioClip correctSound;
    [SerializeField] protected AudioClip wrongSound;
    [SerializeField] protected float audioLevel;
    [SerializeField] protected float colorFeedbackDelay = 0.5f;
    [SerializeField] protected float screenOpeningDelay = 5f;
    
    public virtual void StartMiniGame()
    {
        
    }
    
    public virtual void EndMiniGame()
    {
        gameObject.SetActive(false);
        MapManager.Instance.TeleportPlayerToSpawnPoint1();
        MapManager.Instance.ShowMap();
    }
    
    protected virtual void UpdateTimer()
    {
        float timeElapsed = Time.time - startTime;
        timerText.text = "Time: " + timeElapsed.ToString("F2");
    }
}
 