using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onTimerReached = new UnityEvent();

    private IEnumerator _Delay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        onTimerReached.Invoke();
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }
    
    public void Delay(float delayInSeconds)
    {
        this.StartCoroutine(_Delay(delayInSeconds));
    }
}
