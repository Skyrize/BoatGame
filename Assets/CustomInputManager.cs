using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum KeyboardType
{
    QWERTY = 0,
    AZERTY = 1
}

public class CustomInputManager : MonoBehaviour
{
    public KeyCode keyLeft;
    public KeyCode keyRight;
    public KeyCode keyUp;
    public KeyCode keyDown;
    
    public KeyCode keyShootLeft;
    public KeyCode keyShootRight;
    public KeyboardType type;

    public static CustomInputManager Instance = null;
    
    private void Awake() {
        if (Instance) {
            Destroy(this);
        } else {
            Instance = this;
        }
        type = (KeyboardType)PlayerPrefs.GetInt("key", 0);
        SetInput();
    }

    void SetInput()
    {
        PlayerPrefs.SetInt("key", (int)type);
        switch (type)
        {
            case KeyboardType.QWERTY:
            keyLeft = KeyCode.A;
            keyRight = KeyCode.D;
            keyUp = KeyCode.W;
            keyDown = KeyCode.S;
            keyShootLeft = KeyCode.Q;
            keyShootRight = KeyCode.E;
            break;
            case KeyboardType.AZERTY:
            keyLeft = KeyCode.Q;
            keyRight = KeyCode.D;
            keyUp = KeyCode.Z;
            keyDown = KeyCode.S;
            keyShootLeft = KeyCode.A;
            keyShootRight = KeyCode.E;
            break;
        }
    }

    public void ToggleType()
    {
        switch (type)
        {
            case KeyboardType.QWERTY:
            type = KeyboardType.AZERTY;
            break;
            case KeyboardType.AZERTY:
            type = KeyboardType.QWERTY;
            break;
        }
        SetInput();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
