using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float speed = 2;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 1 * speed * Time.deltaTime, 0) * transform.rotation; 
    }
}
