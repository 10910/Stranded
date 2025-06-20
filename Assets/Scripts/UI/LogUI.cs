using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
    public List<LogTextSO> logs;
    public GameObject contentPrfb;
    public Transform content; //parent of the buttons
    public ToggleGroup toggleGroup;
    public TextMeshProUGUI logBody;
    public MemberUI member;

    private Dictionary<LogTextSO, Toggle> toggleDict;
    // Start is called before the first frame update
    void Awake()
    {
        logs = new List<LogTextSO>();
        logs = Resources.LoadAll<LogTextSO>("Logs/").ToList();
        toggleDict = new Dictionary<LogTextSO, Toggle>();
        CreateToggles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateToggles(){
        for (int i = 0; i < logs.Count; i++) {
            int index = i;
            GameObject instance = Instantiate(contentPrfb, content);
            instance.GetComponentInChildren<TextMeshProUGUI>().text = logs[i].displayTitle;
            instance.SetActive(false);
            Toggle toggle = instance.GetComponent<Toggle>();
            toggle.group = toggleGroup;
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn) {
                    logBody.text = logs[index].entries["body"];
                }
            });
            toggleDict.Add(logs[i], toggle);
        }
    }

    public void ToggleBySO(LogTextSO so){
        if(!so.isViewed){
            so.isViewed = true;
            toggleDict[so].gameObject.SetActive(true);
            foreach (var entry in so.entries) { 
                if(entry.Key != "body"){
                    member.UpdateText(entry.Key, entry.Value);
                }
            }
        }
        toggleDict[so].isOn = true;
    }
}
