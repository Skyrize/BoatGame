using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine.Events;
using System;
using MLAPI.Transports;

[System.Serializable]
public enum PlayMode
{
    SOLO,
    MULTIPLAYER
}
public class NetworkHandler : NetworkBehaviour
{
    public List<ulong> clients = new List<ulong>();
    public int maxPlayer = 2;
    public static NetworkHandler Instance = null;
    bool connected = false;
    public UnityEvent onConnectionFailed = new UnityEvent();

    private void Awake() {
        if (Instance) {
            Destroy(this);
        } else {
            Instance = this;
        }
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        NetworkManager.Singleton.ConnectionApprovalCallback += OnClientConnectApproval;
    }

    void OnClientConnectApproval(byte[] connectionData, ulong clientId, MLAPI.NetworkManager.ConnectionApprovedDelegate callback)
    {
        bool approve = true;
        bool createPlayerObject = true;

        if (clients.Count == maxPlayer - 1)
            approve = false;
        callback(createPlayerObject, null, approve, null, null);
    }

    void OnClientConnected(ulong clientId)
    {
        if (!IsHost) {
            connected = true;
            SceneManager.Instance.StartJoinLevel();
            NetworkLevel.Instance.OnJoined();
        } else {
            if (clients.Contains(clientId))
                Debug.LogError($"Network Error : two clients with the same id {clientId}.");
            clients.Add(clientId);
            NetworkLevel.Instance.OnPlayerJoined();
        }
        Debug.Log($"Client connected {clientId}");
    }
    
    void OnClientDisconnected(ulong clientId)
    {
        if (!IsHost) {
            if (connected == false) {
                onConnectionFailed.Invoke();
            } else {
                connected = false;
                SceneManager.Instance.BackToMenu();
            }
        } else {
            if (!clients.Contains(clientId)) {
                Debug.LogError($"Network Error : tried to disconnect client with id {clientId} but could not find it.");
                return;
            }
            clients.Remove(clientId);
            NetworkLevel.Instance.OnPlayerLeft();
        }
        Debug.Log($"Client disconnected {clientId}");
    }
    
    public void StartHost()
    {
        connected = true;
        
        NetworkManager.Singleton.StartHost();
        var unet = NetworkManager.Singleton.GetComponent<UNetTransport>();
    }
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void Disconnect()
    {
        if (NetworkManager.Singleton.IsHost) {
            NetworkManager.Singleton.StopHost();
            connected = false;
        } else if (IsClient) {
            NetworkManager.Singleton.StopClient();
            connected = false;
        }
    }

}
