using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButtonUI : MonoBehaviour
{
    public Color baseColor = Color.white;
    public Color hoverColor = new Color(.8f, .8f, .8f, 1);
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        baseColor = image.color;
    }

    public void OnHovered()
    {
        image.color = hoverColor;
    }

    public void OnUnhovered()
    {
        image.color = baseColor;
    }

}
