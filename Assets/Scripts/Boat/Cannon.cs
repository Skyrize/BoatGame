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
    [SerializeField] protected float protectionLenght = 100f;
    [SerializeField] protected float protectionAngle = 2f;
    [SerializeField] protected bool debug = true;
    [SerializeField] protected float debugProtectionMultiplier = 1f;
    [Header("Events")]
    [SerializeField] protected UnityEvent onFire = new UnityEvent();
    [Header("References")]
    [SerializeField] protected GameObject cannonBallPrefab;
    [Header("Runtime")]
    [SerializeField] protected bool isLoaded = true;

    private Flock parentFlock;
    // Start is called before the first frame update
    void Start()
    {
        parentFlock = GetComponentInParent<Flock>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Reload() {
        isLoaded = true;
    }

    private bool _canFire(Vector3 point, Vector3 angleDirection) {
        RaycastHit hit;
        BoatAgent targetBoat;
        Vector3 origin = transform.position + point;
        Vector3 finalPoint = transform.position + point + angleDirection * protectionAngle + transform.forward * protectionLenght;
        Vector3 direction = finalPoint - origin;
        
        if (debug) {
            Debug.DrawLine(origin, finalPoint, Color.yellow, .5f);
        }
        if (Physics.Raycast(origin, direction, out hit, protectionLenght, mask)) {
            targetBoat = hit.transform.GetComponent<BoatAgent>();
            if (targetBoat && targetBoat.flock == parentFlock) {
                return false;
            }
        }
        return true;
    }

    public bool CanFire() {
        Vector3 pointCenter = Vector3.zero;
        Vector3 pointUp = transform.up * protectionRange;
        Vector3 pointDownLeft = (- transform.up - transform.right) * protectionRange;
        Vector3 pointDownRight = (- transform.up + transform.right) * protectionRange;

        return _canFire(pointCenter, pointCenter) && _canFire(pointUp, transform.up) && _canFire(pointDownLeft, -transform.right) && _canFire(pointDownRight, transform.right);
    }

    public void Fire() {
        if (!isLoaded || !CanFire())
            return;
        isLoaded = false;
        GameObject cannonBall = GameObject.Instantiate(cannonBallPrefab, transform.position, transform.rotation);

        cannonBall.GetComponent<Rigidbody>().AddForce(transform.forward * ejectionForce, ForceMode.Impulse);
        onFire.Invoke();
        Invoke("Reload", reloadTime);
    }

    private void OnDrawGizmos() {
        if (debug) {
            Vector3 pointUp = transform.up * protectionRange;
            Vector3 pointDownLeft = (- transform.up - transform.right) * protectionRange;
            Vector3 pointDownRight = (- transform.up + transform.right) * protectionRange;
            Vector3 lenght = transform.forward * protectionLenght * debugProtectionMultiplier;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + lenght);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + pointUp, transform.position + pointUp + transform.up * protectionAngle + lenght);
            Gizmos.DrawLine(transform.position + pointDownLeft, transform.position + pointDownLeft - transform.right * protectionAngle + lenght);
            Gizmos.DrawLine(transform.position + pointDownRight, transform.position + pointDownRight + transform.right * protectionAngle + lenght);
        }
    }
}
