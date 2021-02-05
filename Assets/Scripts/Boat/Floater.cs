using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float depthBeforeSubmerged = 1f;
    [SerializeField] protected float displacementAmount = 3f;
    [SerializeField] protected int floaterCount = 1;
    [SerializeField] protected float waterDrag = 0.99f;
    [SerializeField] protected float waterAngularDrag = 0.5f;
    public   Color debugColor = Color.yellow;

    [Header("References")]
    [SerializeField] protected Rigidbody rb;
    
    private void FixedUpdate() {
        float waveHeight = WaveManager.Instance.GetWaveHeight(transform.position.x);

        rb.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
        if (transform.position.y < waveHeight) {
            debugColor = Color.yellow;
            float displacementMultiplier = (waveHeight - transform.position.y) / depthBeforeSubmerged * displacementAmount;
            rb.drag = displacementMultiplier * waterDrag;
            rb.angularDrag = displacementMultiplier * waterAngularDrag;
            // rb.AddForce(displacementMultiplier * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.Acceleration);
            // rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.Acceleration);
            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
        } else {
            rb.drag = 0;
            rb.angularDrag = 0;
            debugColor = Color.red;
        }
        Debug.DrawLine(transform.position, transform.position + rb.velocity, debugColor, Time.fixedDeltaTime);
    }
}
