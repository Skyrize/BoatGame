using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform attacker;
    public Transform target;
    public Transform dest;
    public float attackRange = 10;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void ChaseTarget()
    {
        float dot = Mathf.Abs(Vector3.Dot(target.forward, attacker.forward));
        Vector3 targetForwardRelative = target.forward;
        Vector3 attackerForwardRelative = attacker.forward;
        Vector3 leftPoint = target.position + (- target.right + Vector3.Lerp(targetForwardRelative, attackerForwardRelative, dot)) * attackRange;
        Vector3 rightPoint = target.position + (target.right + Vector3.Lerp(targetForwardRelative, attackerForwardRelative, dot)) * attackRange;

        if (attacker.InverseTransformPoint(target.position).x > 0) {
            dest.position = leftPoint;
        } else {
            dest.position = rightPoint;
        }
    }


    private void Update() {
        ChaseTarget();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if (attacker)
        Gizmos.DrawWireSphere(attacker.position, attackRange);
    }
}
