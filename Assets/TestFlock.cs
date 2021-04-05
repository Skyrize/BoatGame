using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFlock : Flock
{
    public bool fillAtStart = true;
    public int nbAgentGenerate = 500;

    void Start() {
        if (fillAtStart) Populate();
        
        foreach (FlockAgent agent in agents)
        {
            agent.flock = this;
        }
    }

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

    public override void Generate()
    {
        var boundaries = FindObjectOfType<CameraController>().Boundaries;

        for (int i = 0; i != nbAgentGenerate;)
        {
            //TODO Generate
        }
    }

    private void Update() {
        UpdateAgents();
    }
}
