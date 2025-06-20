using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    public Transform pagesParent;
    public List<GameObject> pages = new List<GameObject>();
    public List<Toggle> toggles = new List<Toggle>();
    public int defaultIdx;

    void Start() {
        for (int i = 0; i < transform.childCount; i++) {
            int index = i;
            Toggle toggle = transform.GetChild(i).gameObject.GetComponent<Toggle>();
            toggles.Add(toggle);
            if (i == defaultIdx) {
                toggle.isOn = true;
            }
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn) ShowPage(index);
            });
        }
        foreach (Transform page in pagesParent) { 
            pages.Add(page.gameObject);
        }
        ShowPage(defaultIdx);
        toggles[defaultIdx].isOn = true;   
    }

    void ShowPage(int index) {
        for (int i = 0; i < pages.Count; i++) {
            pages[i].SetActive(i == index);
        }
    }

    public void Toggle(int i){
        toggles[i].isOn = true;
    }
}
