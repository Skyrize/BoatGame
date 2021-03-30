using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartManager : MonoBehaviour
{
    public Transform playerOneStart = null;
    public Transform playerTwoStart = null;
    public Bounds boundaries = new Bounds();

    public static PlayerStartManager Instance = null;
    
    private void Awake() {
        if (Instance) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boundaries.center, boundaries.size);
    }
}
