using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClearTextOnDisable : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    // Start is called before the first frame update
    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        if(!gameObject.activeInHierarchy){
            textUI.text = null;
        }
    }
}
