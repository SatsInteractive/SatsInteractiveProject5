using System;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : Singleton<PointsManager>
{
    [SerializeField] private float points = 0;
    public float maxPoints = 180;
    public event Action<float> OnPointsUpdated;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void SetPoints(float points)
    {
        this.points = points;
        BroadcastPoints();
    }

    public void AddPoints(float points)
    {
        this.points += points;
        BroadcastPoints();
    }
    
    private void BroadcastPoints()
    {
        OnPointsUpdated?.Invoke(points);
    }
    
    public float GetPoints()
    {
        return points;
    }
}
