using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
    public GameObject contentPrfb;
    public Transform content; //parent of the buttons
    public ToggleGroup toggleGroup;
    public TextMeshProUGUI logBody;
    public MemberUI member;

    private Dictionary<string, Toggle> toggleDict;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateToggle(LogTextSO logSO){
        
        GameObject instance = Instantiate(contentPrfb, content);
        print(logSO.l_DisplayTitle.GetLocalizedString());
        instance.GetComponentInChildren<TextMeshProUGUI>().text = logSO.l_DisplayTitle.GetLocalizedString();
        instance.SetActive(false);
        Toggle toggle = instance.GetComponent<Toggle>();
        toggle.group = toggleGroup;
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) {
                logBody.text = logSO.entries["body"].GetLocalizedString();
            }
        });
        toggleDict.Add(logSO.title, toggle);
        //Debug.Log("toggle created");
        //print("dict size:" + toggleDict.Count);

    }

    public void ToggleBySO(LogTextSO so){
        if(!so.isViewed){
            // first time view, display toggle UI and select it
            so.isViewed = true;
            toggleDict[so.title].gameObject.SetActive(true);
            foreach (var entry in so.entries) { 
                if(entry.Key != "body"){
                    //member.UpdateMemberInfo(entry.Key, entry.Value.GetLocalizedString());
                }
            }
        }
        toggleDict[so.title].isOn = true;
        toggleDict[so.title].Select();
        //LayoutRebuilder.ForceRebuildLayoutImmediate(toggleDict[so].transform.parent as RectTransform);
    }

    public void LoadResources(){
        toggleDict = new Dictionary<string, Toggle>();

        var locHandle = Addressables.LoadResourceLocationsAsync("Log");
        locHandle.WaitForCompletion();

        if (locHandle.Status == AsyncOperationStatus.Succeeded) {
            foreach (var loc in locHandle.Result) {
                var assetHandle = Addressables.LoadAssetAsync<LogTextSO>(loc);
                assetHandle.WaitForCompletion();

                if (assetHandle.Status == AsyncOperationStatus.Succeeded) {
                    Debug.Log("加载：" + assetHandle.Result.name);
                    CreateToggle(assetHandle.Result);
                }
                else {
                    Debug.LogWarning($"加载失败：{loc.PrimaryKey}");
                }
            }
        }
        else {
            Debug.LogError("获取资源位置失败！");
        }
    }
}
