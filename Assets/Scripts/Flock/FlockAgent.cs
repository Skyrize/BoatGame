using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class FlockAgent : MonoBehaviour
{
    [Header("Settings")]
    public bool debug = false;
    [Range(1f, 100f)]
    [SerializeField] protected float neighborRadius = 5f;
    [Range(0f, 1f)]
    [SerializeField] protected float avoidanceRadiusMultiplier = 0.5f;
    [Range(1f, 100f)]
    [SerializeField] protected float driveFactor = 10f;
    public float DriveFactor { get { return driveFactor; } }
    [Range(1f, 100f)]
    [SerializeField] protected float maxSpeed = 5f;
    public float MaxSpeed { get { return maxSpeed; } }

    //Computation optimisation
    protected float squareMaxSpeed;
    public float SquareMaxSpeed { get { return squareMaxSpeed; } }
    protected float squareNeighborRadius;
    public float SquareNeighborRadius { get { return squareNeighborRadius; } }
    protected float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    [Header("References")]
    [SerializeField] public Flock flock;
    [SerializeField] protected Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }


    // Start is called before the first frame update
    void Start()
    {
    }

    public abstract void Move(Vector3 velocity);

    public List<Transform> GetNearbyObstacles() 
    {
        Collider[] obstacles = Physics.OverlapSphere(transform.position, neighborRadius);
        List<Transform> result = new List<Transform>();

        foreach (Collider obstacle in obstacles)
        {
            if (obstacle != agentCollider) {
                result.Add(obstacle.transform);
            }
        }
        return result;
    }
}
