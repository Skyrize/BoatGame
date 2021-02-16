using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/PathFollow")]
public class PathFollowBehavior : FilteredFlockBehavior
{
    Vector3 currentVelocity;
    [SerializeField] protected float agentSmoothTime = 0.5f;

    public override Vector3 CalculateMove(in FlockAgent agent, in List<Transform> context)
    {
        BoatAgent boatAgent = agent as BoatAgent;

        return boatAgent.GetInput();
    }
}