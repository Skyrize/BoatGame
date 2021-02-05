using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttentionPointController : MonoBehaviour
{
    public Transform[] targets = new Transform[4];

    List<NavMeshAgent>[] agentGroups = new List<NavMeshAgent>[4];
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        var agents = GameObject.FindObjectsOfType<NavMeshAgent>();
        for (int i = 0; i != agentGroups.Length; i++) {
            agentGroups[i] = new List<NavMeshAgent>();
        }

        foreach (var agent in agents)
        {
            agentGroups[agent.GetComponent<AgentBehavior>().targetIndex].Add(agent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            index = 0;
        } else if (Input.GetKeyDown(KeyCode.Z)) {
            index = 1;
        } else if (Input.GetKeyDown(KeyCode.E)) {
            index = 2;
        } else if (Input.GetKeyDown(KeyCode.R)) {
            index = 3;
        }

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                targets[index].position = hit.point;
                foreach (var agent in agentGroups[index])
                {
                    agent.SetDestination(targets[index].position);
                }
            }
        }
    }
}
