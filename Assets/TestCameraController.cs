using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float speed = 200f;
    [SerializeField] protected float scrollSpeed = 10000f;
    [SerializeField] protected float borderThicknessRatio = 10f;
    [SerializeField] protected Bounds boundaries = new Bounds(new Vector3(0, 200, 0), new Vector3(450, 200, 450));
    [SerializeField] protected bool debug = true;
    
    [Header("Runtime")]
    [SerializeField] protected Vector3 direction = Vector3.zero;
    protected Camera cam;
    protected Plane seaLevel = new Plane(Vector3.up, Vector3.zero);
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
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

        if (!debug) {
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

    public void Focus(Vector3 position)
    {
        if (!cam)
            cam = Camera.main;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        float distance;
        seaLevel.Raycast(ray, out distance);
        Vector3 move = position - ray.direction * distance;
        move.y = cam.transform.position.y;
        cam.transform.position = move;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.Instance.LoadScene("Menu");
    }

    private void OnDrawGizmos() {
        if (debug) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(boundaries.center, boundaries.size);
        }
    }
}
