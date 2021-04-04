using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidAgent : FlockAgent
{
    public float speed = 1;
    public float rotateSpeed = 5;
    Vector3 currentVelocity;
    [SerializeField] protected float smoothTime = 0.5f;
    override protected void Awake() {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
    }

    public override void Move(Vector3 target)
    {
        target.y = 0;
        if (target == Vector3.zero) {
            target = transform.position + transform.forward;
        }
        Quaternion targetRotation = Quaternion.LookRotation(target);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation , Time.deltaTime * rotateSpeed);
        // Quaternion nextRotation = Quaternion.Lerp(transform.rotation, )
        // Vector3 smoothed = Vector3.SmoothDamp(transform.forward, transform.position + target, ref currentVelocity, smoothTime);
        // if (float.IsNaN(smoothed.x) || float.IsNaN(smoothed.y) || float.IsNaN(smoothed.z)) {
        //     smoothed = target;
        // }
        // transform.forward = target;

        if (debug) Debug.DrawRay(transform.position + target, Vector3.up * 10, Color.blue, Time.deltaTime);
        // rb.AddForce(target, ForceMode.VelocityChange);
        // rb.MovePosition(transform.position + target * Time.deltaTime);
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        // transform.Translate(target * Time.deltaTime);
        // transform.position += target * Time.deltaTime;
    }

    private void OnDrawGizmos() {
        if (!debug)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, neighborRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, neighborRadius * avoidanceRadiusMultiplier);
    }
}
