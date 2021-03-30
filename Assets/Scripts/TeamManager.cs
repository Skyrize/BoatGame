using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    PLAYER_1,
    PLAYER_2,
    AI
}

public class TeamManager : MonoBehaviour
{
    public Team team = Team.PLAYER_1;

    private void Start() {
        foreach (BoatAgent boat in transform.GetComponentsInChildren<BoatAgent>())
        {
            boat.Team = team;
        }    
    }
}
