using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAgent : CarController
{
    [Header("Agent")]
    [SerializeField] protected  Transform destination = null;
    [SerializeField] protected float minAcceleration = 0.3f;
    [SerializeField] protected  int areaMask = NavMesh.AllAreas;
    [Header("Debug")]
    [SerializeField] protected  bool debug = false;
    [SerializeField] protected  bool stop = false;
    [SerializeField] protected  float precision = 0.3f;
    [Header("Runtime")]
    [SerializeField] protected  bool hasPath = false;
    [SerializeField] protected  Vector3 direction = Vector3.zero;
    [SerializeField] protected  Vector3 target = Vector3.zero;
    [SerializeField] protected  Vector3[] corners;

    [SerializeField] float speedBegin = 0;
    [SerializeField] float timeTotal = 0;
    [SerializeField] float speedLostPerSec = 0;

    protected  NavMeshPath path;

    override protected void Start() {
        base.Start();
        path = new NavMeshPath();
    }

    virtual protected void GetDirection()
    {
        corners = path.corners;

        this.direction = transform.InverseTransformPoint(path.corners[1]);
        this.direction.y = 0;
        target = direction;
        this.direction.Normalize();

        this.steerInput = direction.x;
        // this.accelerationInput = direction.z;
        this.accelerationInput = Mathf.Max(direction.z, minAcceleration);

        // var t = transform.InverseTransformDirection(rb.velocity);
        // var dot = Vector3.Dot(t, direction);
        // Debug.DrawLine(transform.position, transform.position + rb.velocity, Color.yellow, precision);
        // Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(transform.InverseTransformPoint(path.corners[2])), Color.green, precision);

        // Debug.Log(t.ToString());
        // Debug.Log(dot.ToString());
        Debug.Log(rb.velocity.magnitude);

        if (isBraking) {
            if (speedBegin == 0) {
                speedBegin = rb.velocity.magnitude;
            }
            timeTotal += Time.deltaTime;
            if (rb.velocity.magnitude <= 0.001f) {
                isBraking = false;
                speedLostPerSec = speedBegin / timeTotal;
            }
        }

        // var stoppingDistance = transform.position + rb.velocity * 1 + 0.5f * 

    }

    override protected void GetInput() {
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

}
