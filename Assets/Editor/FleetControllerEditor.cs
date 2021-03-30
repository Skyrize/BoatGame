using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FleetController), true)]
public class FleetControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FleetController myScript = (FleetController)target;
        if(GUILayout.Button("KILL"))
        {
            myScript.Kill();
        }
    }
}