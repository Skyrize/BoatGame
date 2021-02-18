using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltitudeKill : MonoBehaviour
{
    public float yMin = -20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < yMin) {
            Kill();
        }
    }
}
