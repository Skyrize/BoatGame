using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Flock), true)]
public class FlockEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Flock myScript = (Flock)target;
        if(GUILayout.Button("Populate"))
        {
            myScript.Populate();
        }
        if(GUILayout.Button("Generate"))
        {
            myScript.Generate();
        }
    }
}