using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float magnitude = 0.06f;

    private bool done = true;
    
    public void Shake()
    {
        if (done)
            StartCoroutine("_Shake");
    }

    private IEnumerator _Shake()
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;
        done = false;

        while (elapsed < duration) {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            transform.localPosition = new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        done = true;
        transform.localPosition = originalPos;
    }

}