using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoatAgent : FlockAgent
{
    [Header("BoatSettings")]
    [SerializeField] protected float steerSpeed = 10f;
    [SerializeField] protected float maxSteerSpeed = 10f;
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float minAcceleration = 0.3f;

    [Header("BoatAgent")]
    [SerializeField] public Transform destination = null;
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
    }

    public override void Move(Vector3 newDirection)
    {
        if (!isActiveAndEnabled) {
            rb.velocity = Vector3.zero;
            return;
        }
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
        }
        this.direction.z = Mathf.Max(direction.z, minAcceleration);
    }
    
    public Vector3 GetInput() {
        if (destination == null)
            return transform.forward;
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
        Vector3 finalForce;

        accelerationForce.y = 0;
        rb.AddForce(accelerationForce);
        finalForce = transform.InverseTransformDirection(Vector3.ClampMagnitude(rb.velocity, maxSpeed));
        finalForce.z = Mathf.Max(finalForce.z, minAcceleration);
        // Debug.Log(gameObject.name + finalForce.ToString());
        rb.velocity = transform.TransformDirection(finalForce);
        if (debug) {
            Debug.DrawLine(transform.position, transform.position + finalForce * 10, Color.red, Time.fixedDeltaTime);
            Debug.DrawLine(transform.position, transform.position + rb.velocity * 10, Color.green, Time.fixedDeltaTime);
        }
    }

    void Steer() {
        targetSteerAngle = steerSpeed * steerInput;
        // currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, Time.deltaTime * maxSteerSpeed);
        Quaternion steerForce = Quaternion.Euler(rb.rotation.eulerAngles + transform.up * targetSteerAngle * Time.fixedDeltaTime);
        rb.MoveRotation(steerForce);
        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, maxSteerSpeed);
    }

    // protected virtual void FixedUpdate() {
    //     Move();
    //     Steer();
    // }
}
