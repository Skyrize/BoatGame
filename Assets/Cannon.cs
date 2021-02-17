using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float reloadTime = 1f;
    [SerializeField] protected float ejectionForce = 100f;
    [Header("References")]
    [SerializeField] protected GameObject cannonBallPrefab;
    [Header("Runtime")]
    [SerializeField] protected bool isLoaded = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Reload() {
        isLoaded = true;
    }

    public void Fire() {
        if (!isLoaded)
            return;
        isLoaded = false;
        GameObject cannonBall = GameObject.Instantiate(cannonBallPrefab, transform.position, transform.rotation);

        cannonBall.GetComponent<Rigidbody>().AddForce(transform.forward * ejectionForce, ForceMode.Impulse);
        Invoke("Reload", reloadTime);
    }
}
