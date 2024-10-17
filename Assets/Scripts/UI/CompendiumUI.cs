using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompendiumUI : MonoBehaviour
{
    public CompendiumSO compendiumSO;
    public GameObject contentPrfb;
    public TextMeshProUGUI nameText, discovererText, discoveryText, descriptionText;
    public Transform Content; //parent of the buttons

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
            GameObject instance = Instantiate(contentPrfb, Content);
            instance.GetComponent<InfoInitializer>().Initialize(compendiumSO.creaturesArray[i]);
        }
    }

    public void ClearButtons(){
        for (int i = 0; i < compendiumSO.creaturesArray.Length; i++)
        {
            Destroy(Content.GetChild(i).gameObject);
        }
    }
}
