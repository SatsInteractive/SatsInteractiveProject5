using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    protected virtual void Awake()
    {
        var objs = FindObjectsOfType(typeof(T)) as T[];
        if (objs.Length > 1) Destroy(gameObject);
    }

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                var objs = FindObjectsOfType(typeof(T)) as T[];

                if (objs.Length > 0) instance = objs[0];

                if (objs.Length > 1)
                {
                    Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                }

                if (instance == null)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = string.Format("_{0}", typeof(T).Name);
                    instance = gameObject.AddComponent<T>();
                }
            }

            return instance;
        }
    }
}
