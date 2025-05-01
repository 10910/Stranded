using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PromptText : MonoBehaviour
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

    public void setText(string text){
        textUI.text = text;
    }
}
