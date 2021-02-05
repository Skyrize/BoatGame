using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float amplitude = 1f;
    [Range(0.001f, 1000)]
    [SerializeField] protected float length = 2f;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected float offset = 2f;
    

    private void Update() {
        offset += Time.deltaTime * speed;
    }

    public float GetWaveHeight(float x)
    {
        return amplitude * Mathf.Sin(x / length + offset);
    }

    private static WaveManager instance = null;
    public static WaveManager Instance {
        get {
            if (!instance) {
                Debug.LogException(new MissingReferenceException("WaveManager not initialized. Be careful to ask for instance AFTER Awake method."));
            }
            return instance;
        }
        set {
            if (instance != null)
                Debug.LogException(new System.Exception("More than one WaveManager in the scene. Please remove the excess"));
            instance = value;
        }
    }
            
    private void Awake() {
        Instance = this;
    }
}
