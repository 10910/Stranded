using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InfoUnitDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //EditorGUI.BeginProperty(position, label, property);

        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        //var indent = EditorGUI.indentLevel;
        //EditorGUI.indentLevel = 0;

        
        //var isRevealedRect = new Rect(position.x, position.y, 20, position.height);
        //var isViewedRect = new Rect(position.x + 30, position.y, 20, position.height);
        //var textRect = new Rect(position.x + 60, position.y, position.width - 60, position.height);
        //EditorGUI.Toggle(isRevealedRect, true);
        //// Draw fields - pass GUIContent.none to each so they are drawn without labels
        ////EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);
        ////EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("unit"), GUIContent.none);
        ////EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);

        //// Set indent back to what it was
        //EditorGUI.indentLevel = indent;

        //EditorGUI.EndProperty();
    }
}
