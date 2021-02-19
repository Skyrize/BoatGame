using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float speed = 20f;
    [SerializeField] protected float scrollSpeed = 200f;
    [SerializeField] protected float borderThicknessRatio = 10f;
    [SerializeField] protected Bounds boundaries = new Bounds();
    [SerializeField] protected bool debug = true;
    
    [Header("Runtime")]
    [SerializeField] protected Vector3 direction = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Move()
    {
        float yPixels = Screen.height / borderThicknessRatio;
        float xPixels = Screen.width / borderThicknessRatio;
        float yMin = yPixels;
        float xMin = xPixels;
        float yMax = Screen.height - yPixels;
        float xMax = Screen.width - yPixels;

        direction.x = Input.GetAxisRaw("Horizontal");
        direction.z = Input.GetAxisRaw("Vertical");

        if (Input.mousePosition.x <= xMin) {
            direction.x = -1;
        } else if (Input.mousePosition.x >= xMax) {
            direction.x = 1;
        }
        if (Input.mousePosition.y <= yMin) {
            direction.z = -1;
        } else if (Input.mousePosition.y >= yMax) {
            direction.z = 1;
        }


        direction.Normalize();
        direction.x *= speed;
        direction.z *= speed;
        direction.y = -Input.GetAxisRaw("Mouse ScrollWheel") * scrollSpeed;

        transform.Translate(direction * Time.deltaTime, Space.World);
        Vector3 finalPos = transform.position;
        finalPos.x = Mathf.Clamp(finalPos.x, boundaries.min.x, boundaries.max.x);
        finalPos.y = Mathf.Clamp(finalPos.y, boundaries.min.y, boundaries.max.y);
        finalPos.z = Mathf.Clamp(finalPos.z, boundaries.min.z, boundaries.max.z);
        transform.position = finalPos;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        // if (Input.GetMouseButtonDown(1)) {
        //     Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     // mousePos.z = transform.position.z;
        //     this.transform.position = mousePos;
        // }
        // Camera.main.orthographicSize -= Input.mouseScrollDelta.y * speed * Time.deltaTime;
    }

    private void OnDrawGizmos() {
        if (debug) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(boundaries.center, boundaries.size);
        }
    }
}
