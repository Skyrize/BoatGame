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
    
    public void Join()
    {
        if (!Regex.Match(ipField.text, @"\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.|$)){4}\b").Success) {
            errorText.text = "Invalid IP Address";
        } else {
            errorText.text = "Connecting ..";
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = ipField.text;
            SceneManager.Instance.Join();
        }
    }
}
