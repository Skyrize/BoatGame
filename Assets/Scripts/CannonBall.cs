using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MLAPI;
using MLAPI.Messaging;

public class CannonBall : NetworkBehaviour
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

    [ClientRpc]
    void OnTouchWaterClientRpc()
    {
        if (IsHost)
            return;
        touchedWater = true;
        onTouchWater.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= 0 && !touchedWater && IsHost) {
            touchedWater = true;
            onTouchWater.Invoke();
            OnTouchWaterClientRpc();
        }
    }
}
