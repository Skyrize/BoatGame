using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MLAPI;

public class NetworkEvent : NetworkBehaviour
{
    [Header("Events")]
    public UnityEvent IsPlayerStart = new UnityEvent();
    public UnityEvent IsNotPlayerStart = new UnityEvent();

    public override void NetworkStart()
    {
        base.NetworkStart();
        if (IsLocalPlayer) {
            IsPlayerStart.Invoke();
        } else {
            IsNotPlayerStart.Invoke();
        }
    }
}
