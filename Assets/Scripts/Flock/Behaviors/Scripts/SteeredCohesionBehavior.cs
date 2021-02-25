using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/SteeredCohesion")]
public class SteeredCohesionBehavior : FilteredFlockBehavior
{
    Vector3 currentVelocity;
    [SerializeField] protected float agentSmoothTime = 0.5f;

    public override Vector3 CalculateMove(in FlockAgent agent, in List<Transform> context)
    {
        List<Transform> newContext = filter != null ? filter.filter(agent, context) : context;
        // Debug.Log("Filtered Steered = " + newContext.Count);
        if (newContext.Count == 0) {
            return Vector3.zero;
        }
        Vector3 steeredCohesionMove = Vector3.zero;

        foreach (Transform item in newContext)
        {
            steeredCohesionMove += item.position;
        }
        // Debug.Log("Filtered Steered1 = " + steeredCohesionMove.ToString());
        steeredCohesionMove /= newContext.Count;
        // Debug.Log("Filtered Steered2 = " + steeredCohesionMove.ToString());

        steeredCohesionMove -= agent.transform.position;
        // Debug.Log("Filtered Steered3 = " + Time.deltaTime);
        
        Vector3 smoothed = Vector3.SmoothDamp(agent.transform.forward, steeredCohesionMove, ref currentVelocity, agentSmoothTime);
        if (float.IsNaN(smoothed.x) || float.IsNaN(smoothed.y) || float.IsNaN(smoothed.z)) {
            return Vector3.zero;
        }
        return smoothed;
    }
}