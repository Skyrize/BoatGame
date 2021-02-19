using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagesOnTouch : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float damageAmount = 1;
    [Header("References")]
    [SerializeField] private Collider hitBox = null;

    public void ApplyDamages(GameObject target) {
        HealthComponent targetHealth = target.GetComponentInParent<HealthComponent>();

        if (targetHealth != null) {
            targetHealth.ReduceHealth(damageAmount);
        }
    }

    private void OnCollisionEnter(Collision other) {
        ContactPoint contactPoint = other.GetContact(0);

        if (contactPoint.thisCollider == hitBox) {
            ApplyDamages(other.gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        ApplyDamages(other.gameObject);
    }

    private void Awake() {
        if (!hitBox)
            hitBox = GetComponentInChildren<Collider>();
    }
}
