using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputUI : MonoBehaviour
{
    public TMPro.TMP_Text text;

    private void Start() {
        UpdateType();
    }

    public void ToggleType()
    {
        CustomInputManager.Instance.ToggleType();
        UpdateType();
    }
    
    public void UpdateType()
    {
        text.text = CustomInputManager.Instance.type.ToString();
    }
}
