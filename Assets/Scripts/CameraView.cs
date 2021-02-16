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
        if (Input.GetKeyDown(KeyCode.U)) {
            SetView(0);
        } else if (Input.GetKeyDown(KeyCode.I)) {
            SetView(1);
        } else if (Input.GetKeyDown(KeyCode.O)) {
            SetView(2);
        } else if (Input.GetKeyDown(KeyCode.P)) {
            SetView(3);
        }

    }
    
}
