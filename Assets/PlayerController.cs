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

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();

        foreach (var fleet in gameManager.fleets)
        {
            if (fleet.GetComponentInParent<TeamManager>().team == gameManager.team) {
                fleets.Add(fleet);
            }
            
        }
    }

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
        if (ControlledFleet) {
            ControlledFleet.Unselect();
            ControlledFleet = null;
        }
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
    void FireLeftClientRpc()
    {
        if (IsHost)
            return;
        Debug.Log("client FireLeft");
        ControlledFleet.FireLeft();
    }
    [ServerRpc]
    void FireLeftServerRpc()
    {
        Debug.Log("server FireLeft");
        ControlledFleet.FireLeft();
        FireLeftClientRpc();
    }

    void FireLeft()
    {
        FireLeftServerRpc();
    }
    [ClientRpc]
    void FireRightClientRpc()
    {
        if (IsHost)
            return;
        Debug.Log("client FireRight");
        ControlledFleet.FireRight();
    }
    [ServerRpc]
    void FireRightServerRpc()
    {
        Debug.Log("server FireRight");
        ControlledFleet.FireRight();
        FireRightClientRpc();
    }

    void FireRight()
    {
        FireRightServerRpc();
    }


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
            
            
            if (Input.GetKeyDown(KeyCode.A) && ControlledFleet) {
                FireLeft();
                
            }
            
            if (Input.GetKeyDown(KeyCode.E) && ControlledFleet) {
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
