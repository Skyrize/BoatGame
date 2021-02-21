using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoatUI : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] protected UnityEvent onSelect = new UnityEvent();
    [SerializeField] protected UnityEvent onUnselect = new UnityEvent();

    public void Select() {
        onSelect.Invoke();
    }
    public void Unselect() {
        onUnselect.Invoke();
    }
}
