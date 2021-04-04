using System.Collections;
using UnityEngine;
using Manager = UnityEngine.SceneManagement.SceneManager;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance = null;
    
    private void Awake() {
        if (Instance) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    public void ReloadCurrentScene()
    {
        Manager.LoadScene(Manager.GetActiveScene().name);
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = Manager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
    public void LoadScene(string sceneName)
    {
        Manager.LoadScene(sceneName);
    }

    public void LoadSceneAsyncro(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void Exit()
    {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
    }
    
    void OnSoloSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        NetworkHandler.Instance.StartHost();
        Manager.sceneLoaded -= OnSoloSceneLoaded;
        GameManager.Instance.Init(PlayMode.SOLO);
        NetworkLevel.Instance.Unpause();
    }

    void OnHostSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        NetworkHandler.Instance.StartHost();
        Manager.sceneLoaded -= OnHostSceneLoaded;
        GameManager.Instance.Init(PlayMode.MULTIPLAYER);
        NetworkLevel.Instance.OnHosted();
    }

    public void StartSoloLevel(string level)
    {
        Manager.sceneLoaded += OnSoloSceneLoaded;
        LoadScene(level);
    }

    public void StartHostLevel(string level)
    {
        Manager.sceneLoaded += OnHostSceneLoaded;
        LoadScene(level);
    }

    public void Join()
    {
        NetworkHandler.Instance.StartClient();
    }

    public void StartJoinLevel()
    {
        GameManager.Instance.Init(PlayMode.MULTIPLAYER);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        if (Manager.GetActiveScene().name == "Menu") {
            return;
        }
        NetworkHandler.Instance.Disconnect();
        LoadScene("Menu");
    }
}
