using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

[System.Serializable]
public class DamageEvent : UnityEvent<float>
{
    public bool HasPersistentListener(UnityAction<float> call)
    {
        for (int i = 0; i != this.GetPersistentEventCount(); i++) {
            if (this.GetPersistentMethodName(i) == call.Method.Name)
                return true;
        }
        return false;
    }
}
[System.Serializable]
public class HealEvent : UnityEvent<float>
{
    public bool HasPersistentListener(UnityAction<float> call)
    {
        for (int i = 0; i != this.GetPersistentEventCount(); i++) {
            if (this.GetPersistentMethodName(i) == call.Method.Name)
                return true;
        }
        return false;
    }
}
[System.Serializable]
public class DeathEvent : UnityEvent
{
    public bool HasPersistentListener(UnityAction call)
    {
        for (int i = 0; i != this.GetPersistentEventCount(); i++) {
            if (this.GetPersistentMethodName(i) == call.Method.Name)
                return true;
        }
        return false;
    }
}
[System.Serializable]
public class LifeEvent : UnityEvent<float>
{
    public bool HasPersistentListener(UnityAction<float> call)
    {
        for (int i = 0; i != this.GetPersistentEventCount(); i++) {
            if (this.GetPersistentMethodName(i) == call.Method.Name)
                return true;
        }
        return false;
    }
}

public class HealthComponent : NetworkBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    private NetworkVariableFloat actualHealth = new NetworkVariableFloat(
        new NetworkVariableSettings {
            WritePermission = NetworkVariablePermission.ServerOnly,
            ReadPermission = NetworkVariablePermission.Everyone
        }, 100f
    );
    [SerializeField]
    private float maxHealth = 100f;

    [Header("Events")]
    public HealEvent onHealEvent = new HealEvent();
    public DamageEvent onDamageEvent = new DamageEvent();
    public DeathEvent onDeathEvent = new DeathEvent();
    public LifeEvent onLifeUpdate = new LifeEvent();

    private float ActualHealth {
        get {
            return actualHealth.Value;
        }
        set {
            if (IsHost) {
                actualHealth.Value = Mathf.Clamp(value, 0, maxHealth);
                onLifeUpdate.Invoke(HealthRatio);
                OnLifeUpdateClientRpc();
            }
        }
    }

    [ClientRpc]
    void OnLifeUpdateClientRpc()
    {
        if (IsHost)
            return;
        onLifeUpdate.Invoke(HealthRatio);
    }


    public float HealthRatio => actualHealth.Value / maxHealth;

    public float Health {
        get {
            return actualHealth.Value;
        }
    }

    public float MaxHealth {
        get {
            return maxHealth;
        }
    }

    public bool IsAlive {
        get {
            return actualHealth.Value > 0;
        }
    }
    public bool IsDead {
        get {
            return !IsAlive;
        }
    }

    public override void NetworkStart()
    {
        base.NetworkStart();
        
    }

    [ClientRpc]
    void OnDamageEventClientRpc(float amount)
    {
        if (IsHost)
            return;
        onDamageEvent.Invoke(amount);
    }
    [ClientRpc]
    void OnDeathEventClientRpc()
    {
        if (IsHost)
            return;
        onDeathEvent.Invoke();
    }
    public void ReduceHealth(float amount)
    {
        if (IsDead || !IsHost)
            return;
        ActualHealth -= amount;
        onDamageEvent.Invoke(amount);
        OnDamageEventClientRpc(amount);
        if (IsDead) {
            onDeathEvent.Invoke();
            OnDeathEventClientRpc();
        }
    }
    
    [ClientRpc]
    void OnHealEventClientRpc(float amount)
    {
        if (IsHost)
            return;
        onHealEvent.Invoke(amount);
    }
    public void IncreaseHealth(float amount)
    {
        if (IsDead || !IsHost)
            return;
        ActualHealth += amount;
        onHealEvent.Invoke(amount);
        OnHealEventClientRpc(amount);
    }
    [ClientRpc]
    void KillClientRpc()
    {
        if (IsHost)
            return;
        Destroy(this.gameObject);
    }

    public void Kill() {
        if (!IsHost)
            return;
        KillClientRpc();
        Destroy(this.gameObject);
    }

}
