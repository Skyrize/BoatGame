using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected LayerMask selectableMask = 1;
    [Header("Runtime")]
    [SerializeField] protected FleetController controlledFleet = null;
    [Header("Events")]
    [SerializeField] protected UnityEvent onLevelStart = new UnityEvent();
    [SerializeField] protected UnityEvent onWin = new UnityEvent();
    [SerializeField] protected UnityEvent onLose = new UnityEvent();

    static protected GameManager _instance = null;
    static public GameManager instance {
        get {
            if (_instance == null)
                Debug.LogException(new System.Exception("Asking for instance too early (awake)"));
            return GameManager._instance;
        }

        set {
            if (_instance) {
                Debug.LogException(new System.Exception("More thand one GameManager in the Scene"));
            } else {
                _instance = value;
            }
        }
    }

    protected Camera cam;
    protected Plane seeLevel = new Plane(Vector3.forward, Vector3.zero);

    protected void Awake() {
        instance = this;
        cam = Camera.main;
        // DontDestroyOnLoad(this.gameObject);
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

    // Start is called before the first frame update
    void Start()
    {
        onLevelStart.Invoke();
    }

    void UnselectFleet()
    {
        if (controlledFleet) {
            controlledFleet.Unselect();
            controlledFleet = null;
        }
    }

    void SelectFleet(FleetController target)
    {
        if (controlledFleet == target)
            return;
        UnselectFleet();
        controlledFleet = target;
        controlledFleet.Select();
    }

    void SelectWithMouse() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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

    void MoveFleet() {
        if (!controlledFleet)
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        seeLevel.Raycast(ray, out distance);
        controlledFleet.SetDestination(ray.GetPoint(distance));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            SelectWithMouse();
        }
        if (Input.GetMouseButtonDown(0)) {
            MoveFleet();
        }
    }
}
