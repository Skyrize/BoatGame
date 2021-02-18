using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoatController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float maxSteerAngle = 10f;
    [SerializeField] protected float maxSteerSpeed = 10f;
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float maxSpeed = 10f;

    [Header("Agent")]
    [SerializeField] protected  Transform destination = null;
    [SerializeField] protected float minAcceleration = 0.3f;
    [SerializeField] protected  int areaMask = NavMesh.AllAreas;

    [Header("References")]
    [SerializeField] protected Rigidbody rb = null;
    protected  NavMeshPath path;

    [Header("Runtime")]
    [SerializeField] protected float targetSteerAngle = 5f;
    [SerializeField] protected float currentSteerAngle = 0f;
    [SerializeField] protected float steerInput = 0;
    [SerializeField] protected float accelerationInput = 0;
    [SerializeField] protected  Vector3 direction = Vector3.zero;
    
    [Header("Debug")]
    [SerializeField] protected  bool debug = false;
    [SerializeField] protected  bool stop = false;
    [SerializeField] protected  float precision = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        path = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    virtual protected void GetDirection()
    {
        this.direction = transform.InverseTransformPoint(path.corners[1]);
        this.direction.y = 0;
        this.direction.Normalize();

        this.steerInput = direction.x;
        var dot = Vector3.Dot(this.direction, Vector3.forward);
        if (dot < 0) {
            //raycast in direction of steerInput, if can turn then turn, if not then check opposite side, if still not then forward
            this.steerInput = 1f * Mathf.Sign(steerInput);
            Debug.Log("backward");
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
        this.accelerationInput = Mathf.Max(direction.z, minAcceleration);
    }
    
    protected void GetInput() {
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
            Debug.Log("No path found");
        }
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

    protected virtual void FixedUpdate() {
        Move();
        Steer();
    }

}
