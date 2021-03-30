using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum FireSide
{
    NONE,
    LEFT,
    RIGHT,
    BOTH
}

public class AIFleetController : FleetController
{
    [Header("Settings")]
    [SerializeField] protected float arrivalDist = 35f;
    public float ArrivalDist { get { return arrivalDist; } }
    [SerializeField] protected float detectionRange = 200f, attackRange = 100f, chaseRatio = .33f;
    [SerializeField] protected int fireRatio = 2;
    [SerializeField] protected bool debug = false;

    [Header("References")]
    [SerializeField]
    protected List<FleetController> playerFleets = new List<FleetController>();
    [Header("Runtime")]
    [SerializeField]
    protected FleetController currentTarget = null;
    [SerializeField]
    public Transform CurrentWaypoint = null;
    // Start is called before the first frame update
    override protected void Start()
    {
        var boatAgents = GetComponentsInChildren<BoatAgent>();

        remainingBoats = boatAgents.Length;
        foreach (BoatAgent agent in boatAgents)
        {
            agent.destination = fleetTarget;
            agent.GetComponent<HealthComponent>().onDeathEvent.AddListener(this.OnBoatDestroyed);
        }
        
        var allFleets = GameObject.FindObjectsOfType<FleetController>();

        foreach (var fleet in allFleets)
        {
            if (fleet.GetComponentInParent<TeamManager>().team == Team.PLAYER_1) {
                playerFleets.Add(fleet);
            }
        }
        fleetTarget.GetComponent<EventBinder>().CallEvent("Unselect");
    }

    public void UpdateWaypoint()
    {
        if (Vector3.Distance(Position, CurrentWaypoint.position) < arrivalDist) {
            CurrentWaypoint = WaypointManager.Instance.GetRandomWaypoint();
        }

        SetDestination(CurrentWaypoint.position);
    }

    public bool DetectTarget()
    {
        bool result = false;
        
        currentTarget = null;
        foreach (FleetController fleet in playerFleets)
        {
            if (fleet.RemainingBoats != 0) {
                if (!currentTarget) {
                    if (Vector3.Distance(fleet.Position, Position) <= detectionRange) {
                        result = true;
                        currentTarget = fleet;
                    }
                } else {
                    if (Vector3.Distance(fleet.Position, Position) < Vector3.Distance(currentTarget.Position, Position)) {
                        result = true;
                        currentTarget = fleet;
                    }

                }
            }
        }
        return result;
    }

    public void ChaseTarget()
    {
        Transform target = currentTarget.transform.GetChild(0);
        Transform captain = transform.GetChild(0);
        float dot = Mathf.Abs(Vector3.Dot(target.forward, captain.forward));
        Vector3 leftPoint = target.position + (- target.right + Vector3.Lerp(target.forward, captain.forward, dot)) * attackRange * chaseRatio;
        Vector3 rightPoint = target.position + (target.right + Vector3.Lerp(target.forward, captain.forward, dot)) * attackRange * chaseRatio;

        if (captain.InverseTransformPoint(target.position).x > 0) {
            SetDestination(leftPoint);
        } else {
            SetDestination(rightPoint);
        }
    }

    public bool IsInAttackRange()
    {
        return Vector3.Distance(currentTarget.Position, Position) <= attackRange;
    }

    public bool ValidateTarget()
    {
        if (currentTarget.RemainingBoats == 0) {
            currentTarget = null;
            return false;
        }
        return true;
    }

    public FireSide ShouldFire(Transform boat)
    {
        RaycastHit hit;
        FireSide result = FireSide.NONE;
        Vector3 leftDirection = - boat.right * attackRange;
        Vector3 rightDirection = boat.right * attackRange;

        if (Physics.Raycast(boat.position, leftDirection, out hit, attackRange)) {
            BoatAgent target = hit.transform.gameObject.GetComponent<BoatAgent>();
            if (target && target.Team == Team.PLAYER_1) {
                result = FireSide.LEFT;
            }
            if (debug)
                Debug.DrawLine(boat.position, hit.point, Color.green, Time.deltaTime);
        } else {
            if (debug)
                Debug.DrawRay(boat.position, leftDirection * attackRange, Color.red, Time.deltaTime);
        }
        if (Physics.Raycast(boat.position, rightDirection, out hit, attackRange)) {
            BoatAgent target = hit.transform.gameObject.GetComponent<BoatAgent>();
            if (target && target.Team == Team.PLAYER_1) {
                if (result == FireSide.NONE) {
                    result = FireSide.RIGHT;
                } else {
                    result = FireSide.BOTH;
                }
            }
            if (debug)
                Debug.DrawLine(boat.position, hit.point, Color.green, Time.deltaTime);
        } else {
            if (debug)
                Debug.DrawRay(boat.position, rightDirection * attackRange, Color.red, Time.deltaTime);
        }
        return result;
    }

    public FireSide ShouldFire()
    {
        FireSide result = FireSide.NONE;
        int nbBoat = transform.childCount - 1;
        int nbLeft = 0;
        int nbRight = 0;
        Transform currentBoat;
        int minShotSelf = (int)Mathf.Max(Mathf.Ceil((float)nbBoat / (float)fireRatio), 1);
        int minShotTarget = (int)Mathf.Max(Mathf.Ceil((float)currentTarget.RemainingBoats / (float)fireRatio), 1);
        int minShot = Mathf.Min(minShotSelf, minShotTarget);

        for (int i = 0; i != nbBoat; i++) {
            currentBoat = transform.GetChild(i);
            FireSide boatFireSide = ShouldFire(currentBoat);
            
            switch (boatFireSide)
            {
                case FireSide.NONE:
                break;
                case FireSide.LEFT:
                nbLeft++;
                break;
                case FireSide.RIGHT:
                nbRight++;
                break;
                case FireSide.BOTH:
                nbLeft++;
                nbRight++;
                break;
            }
        }
        if (nbLeft >= minShot) {
            result = FireSide.LEFT;
        }
        if (nbRight >= minShot) {
            if (result == FireSide.NONE) {
                result = FireSide.RIGHT;
            } else {
                result = FireSide.BOTH;
            }
        }
        return result;
    }

    public void Fire(FireSide side)
    {
        switch (side)
        {
            case FireSide.RIGHT:
            FireRight();
            break;
            case FireSide.LEFT:
            FireLeft();
            break;
            case FireSide.BOTH:
            FireRight();
            FireLeft();
            break;
        }
    }


    // Update is called once per frame
    void Update()
    {
    }

    private void OnDrawGizmos() {
        if (!debug)
            return;
        Transform boat = transform.GetChild(0);
        if (boat) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(boat.position, arrivalDist);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(boat.position, detectionRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(boat.position, attackRange);
        }
    }
}

/*

            // if (fleet.RemainingBoats != 0) { // closest
            //     if (!target || Vector3.Distance(transform.GetChild(0).position, fleet.transform.GetChild(0).position) < Vector3.Distance(transform.GetChild(0).position, target.transform.GetChild(0).position)) {
            //         target = fleet;
            //     } 
            // }



Vector3 randomPoint = GetRandomPointAround(minAngle, maxAngle);
            randomPoint = target.InverseTransformDirection(randomPoint);
            for (float i = minAngle; i != maxAngle; i++) {
                float a = Mathf.Deg2Rad * i;
                Vector3 b = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a)).normalized;
                randomPoint = target.InverseTransformDirection(b);
                Debug.DrawLine(target.position + randomPoint * 30, target.position + randomPoint * 30 + Vector3.up * 100, Color.green, Time.deltaTime);
            }
            for (float i = minAngle; i != maxAngle; i++) {
                float a = Mathf.Deg2Rad * (i + 180);
                Vector3 b = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a)).normalized;
                randomPoint = target.InverseTransformDirection(b);
                Debug.DrawLine(target.position + randomPoint * 30, target.position + randomPoint * 30 + Vector3.up * 100, Color.green, Time.deltaTime);
            }
            Debug.DrawLine(target.position + randomPoint * 30, target.position + randomPoint * 30 + Vector3.up * 100, Color.green, Time.deltaTime);
            Debug.Break();




            
    public Vector3 GetRandomPointAround(float minAngle, float maxAngle) // TO BREAK (inside seek dest): choosing randomly around target (vs choosing gradually)
    {
        float angle = Random.Range(Mathf.Deg2Rad * minAngle, Mathf.Deg2Rad * maxAngle);


        return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized;
    }

    public Vector3 GetPointAround(Transform target)
    {
        Vector3 point = target.position;
        NavMeshHit rightHit;
        NavMeshHit leftHit;
        Vector3 leftPoint = target.position - target.right * maxDist;
        Vector3 rightPoint = target.position + target.right * maxDist;
        // Vector3 leftPoint = GetRandomPointAround(minAngle, maxAngle);
        // Vector3 rightPoint = GetRandomPointAround(minAngle + 180, maxAngle + 180);

        // leftPoint = target.InverseTransformDirection(leftPoint) * maxDist;
        // rightPoint = target.InverseTransformDirection(rightPoint) * maxDist;
        
        

        if (NavMesh.SamplePosition(leftPoint, out leftHit, fleetSize, NavMesh.AllAreas)) {
            Debug.DrawLine(leftHit.position, leftHit.position + Vector3.up * 100, Color.green, 10f);
        } else {
            Debug.Log("Left is bad");
        }
        if (NavMesh.SamplePosition(rightPoint, out rightHit, fleetSize, NavMesh.AllAreas)) {
            Debug.DrawLine(rightHit.position, rightHit.position + Vector3.up * 100, Color.blue, 10f);
            // ??
        } else {
            Debug.Log("Right is bad");
        }

        if (rightHit.distance > leftHit.distance) { // more space on left than right
            point = leftHit.position;
        } else if (rightHit.distance > leftHit.distance) { // more space on right than left
            point = rightHit.position;
        } else { // equal space
            point = Random.Range(0, 2) == 0 ? leftHit.position : rightHit.position;
        }


        // if (NavMesh.FindClosestEdge(target.position, out hit, NavMesh.AllAreas)) {
        // }
        return point;
    }

    private Vector3 relativeDest = Vector3.zero;

    public bool IsDestinationValid(FleetController currentTarget)
    {
        if (relativeDest == Vector3.zero)
            return false;
        NavMeshHit hit;
        Vector3 position = currentTarget.transform.GetChild(0).position + relativeDest;

        if (NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas)) {
            return true;
        }

        return false;
    }

    public Vector3 SeekDestination(FleetController target) // TO BREAK (own): choosing first child closest random point;
    {
        Transform destination = target.transform.GetChild(0); //Just that = direct hit
        Vector3 point = GetPointAround(destination);
        
        relativeDest = point - destination.position;
        return point;
    }


    // Update is called once per frame
    void Update()
    {
        // if (IsDestinationValid(currentTarget)) {
        //     Transform destination = currentTarget.transform.GetChild(0); //Just that = direct hit
        //     SetDestination(destination.position + relativeDest);
        // } else {
        //     Vector3 currentDest = SeekDestination(currentTarget);
        //     SetDestination(currentDest);
        // }
    }
*/
