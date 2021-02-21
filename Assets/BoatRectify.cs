using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRectify : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected float threshold = 15f;
    [Header("Runtime")]
    [SerializeField] protected Vector3 currentVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Mathf.Abs(transform.EulerAngles.y) > threshold || Mathf.Abs(transform.localEulerAngles.z) > threshold)
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.eulerAngles.y, 0), Time.deltaTime * speed);
    }

}
