using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Behavior
{
    public FlockBehavior behavior;
    public float weight;
    public float max;
}

[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : FlockBehavior
{
    public bool useMax = true;
    [SerializeField] protected Behavior[] behaviors;
    public override Vector3 CalculateMove(in FlockAgent agent, in List<Transform> context)
    {
        Vector3 result = Vector3.zero;

        foreach (Behavior item in behaviors)
        {
            Vector3 move = item.behavior.CalculateMove(agent, context) * item.weight;

            if (useMax) {
                if (move.sqrMagnitude != 0 && move.sqrMagnitude > item.max * item.max) {
                    move = move.normalized * item.max;
                }
            } else {
                if (move.sqrMagnitude != 0 && move.sqrMagnitude > item.weight * item.weight) {
                    move = move.normalized * item.weight;
                }
            }
            if (item.behavior.name == "Avoidance Behavior") {
                
                Debug.DrawRay(move, Vector3.up * 10, Color.green, Time.deltaTime);
            }
            result += move;
        }

        return result;
    }
}
