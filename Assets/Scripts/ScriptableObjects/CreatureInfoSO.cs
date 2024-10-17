using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TextData", menuName = "ScriptableObjects/CreatureInfo")]
public class CreatureInfoSO : ScriptableObject
{
    public string creatureName;
    public ShipMember firstDiscoverer;
    public ThreatLevel threatLevel;
    public DiscoveryState discoveredState;
    public Texture2D imageTex;
    public Sprite sprite;
    public List<string> descriptions;
    public List<InfoUnit<string>> descriptions__;

}

public struct DescriptionText{
    public string text;
    public bool isRevealed;
    public bool isViewed;
}

public enum ShipMember{
    Me,
    Captain,
    Engineer,
    Professor,
    Assistant,
    Unknown
}

public enum ThreatLevel{
    Safe,
    Depends,
    Run
}

public enum DiscoveryState{
    Undiscovered,
    UnderExploration,
    NeedRecover,
    Completed
}