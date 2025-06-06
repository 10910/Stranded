using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompendiumUI : MonoBehaviour
{
    public CompendiumSO compendiumSO;
    public GameObject contentPrfb;
    public TextMeshProUGUI nameText, discovererText, discoveryText, descriptionText;
    public Transform Content; //parent of the buttons
    public ToggleGroup toggleGroup;
    public Image creatureImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateButtons(){
        for (int i = 0; i < compendiumSO.creaturesArray.Length; i++)
        {
            int index = i;
            GameObject instance = Instantiate(contentPrfb, Content);
            instance.GetComponent<InfoInitializer>().Initialize(compendiumSO.creaturesArray[i]);
            Toggle toggle = instance.GetComponent<Toggle>();
            toggle.group = toggleGroup;
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn) {
                    var info = compendiumSO.creaturesArray[index];
                    print($"{info.name} info load to ui");
                    nameText.text = info.name;
                    discoveryText.text = info.discoveredState.ToString();
                    if (info.discoveredState == DiscoveryState.Undiscovered) {
                        descriptionText.text = "Unknown";
                    }else{
                        string description = "";
                        foreach (string desc in info.descriptions) {
                            description += desc + "\n";
                        }
                        descriptionText.text = description;
                    }
                    if (info.imageTex != null) {
                        Sprite sprite = Sprite.Create(info.imageTex, new Rect(0, 0, info.imageTex.width, info.imageTex.height), new Vector2(0.5f, 0.5f));
                        creatureImage.sprite = sprite;
                    }else{
                        creatureImage.sprite = null;
                    }
                }
            });
        }
    }

    public void ClearButtons(){
        for (int i = 0; i < compendiumSO.creaturesArray.Length; i++)
        {
            Destroy(Content.GetChild(i).gameObject);
        }
    }
}
