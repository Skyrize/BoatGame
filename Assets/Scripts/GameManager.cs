using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MLAPI;

public class GameManager : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject fleetsAI = null;
    [SerializeField] protected GameObject fleetsPlayer2 = null;
    [Header("Runtime")]
    public FleetController[] fleets;
    public Team team = Team.PLAYER_1;
    [SerializeField] protected int remainingPlayerFleets = -1;
    [SerializeField] protected int remainingEnemyFleets = -1;
    PlayMode playMode = PlayMode.SOLO;
    [Header("Events")]
    [SerializeField] protected UnityEvent onWin = new UnityEvent();
    [SerializeField] protected UnityEvent onLose = new UnityEvent();

    public static GameManager Instance = null;
    
    private void Awake() {
        if (Instance) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    public void DisableMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void FreezeTime()
    {
        Time.timeScale = 0;
    }

    public void EnableMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

	public void Quit () 
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

    public void UnfreezeTime()
    {
        Time.timeScale = 1;
    }

    public void Win()
    {
        onWin.Invoke();
    }
    public void Lose()
    {
        onLose.Invoke();
    }

    void DecreasePlayerFleet(FleetController target)
    {
        remainingPlayerFleets--;
        if (remainingPlayerFleets == 0) {
            onLose.Invoke();
        }
    }

    void DecreaseEnemyFleet(FleetController target)
    {
        remainingEnemyFleets--;
        if (remainingEnemyFleets == 0) {
            onWin.Invoke();
        }  
    }

    void SetupFleets()
    {
        fleets = GameObject.FindObjectsOfType<FleetController>();

        remainingPlayerFleets = 0;
        remainingEnemyFleets = 0;
        foreach (var fleet in fleets)
        {
            if (fleet.GetComponentInParent<TeamManager>().team == team) {
                fleet.onFleetDestroyed.AddListener(DecreasePlayerFleet);
                remainingPlayerFleets++;
            } else {
                fleet.onFleetDestroyed.AddListener(DecreaseEnemyFleet);
                remainingEnemyFleets++;
            }
        }
    }

    public void Init(PlayMode mode)
    {
        this.playMode = mode;
        switch (mode) {
            case PlayMode.SOLO:
            fleetsPlayer2.SetActive(false);
            break;
            case PlayMode.MULTIPLAYER:
            fleetsAI.SetActive(false);
            break;
        }
        if (IsHost) {
            team = Team.PLAYER_1; // bof
        } else {
            team = Team.PLAYER_2; // et re bof
        }
        SetupFleets();
    }

}
