using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "TextData", menuName = "ScriptableObjects/Compendium")]
public class CompendiumSO : ScriptableObject
{
    public CreatureInfoSO[] creaturesArray;
    public Dictionary<string, CreatureInfoSO> creatures;
    public string imagesDirPath;
    private void OnEnable()
    {
        // load creatureSO from Creatures folder
        creaturesArray = Resources.LoadAll<CreatureInfoSO>("Creatures");
        // TEST ONLY: set all creature's discovery state to undiscovered
        foreach (CreatureInfoSO creature in creaturesArray){
            creature.discoveredState = DiscoveryState.Undiscovered;
        }

        creatures = new Dictionary<string, CreatureInfoSO>();
        // load creature images
        foreach(var creature in creaturesArray){
            string filePath = imagesDirPath + creature.name + ".png";
            if (File.Exists(filePath))
            {
                var data = File.ReadAllBytes(filePath);
                creature.imageTex = new Texture2D(100, 100);
                creature.imageTex.LoadImage(data);
            }
            creatures.Add(creature.name, creature);
        }
    }

    public bool SaveImage(Texture2D tex, string creatureName){
        if(creatures.TryGetValue(creatureName, out CreatureInfoSO infoSO)){
            infoSO.imageTex = tex;
            var texData = infoSO.imageTex.EncodeToPNG();
            string filePath = imagesDirPath + creatureName + ".png";
            File.WriteAllBytes(filePath, texData);
            return true;
        }else{
            return false;
        }
    }

}

