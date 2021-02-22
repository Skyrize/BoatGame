using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cannon : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float reloadTime = 1f;
    [SerializeField] protected float ejectionForce = 100f;
    [SerializeField] protected LayerMask mask = 0;
    [SerializeField] protected float protectionRange = 0.2f;
    [SerializeField] protected float protectionLength = 100f;
    [SerializeField] protected float protectionAngle = 2f;
    [SerializeField] protected int nbProtectionRays = 3;
    [SerializeField] protected bool debug = true;
    [SerializeField] protected float debugProtectionMultiplier = 1f;
    [Header("Events")]
    [SerializeField] protected UnityEvent onFire = new UnityEvent();
    [Header("References")]
    [SerializeField] protected GameObject cannonBallPrefab;
    [Header("Runtime")]
    [SerializeField] protected bool isLoaded = true;

    private GameObject cannonBall = null;

    private Flock parentFlock;
    // Start is called before the first frame update
    void Start()
    {
        parentFlock = GetComponentInParent<Flock>();
        cannonBall = GameObject.Instantiate(cannonBallPrefab, transform.position, transform.rotation, transform);
        
        cannonBall.GetComponent<EventBinder>().CallEvent("Disable");
        cannonBall.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Reload() {
        isLoaded = true;
    }

    private bool _canFire(Vector3 point, Vector3 endPoint) {
        bool canFire = true;
        RaycastHit hit;
        BoatAgent targetBoat;
        Vector3 origin = transform.position + point;
        Vector3 direction = endPoint - origin;
        
        if (Physics.Raycast(origin, direction, out hit, protectionLength, mask)) {
            targetBoat = hit.transform.GetComponent<BoatAgent>();
            if (targetBoat && targetBoat.flock == parentFlock) {
                canFire = false;
            }
        }
        if (debug) {
            Debug.DrawLine(origin, endPoint, canFire ? Color.yellow : Color.blue, 1.5f);
            // Debug.Break();
        }

        return canFire;
    }

    public bool CanFire() {
        Vector3 length = transform.forward * protectionLength;

        if (!_canFire(transform.position, transform.position + length)) {
            return false;
        }
        for (int i = 0; i != nbProtectionRays; i++) {
            float angleMult = (float)(i + 1) / (float)nbProtectionRays;
            Vector3 pointLeft = (- transform.up - transform.right) * protectionRange * angleMult;
            Vector3 pointRight = (- transform.up + transform.right) * protectionRange * angleMult;
            Vector3 endPointLeft = transform.position + pointLeft - transform.right * protectionAngle * angleMult + length;
            Vector3 endPointRight = transform.position + pointRight + transform.right * protectionAngle * angleMult + length;
            
            if (!_canFire(pointLeft, endPointLeft)
            || !_canFire(pointRight, endPointRight))
                return false;
        }

        return (true);
    }

    public void Fire() {
        if (!isLoaded)
            return;
        if (!CanFire())
            return;
        if (cannonBall.GetComponent<CannonBall>().enabled == false) {
            isLoaded = false;
            cannonBall.SetActive(true);
            cannonBall.GetComponent<EventBinder>().CallEvent("Reset");
            cannonBall.transform.position = transform.position;
            cannonBall.transform.rotation = transform.rotation;
            cannonBall.GetComponent<Rigidbody>().AddForce(transform.forward * ejectionForce, ForceMode.Impulse);
            onFire.Invoke();
            Invoke("Reload", reloadTime);
        }
    }

    private void OnDrawGizmos() {
        if (debug) {

            Vector3 length = transform.forward * protectionLength * debugProtectionMultiplier;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + length);
            
            Gizmos.color = Color.red;
            for (int i = 0; i != nbProtectionRays; i++) {
                float angleMult = (float)(i + 1) / (float)nbProtectionRays;
                Vector3 pointLeft = (- transform.up - transform.right) * protectionRange * angleMult;
                Vector3 pointRight = (- transform.up + transform.right) * protectionRange * angleMult;
                
                Gizmos.DrawLine(transform.position + pointLeft, transform.position + pointLeft - transform.right * protectionAngle * angleMult + length);
                Gizmos.DrawLine(transform.position + pointRight, transform.position + pointRight + transform.right * protectionAngle * angleMult + length);
            }
        }
    }
}
