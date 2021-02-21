using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinAnimation : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float amplitude = 1f;
    [Range(0.001f, 1000)]
    [SerializeField] protected float length = 2f;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected float offset = 2f;
    [Header("References")]
    [SerializeField] protected MeshFilter meshFilter;

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
    }
    public float GetWaveHeight(float x)
    {
        return amplitude * Mathf.Sin(x / length + offset);
    }

    // Update is called once per frame
    void Update()
    {
        offset += Time.deltaTime * speed;
        Vector3[] vertices = meshFilter.mesh.vertices;

        for (int i = 0; i != vertices.Length; i++)
        {
            if (vertices[i].x == 0)
                continue;
            vertices[i].x = GetWaveHeight(transform.position.z + vertices[i].z);
        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();
        
    }
}
