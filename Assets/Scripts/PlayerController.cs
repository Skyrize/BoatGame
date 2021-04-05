using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class PlayerController : NetworkBehaviour
{
    [Header("Settings")]
    [SerializeField] protected LayerMask selectableMask = 1;
    [Header("Runtime")]
    NetworkVariable<FleetController> networkControlledFleet = new NetworkVariable<FleetController>(
        new NetworkVariableSettings 
            {
                WritePermission = NetworkVariablePermission.OwnerOnly,
                ReadPermission = NetworkVariablePermission.Everyone
            }, null);
    [SerializeField] protected FleetController controlledFleet = null; 
    FleetController ControlledFleet {
        get {
            return networkControlledFleet.Value;
        }
        set {
            networkControlledFleet.Value = value;
        }
    }
    [SerializeField]
    protected List<FleetController> fleets = new List<FleetController>();
    protected Camera cam;
    protected Plane seaLevel = new Plane(Vector3.up, Vector3.zero);

    GameManager gameManager;
    Team team;

    private void Start() {
        if (IsHost && IsLocalPlayer) {
            team = Team.PLAYER_1; // bof
        } else {
            team = Team.PLAYER_2; // et re bof
        }
        gameManager = FindObjectOfType<GameManager>();

        foreach (var fleet in gameManager.fleets)
        {
            if (fleet.GetComponentInParent<TeamManager>().team == team) {
                fleets.Add(fleet);
            }
            
        }
    }
    
    /* #region Controls */
    void ValueChanged(FleetController prevF, FleetController newF)
    {
        controlledFleet = newF;
    }

    public override void NetworkStart()
    {
        base.NetworkStart();

        networkControlledFleet.OnValueChanged += ValueChanged;
        if (IsLocalPlayer) {
            cam = GetComponent<Camera>();

        }
    }
    void FocusFleet()
    {
        if (!ControlledFleet)
            return;
        GetComponent<CameraController>().Focus(ControlledFleet.transform.GetChild(0).position);
    }

    void UnselectFleet()
    {
        if (!ControlledFleet)
            return;
        ControlledFleet.Unselect();
        ControlledFleet = null;
    }

    void SelectFleet(FleetController target)
    {
        if (ControlledFleet == target || target.GetComponentInParent<TeamManager>().team != gameManager.team)
            return;
        UnselectFleet();
        ControlledFleet = target;
        ControlledFleet.Select();
    }

    void SelectWithMouse() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        BoatAgent target = null;

        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 5f);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectableMask)) {
            target = hit.transform.GetComponent<BoatAgent>();

            if (target) {
                SelectFleet(target.flock.GetComponent<FleetController>());
                return;
            }
        }
        UnselectFleet();
    }

    
    [ServerRpc]
    void MoveFleetServerRpc(Vector3 target)
    {
        if (!ControlledFleet)
            return;
        ControlledFleet.SetDestination(target);
    }

    void MoveFleet() {
        if (!ControlledFleet)
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        float distance;
        seaLevel.Raycast(ray, out distance);

        MoveFleetServerRpc(ray.GetPoint(distance));
        // ControlledFleet.SetDestination();
    }

    [ClientRpc]
    void FireLeftClientRpc(int fleetIndex)
    {
        if (IsHost || fleetIndex == -1)
            return;
        FleetController fleet = fleets[fleetIndex];
        if (!fleet) {
            Debug.Log("Null fleet controller (destroyed ?)");
            return;
        }
        fleet.GetComponent<FleetController>().FireLeft();
    }
    [ServerRpc]
    void FireLeftServerRpc()
    {
        if (!ControlledFleet)
            return;
        ControlledFleet.FireLeft();
        FireLeftClientRpc(fleets.IndexOf(ControlledFleet));
    }

    void FireLeft()
    {
        if (!ControlledFleet)
            return;
        FireLeftServerRpc();
    }
    [ClientRpc]
    void FireRightClientRpc(int fleetIndex)
    {
        if (IsHost || fleetIndex == -1)
            return;
        FleetController fleet = fleets[fleetIndex];
        if (!fleet) {
            Debug.Log("Null fleet controller (destroyed ?)");
            return;
        }
        fleet.GetComponent<FleetController>().FireRight();
    }
    [ServerRpc]
    void FireRightServerRpc()
    {
        if (!ControlledFleet)
            return;
        ControlledFleet.FireRight();
        FireRightClientRpc(fleets.IndexOf(ControlledFleet));
    }

    void FireRight()
    {
        if (!ControlledFleet)
            return;
        FireRightServerRpc();
    }
/* #endregion */
    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer && cam) {
            if (Input.GetMouseButtonDown(0)) {
                SelectWithMouse();
            }
            if (Input.GetMouseButtonDown(1)) {
                MoveFleet();
            }
            if (Input.GetKey(KeyCode.Space)) {
                FocusFleet();
            }
            
            
            if (Input.GetKeyDown(CustomInputManager.Instance.keyShootLeft)) {
                FireLeft();
                
            }
            
            if (Input.GetKeyDown(CustomInputManager.Instance.keyShootRight)) {
                FireRight();
                
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                // TODO : proper disconnect menu ?
                SceneManager.Instance.BackToMenu();
            }

            for (int i = 0; i != fleets.Count; i++) {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i)) {
                    SelectFleet(fleets[i]);
                }
            }
        }
    }
}
