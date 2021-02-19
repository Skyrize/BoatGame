using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CollisionEvent : UnityEvent<GameObject>
{
}

public class OnTouchEvent : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool useTag = false;
    [SerializeField]
    private List<string> tags = null;

    [Header("Events")]
    [SerializeField] private CollisionEvent onTouch = new CollisionEvent();

    private void OnCollisionEnter(Collision other) {
        Debug.Log("collide");
        if (useTag) {
            if (tags.Contains(other.gameObject.tag)) {
                onTouch.Invoke(other.gameObject);
            }
        } else {
            onTouch.Invoke(other.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log("trigger");
        if (useTag) {
            if (tags.Contains(other.gameObject.tag)) {
                onTouch.Invoke(other.gameObject);
            }
        } else {
            onTouch.Invoke(other.gameObject);
        }
    }
}
