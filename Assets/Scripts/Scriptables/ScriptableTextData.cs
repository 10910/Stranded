using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextData", menuName = "ScriptableObjects/TextData")]
public class ScriptableTextData : ScriptableObject
{
    public List<string> texts;
}
