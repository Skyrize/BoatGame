using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;

public class TeamUI : NetworkBehaviour
{
    public Color colorRight = Color.blue;
    public Color colorLeft = Color.red;
    Team team;
    public Image circle;
    public GameObject healthBar = null;
    // Start is called before the first frame update
    void Start()
    {
        team = GetComponentInParent<TeamManager>().team;
        switch (team)
        {
            case Team.PLAYER_1:
            circle.color = colorRight;
            if (!IsHost)
                healthBar.SetActive(false);
            break;
            default:
            circle.color = colorLeft;
            if (IsHost)
                healthBar.SetActive(false);
            break;
        }
    }

    public void Selected()
    {
        circle.color = Color.white;

    }

    public void Unselected()
    {
        switch (team)
        {
            case Team.PLAYER_1:
            circle.color = colorRight;
            break;
            default:
            circle.color = colorLeft;
            break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
