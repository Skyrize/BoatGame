using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamesManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    protected GameObject[] flames;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void UpdateWithLife(float ratio) {
        int index = 0;
        for (float i = 1; i > ratio; i -= 0.1f) {
            flames[index].SetActive(true);
            index++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
