using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidFlock : Flock
{

    override public void UpdateAgent(FlockAgent agent)
    {
        List<Transform> context = agent.GetNearbyObstacles();
        Vector3 move = behavior.CalculateMove(agent, context);
        move *= agent.DriveFactor;
        if (move.sqrMagnitude > agent.SquareMaxSpeed) {
            move = move.normalized * agent.MaxSpeed;
        }
        agent.Move(move);
    }
    
    // Update is called once per frame
    protected virtual void Update()
    {
        if (Time.timeScale == 0) return;
        UpdateAgents();
    }
}
