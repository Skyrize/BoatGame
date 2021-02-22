using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Behavior
{
    public FlockBehavior behavior;
    public float weight;
}

[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : FlockBehavior
{
    [SerializeField] protected Behavior[] behaviors;
    public override Vector3 CalculateMove(in FlockAgent agent, in List<Transform> context)
    {
        Vector3 result = Vector3.zero;

            Debug.Log("---------------------agent move = " + agent.gameObject.name);
        foreach (Behavior item in behaviors)
        {
            Vector3 move = item.behavior.CalculateMove(agent, context) * item.weight;

            // Debug.Log("last move =" + move.ToString());
            //un peu de la merde ça (c'est un clamp un peu nul en fait)
            if (move.sqrMagnitude != 0 && move.sqrMagnitude > item.weight * item.weight) {
                move = move.normalized * item.weight;
            }
            // Debug.Log("---------------last move normalized =" + move.ToString());
            Debug.Log("Adding move = " + move.ToString());
            result += move;
        }
        Debug.Log("---------TOTAAAAL = " + result.ToString());

        return result;
    }
}
