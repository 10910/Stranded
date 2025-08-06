using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public struct textEntry
{
    public string key;
    [TextArea(2,20)]
    public string text;
    public LocalizedString l_text;
}
[CreateAssetMenu(fileName = "TextData", menuName = "ScriptableObjects/TextData")]
public class LogTextSO : ScriptableObject
{
    public LocalizedString l_DisplayTitle;
    public string title;
    public string displayTitle;
    public bool isViewed;
    public List<textEntry> texts;
    public Dictionary<string, LocalizedString> entries;
    private void OnEnable() {
        title = name;
        //displayTitle = name;
        entries = new Dictionary<string, LocalizedString>();
        foreach (var text in texts) {
            if (text.key != "") {
                entries[text.key] = text.l_text;
            }else{
                Debug.LogWarning(title + ": Log entry key is empty");
            }
        }
        isViewed = false;
    }
}
