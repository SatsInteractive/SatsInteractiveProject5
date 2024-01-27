using UnityEngine;
using UnityEngine.EventSystems;

public class EnsureEventSystem : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }
}

