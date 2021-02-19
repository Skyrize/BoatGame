using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Billboard : MonoBehaviour
{
    Camera cam = null;
    // Start is called before the first frame update
    void Start()
    {
        if (!cam)
            cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam) {
            transform.LookAt(cam.transform.position, -Vector3.up);
        }
    }
}
