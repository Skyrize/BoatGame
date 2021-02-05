using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidAgent : FlockAgent
{
    protected void Awake() {
        if (!agentCollider) {
            agentCollider = GetComponent<Collider>();
        }
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
    }

    public override void Move(Vector3 velocity)
    {
        if (velocity == Vector3.zero) {
            Debug.Log("Zero for agent " + gameObject.name);
            Debug.Break();
        }
        velocity.y = 0;
        transform.forward = velocity;

        // rb.AddForce(velocity, ForceMode.VelocityChange);
        // rb.MovePosition(transform.position + velocity * Time.deltaTime);
        transform.Translate(velocity * Time.deltaTime, Space.World);
        // transform.Translate(velocity * Time.deltaTime);
        // transform.position += velocity * Time.deltaTime;
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
