using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using MLAPI.Transports.UNET;

public class NetworkLevel : NetworkBehaviour
{
    public GameObject WaitingScreen = null;
    public TMPro.TMP_Text ipText = null;
    public GameObject WaitingForJoin = null;
    public GameObject WaitingForReady = null;
    public GameObject WaitingForYou = null;

    public NetworkVariableBool paused = new NetworkVariableBool(
        new NetworkVariableSettings {
            WritePermission = NetworkVariablePermission.ServerOnly,
            ReadPermission = NetworkVariablePermission.Everyone,
        }, false
    );
    public NetworkVariableBool ready = new NetworkVariableBool(
        new NetworkVariableSettings {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone,
        }, false
    );

    public static NetworkLevel Instance = null;

    private void Awake() {
        if (Instance) {
            Destroy(this);
        } else {
            Instance = this;
        }
        paused.OnValueChanged += OnPause;
        ready.OnValueChanged += OnReady;
    }

    void OnPause(bool prevValue, bool newValue)
    {
        Time.timeScale = newValue ? 0 : 1;
    }

    void OnReady(bool prevValue, bool newValue)
    {
        if (IsHost && newValue)
            StartGameServerRpc();
    }

    [ServerRpc]
    public void TogglePauseServerRpc()
    {
        if (paused.Value) {
            Unpause();
        } else {
            Pause();
        }
    }

    public void TogglePause()
    {
        if (!IsHost)
            return;
        TogglePauseServerRpc();
    }

    public void Pause()
    {
        paused.Value = true;
    }

    public void Unpause()
    {
        paused.Value = false;
    }

    public void OnHosted()
    {
        Pause();
        WaitingScreen.SetActive(true);
        WaitingForJoin.SetActive(true);

        WaitingForReady.SetActive(false);
        WaitingForYou.SetActive(false);
        
        // ipText.text = $"IP : {NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress}";
        ipText.text = $"IP : {IPManager.GetIP(ADDRESSFAM.IPv4)}";
    }

    public void OnJoined()
    {
        WaitingScreen.SetActive(true);
        WaitingForYou.SetActive(true);

        WaitingForReady.SetActive(false);
        WaitingForJoin.SetActive(false);
    }

    public void OnPlayerJoined()
    {
        WaitingScreen.SetActive(true);
        WaitingForReady.SetActive(true);

        WaitingForJoin.SetActive(false);
        WaitingForYou.SetActive(false);
    }

    public void OnPlayerLeft()
    {
        ready.Value = false;
        OnHosted();
    }

    [ClientRpc]
    void StartGameClientRpc()
    {
        if (IsHost)
            return;
        
        WaitingScreen.SetActive(false);
        WaitingForReady.SetActive(false);

        WaitingForJoin.SetActive(false);
        WaitingForYou.SetActive(false);
    }

    [ServerRpc]
    void StartGameServerRpc()
    {
        WaitingScreen.SetActive(false);
        WaitingForReady.SetActive(false);

        WaitingForJoin.SetActive(false);
        WaitingForYou.SetActive(false);

        StartGameClientRpc();
        Unpause();
    }

    public void OnPlayerReady()
    {
        ready.Value = true;
    }

}
