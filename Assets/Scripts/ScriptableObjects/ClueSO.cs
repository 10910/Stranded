using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ClueEntry
{
    public string conditionLogTitle;    // after read log in this title, this entry will be visible
    [TextArea(2,20)]
    public string text;
}

[Serializable]
public struct ClueLine{
    public List<ClueEntry> entries;
}
[CreateAssetMenu(fileName = "ClueSO", menuName = "ScriptableObjects/ClueSO")]
public class ClueSO : ScriptableObject
{
    public string title;
    public string displayTitle;
    public List<ClueLine> clueLines;
    //public Dictionary<string, string> entries;
    private void OnEnable() {
    }
}
