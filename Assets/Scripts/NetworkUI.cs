using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;

public class NetworkUI : MonoBehaviour
{
    public TMPro.TMP_InputField ipField;
    public TMPro.TMP_Text errorText;

    private void Start() {
        NetworkHandler.Instance.onConnectionFailed.AddListener(OnConnectionFailed);
    }

    void OnConnectionFailed()
    {
        errorText.text = "Connection fail or timeout (is IP correct ?)";
    }

    void Connect(string ip)
    {

        errorText.text = $"Connecting to {(ip == "127.0.0.1" ? "localhost" : ip)} ..";
        NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = ip;
        // NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = "184.72.104.138";
        // NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectPort = 8888;
        NetworkHandler.Instance.StartClient();
    }

    public void CancelJoin()
    {
        NetworkHandler.Instance.Disconnect();
    }
    
    public void Join()
    {
        NetworkHandler.Instance.Disconnect();
        if (ipField.text.Length == 0) {
                Connect("127.0.0.1");
        } else {
            if (!Regex.Match(ipField.text, @"\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.|$)){4}\b").Success) {
                errorText.text = "Invalid IP Address";
            } else {
                Connect(ipField.text);
            }
        }
    }
}
