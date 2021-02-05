using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    public Camera cam;
    public Transform[] viewPositions;
    
    public int viewIndex = 0;

    void SetView(int index)
    {
        viewIndex = index;
        cam.transform.position = viewPositions[viewIndex].position;
        cam.transform.rotation = viewPositions[viewIndex].rotation;
    }

    private void Start() {
        SetView(viewIndex);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            SetView(0);
        } else if (Input.GetKeyDown(KeyCode.Z)) {
            SetView(1);
        } else if (Input.GetKeyDown(KeyCode.E)) {
            SetView(2);
        } else if (Input.GetKeyDown(KeyCode.R)) {
            SetView(3);
        }

    }
    
}
