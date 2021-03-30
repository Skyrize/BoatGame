using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisgustingUI : MonoBehaviour
{
    TMPro.TMP_Text errorText;

    Color invisible = new Color(1, 1, 1, 0f);
    Color transparent = new Color(1, 1, 1, .8f);
    Image image;


    private void Start() {
        image = GetComponent<Image>();
        errorText = GetComponentInChildren<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (errorText.text == "") {
            image.color = invisible;
        } else {
            image.color = transparent;
        }
    }
}
