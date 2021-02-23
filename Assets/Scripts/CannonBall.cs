using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CannonBall : MonoBehaviour
{
    [SerializeField] protected UnityEvent onTouchWater = new UnityEvent();
    bool touchedWater = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Stop()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = false;
        touchedWater = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= 0 && !touchedWater) {
            touchedWater = true;
            onTouchWater.Invoke();
        }
    }
}
