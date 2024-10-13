using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Goggles : MonoBehaviour
{
    public Image mask;
    public TextMeshProUGUI textUI;
    public float detectRadius;
    public float detectDistance;
    private Camera mainCamera;
    private int layerMask;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        layerMask = 1 << 6;
        layerMask += 1 << 7;
    }

    void Update()
    {
        if(mask.enabled){
            RaycastHit hit;
            if(Physics.SphereCast(mainCamera.transform.position, detectRadius, 
                        mainCamera.transform.forward, out hit, detectDistance, layerMask) && 
                        hit.collider.gameObject.layer == 6){
                textUI.text = hit.collider.tag;
            }else{
                textUI.text = null;
            }
        }
    }

    public void OnScan(InputAction.CallbackContext callbackContext){
        if(callbackContext.started){
            //add animation judgement
            if (!mask.gameObject.activeSelf)
            {
                mask.gameObject.SetActive(true);
            }else{
                mask.gameObject.SetActive(false);
            }
        }
    }
}
