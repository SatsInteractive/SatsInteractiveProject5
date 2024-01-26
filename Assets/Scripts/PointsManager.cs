using System;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    [SerializeField] private int points = 0;
    public event Action<int> OnPointsUpdated;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SetPoints(int points)
    {
        this.points = points;
        BroadcastPoints();
    }

    public void AddPoints(int points)
    {
        this.points += points;
        BroadcastPoints();
    }
    
    private void BroadcastPoints()
    {
        OnPointsUpdated?.Invoke(points);
    }
    
    public int GetPoints()
    {
        return points;
    }
}
