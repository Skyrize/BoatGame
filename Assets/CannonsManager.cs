using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    protected Cannon[] leftCannons;
    [SerializeField]
    protected Cannon[] rightCannons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            foreach (Cannon item in leftCannons)
            {
                item.Fire();
            }
        }
    }
}
