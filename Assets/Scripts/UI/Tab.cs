using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    public Transform pagesParent;
    public List<GameObject> pages = new List<GameObject>();
    public int defaultIdx;

    void Start() {
        for (int i = 0; i < transform.childCount; i++) {
            int index = i;
            Toggle toggle = transform.GetChild(i).gameObject.GetComponent<Toggle>();
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
    }

    void ShowPage(int index) {
        for (int i = 0; i < pages.Count; i++) {
            pages[i].SetActive(i == index);
        }
    }
}
