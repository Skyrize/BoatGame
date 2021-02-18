using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private UnityEvent onLevelStart = new UnityEvent();
    [SerializeField] private UnityEvent onWin = new UnityEvent();
    [SerializeField] private UnityEvent onLose = new UnityEvent();

    static private GameManager _instance = null;
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
    private void Awake() {
        instance = this;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
