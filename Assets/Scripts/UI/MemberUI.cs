using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemberUI : MonoBehaviour
{
    public Transform content; //parent of the buttons
    public ToggleGroup toggleGroup;
    public TextMeshProUGUI memberBody;
    public List<Clue> membersText;
    
    [System.Serializable]
    public class Clue{
        [TextArea(2,10)]
        public string text;
        public Clue(string txt) { text = txt; }
    }

    private Dictionary<string, Clue> memberDict;
    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log(logs[0].entries["body"]);
        memberDict = new Dictionary<string, Clue>();
        BindToggles();
        memberDict.Add("Recorder", membersText[0]);
        memberDict.Add("Captain", membersText[1]);
        memberDict.Add("Professor", membersText[2]);
        memberDict.Add("Assistant", membersText[3]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BindToggles(){
        for (int i = 0; i < content.childCount; i++) {
            int index = i;
            Toggle toggle = content.GetChild(i).GetComponent<Toggle>();
            toggle.group = toggleGroup;
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn) {
                    memberBody.text = membersText[index].text;
                }
            });
        }
    }
    
    public void UpdateText(string key, string text){
        memberDict[key].text += text;
    }
}
