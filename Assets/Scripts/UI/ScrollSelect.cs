using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScrollSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform group;
    public Image selectedImg;
    public int maxIndex;
    public int currentIndex;
    void Start()
    {
        selectedImg = group.GetComponentInChildren<Image>();
        selectedImg.color = Color.yellow;
        currentIndex = 0;
        maxIndex = group.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onNavigate(InputAction.CallbackContext callbackContext){
        if (callbackContext.started)
        {
            int dir = (int)callbackContext.ReadValue<Vector2>().y;
            int nextIndex = -dir + currentIndex;
            if (nextIndex < maxIndex && nextIndex >= 0)
            {
                selectedImg.color = Color.white;
                selectedImg = group.GetChild(nextIndex).GetComponent<Image>();
                selectedImg.color = Color.yellow;
                currentIndex = nextIndex;
            }
        }
    }
}
