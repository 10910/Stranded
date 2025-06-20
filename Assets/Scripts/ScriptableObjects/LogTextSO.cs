using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct textEntry
{
    public string key;
    [TextArea(2,20)]
    public string text;
}
[CreateAssetMenu(fileName = "TextData", menuName = "ScriptableObjects/TextData")]
public class LogTextSO : ScriptableObject
{
    public string title;
    public string displayTitle;
    public bool isViewed;
    public List<textEntry> texts;
    public Dictionary<string, string> entries;
    private void OnEnable() {
        title = name;
        displayTitle = name;
        entries = new Dictionary<string, string>();
        foreach (var text in texts) {
            if (text.key != "") {
                entries[text.key] = text.text;
            }else{
                Debug.LogWarning(title + ": Log entry key is empty");
            }
        }
        isViewed = false;
    }
}
