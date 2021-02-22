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

    public void FireLeft()
    {
        if (!isActiveAndEnabled)
            return;
        foreach (Cannon item in leftCannons)
        {
            item.Fire();
        }
    }
    public void FireRight()
    {
        if (!isActiveAndEnabled)
            return;
        foreach (Cannon item in rightCannons)
        {
            item.Fire();
        }
    }
}
