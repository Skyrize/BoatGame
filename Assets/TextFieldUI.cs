using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TextFieldUI : MonoBehaviour
{
    public UnityEvent onValidate = new UnityEvent();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
            Debug.Log("enter");
            onValidate.Invoke();
        }
    }
}
