using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine.UIElements;

[CustomEditor(typeof(CreatureInfoSO))]
public class CustomCreatureInfoSO : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //GUILayout.BeginHorizontal();
        //GUILayout.Toggle(false, "Revealed");
        //GUILayout.Toggle(false, "Viewed");
        //GUILayout.TextArea("");
        //GUILayout.EndHorizontal();
    }


}
