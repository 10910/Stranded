using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
    public GameObject contentPrfb;
    public Transform content; //parent of the buttons
    public ToggleGroup toggleGroup;
    public TextMeshProUGUI logBody;
    public MemberUI member;

    private Dictionary<LogTextSO, Toggle> toggleDict;
    // Start is called before the first frame update
    void Awake()
    {
        toggleDict = new Dictionary<LogTextSO, Toggle>();

        Addressables.LoadResourceLocationsAsync("Log")
            .Completed += handle =>
            {
                foreach (var loc in handle.Result) {
                    Addressables.LoadAssetAsync<LogTextSO>(loc).Completed += h =>
                    {
                        Debug.Log("º”‘ÿ£∫" + h.Result.name);
                        CreateToggle(h.Result);
                    };
                }
            };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateToggle(LogTextSO logSO){
        GameObject instance = Instantiate(contentPrfb, content);
        instance.GetComponentInChildren<TextMeshProUGUI>().text = logSO.displayTitle;
        instance.SetActive(false);
        Toggle toggle = instance.GetComponent<Toggle>();
        toggle.group = toggleGroup;
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) {
                logBody.text = logSO.entries["body"].GetLocalizedString();
            }
        });
        toggleDict.Add(logSO, toggle);
    }

    public void ToggleBySO(LogTextSO so){
        if(!so.isViewed){
            // first time view, display toggle UI and select it
            so.isViewed = true;
            toggleDict[so].gameObject.SetActive(true);
            foreach (var entry in so.entries) { 
                if(entry.Key != "body"){
                    //member.UpdateMemberInfo(entry.Key, entry.Value.GetLocalizedString());
                }
            }
        }
        toggleDict[so].isOn = true;
        toggleDict[so].Select();
        //LayoutRebuilder.ForceRebuildLayoutImmediate(toggleDict[so].transform.parent as RectTransform);
    }
}
