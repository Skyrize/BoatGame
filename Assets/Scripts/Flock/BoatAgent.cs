using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoatAgent : FlockAgent
{
    [SerializeField] protected float steerSpeed = 10f;
    [SerializeField] protected float maxSteerSpeed = 10f;
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float minAcceleration = 0.3f;
    [SerializeField] protected float compensationSpeed = 0.3f;
    [SerializeField] protected Team team = Team.PLAYER_1;
    [SerializeField] public Team Team { get { return team; } set { team = value; } }

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
    [SerializeField] protected Vector3 finalForce = Vector3.zero;
    
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
        if (debug) {
            Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(newDirection) * 10, Color.black, Time.fixedDeltaTime);
        }
        accelerationInput = newDirection.z;
        steerInput = newDirection.x;
        Move();
        Steer();
        Float();
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
        if (destination == null) {
            if (debug) Debug.Log($"{gameObject.name} no destination");
            return transform.forward;
        }
        if (NavMesh.CalculatePath(transform.position, destination.position, areaMask, path)) {

            GetDirection();

            if (debug) {
                for (int i = 0; i < path.corners.Length - 1; i++) {
                    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, precision);
                }
                if (stop) this.accelerationInput = 0;
            }
        } else if (debug) Debug.Log("No path found");

        return transform.TransformDirection(this.direction); // normalize or not ?
    }

    void Move() {
        Vector3 accelerationForce = transform.forward * accelerationInput * speed;

        rb.AddForce(accelerationForce);
        finalForce = transform.InverseTransformDirection(Vector3.ClampMagnitude(rb.velocity, maxSpeed));
        finalForce.z = Mathf.Max(finalForce.z, minAcceleration);
        // Debug.Log(gameObject.name + finalForce.ToString());
        rb.velocity = transform.TransformDirection(finalForce);
        if (debug) {
            Debug.DrawLine(transform.position, transform.position + rb.velocity * 10, Color.green, Time.fixedDeltaTime);
        }
    }

    public Vector3 debugSteer = Vector3.zero;

    void Steer() {
        targetSteerAngle = steerSpeed * steerInput;
        Quaternion steerForce = Quaternion.Euler(transform.up * targetSteerAngle * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * steerForce);
        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, maxSteerSpeed);
    }

    void Float()
    {
        rb.centerOfMass = transform.position;
        Vector3 rectify = transform.position;
        rectify.y = 0;
        transform.position = rectify;
    }
}
