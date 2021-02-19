using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EventBind {
    public string name;
    public UnityEvent evt = new UnityEvent();
}

public class EventBinder : MonoBehaviour
{
    [SerializeField]
    protected EventBind[] events;

    public void CallEvent(string eventName) {
        foreach (EventBind eventBind in events)
        {
            if (eventBind.name == eventName) {
                eventBind.evt.Invoke();
            }
        }
    }
}
