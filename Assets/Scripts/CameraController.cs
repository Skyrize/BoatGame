using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // mousePos.z = transform.position.z;
            this.transform.position = mousePos;
        }
        Camera.main.orthographicSize -= Input.mouseScrollDelta.y * speed * Time.deltaTime;
    }
}
