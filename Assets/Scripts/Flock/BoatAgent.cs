using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoatAgent : FlockAgent
{
    [Header("BoatSettings")]
    [SerializeField] protected float maxSteerAngle = 10f;
    [SerializeField] protected float maxSteerSpeed = 10f;
    [SerializeField] protected float speed = 5f;

    [Header("BoatAgent")]
    [SerializeField] protected Transform destination = null;
    [SerializeField] protected float minAcceleration = 0.3f;
    [SerializeField] protected int areaMask = NavMesh.AllAreas;

    [Header("References")]
    protected NavMeshPath path;

    [Header("Runtime")]
    [SerializeField] protected float targetSteerAngle = 5f;
    [SerializeField] protected float currentSteerAngle = 0f;
    [SerializeField] protected float steerInput = 0;
    [SerializeField] protected float accelerationInput = 0;
    [SerializeField] protected Vector3 direction = Vector3.zero;
    [SerializeField] protected Vector3 test = Vector3.zero;
    
    [Header("Debug")]
    [SerializeField] protected bool stop = false;
    [SerializeField] protected float precision = 0.3f;

    override protected void Awake() {
        base.Awake();
        path = new NavMeshPath();
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
    }

    public override void Move(Vector3 newDirection)
    {
        newDirection = transform.InverseTransformDirection(newDirection);
        newDirection.y = 0;
        Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(newDirection) * 10, Color.black, Time.fixedDeltaTime);
        accelerationInput = newDirection.z;
        steerInput = newDirection.x;
        Move();
        Steer();
    }

    virtual protected void GetDirection()
    {   
        this.direction = transform.InverseTransformPoint(path.corners[1]);
        this.direction.y = 0;
        this.direction.Normalize();

        //maybe all in direction then renormalize ? 
        var dot = Vector3.Dot(this.direction, Vector3.forward);
        if (dot < 0) {
            this.direction.x = 1f * Mathf.Sign(direction.x);
            // Debug.Log("backward " + dot.ToString() + this.direction.ToString() + Vector3.forward.ToString());
        }
        // if (dot < -0.7f) {
        //     if (dot == -1) {
        //         this.steerInput = 1f;
        //         Debug.Log("backward");
        //     } else {
        //         this.steerInput = Mathf.Sign(this.steerInput) * minSteerInput;
        //     }
        //     // if (Random.Range(0, 2) == 0) {
        //     // } else {
        //         // this.steerInput = -0.3f;
        //     // }
        // }
        this.direction.z = Mathf.Max(direction.z, minAcceleration);
    }
    
    public Vector3 GetInput() {
        if (destination == null)
            return Vector3.forward;
        if (NavMesh.CalculatePath(transform.position, destination.position, areaMask, path)) {

            GetDirection();

            if (debug) {
                // Debug.Log(this.input.ToString());
                for (int i = 0; i < path.corners.Length - 1; i++) {
                    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, precision);
                }
                if (stop) {
                    this.accelerationInput = 0;
                }
            }
        } else {
            if (debug)
                Debug.Log("No path found");
        }
        return transform.TransformDirection(this.direction); // normalize or not ?
    }

    void Move() {
        Vector3 accelerationForce = transform.forward * accelerationInput * speed;

        accelerationForce.y = 0;
        rb.AddForce(accelerationForce);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    void Steer() {
        targetSteerAngle = maxSteerAngle * steerInput;
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, Time.deltaTime * maxSteerSpeed);
        Quaternion steerForce = Quaternion.Euler(rb.rotation.eulerAngles + transform.up * currentSteerAngle * Time.fixedDeltaTime);
        rb.MoveRotation(steerForce);
    }

    // protected virtual void FixedUpdate() {
    //     Move();
    //     Steer();
    // }
    private void OnDrawGizmos() {
        if (!debug)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, neighborRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, neighborRadius * avoidanceRadiusMultiplier);
    }
}
