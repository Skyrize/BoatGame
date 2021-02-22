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
        if (!isActiveAndEnabled)
            return;
        foreach (EventBind eventBind in events)
        {
            if (eventBind.name == eventName) {
                eventBind.evt.Invoke();
            }
        }
    }
    private IEnumerator _Delay(string eventName, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        foreach (EventBind eventBind in events)
        {
            if (eventBind.name == eventName) {
                eventBind.evt.Invoke();
            }
        }
    }

    public void DelayEvent(string eventName, float seconds) {
        StartCoroutine(_Delay(eventName, seconds));
    }
}
