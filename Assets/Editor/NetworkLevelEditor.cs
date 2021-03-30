using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NetworkLevel), true)]
public class NetworkLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NetworkLevel myScript = (NetworkLevel)target;
        if(GUILayout.Button("Toggle Pause"))
        {
            myScript.TogglePause();
        }
    }
}