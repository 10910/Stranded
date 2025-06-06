using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Goggles : MonoBehaviour
{
    public Image mask;
    public TextMeshProUGUI nameTMP, infoTMP;
    public float detectRadius;
    public float detectDistance;
    public CompendiumSO compendium;
    public LayerMask layerMask;
    public int captureWidth;
    public Texture2D tex;
    public GameObject scanKeyPrompt, snapKeyPrompt;


    private Camera _mainCamera;
    [SerializeField]
    private CreatureInfoSO _hitCreatureInfo;
    private int capturePosX, capturePosY;

    void Start()
    {
        capturePosX = (Screen.width - captureWidth)/ 2;
        capturePosY = (Screen.height - captureWidth)/ 2;
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if(mask.gameObject.activeSelf){
            RaycastHit hit;
            if(Physics.SphereCast(_mainCamera.transform.position, detectRadius, 
                        _mainCamera.transform.forward, out hit, detectDistance, layerMask) &&
                        hit.collider.GetComponent<Creature>()) {
                _hitCreatureInfo = hit.collider.GetComponent<Creature>().InfoSO;
                if (_hitCreatureInfo.discoveredState == DiscoveryState.Undiscovered){
                    nameTMP.text = "???";
                    infoTMP.text = "";
                    scanKeyPrompt.SetActive(true);
                    snapKeyPrompt.SetActive(false);
                }else{
                    nameTMP.text = _hitCreatureInfo.creatureName;
                    string description = "";
                    foreach (string desc in _hitCreatureInfo.descriptions){
                        description += desc + "\n";
                    }
                    infoTMP.text = description;
                    scanKeyPrompt.SetActive(false);
                    snapKeyPrompt.SetActive(true);
                }
            }
            else{
                nameTMP.text = "";
                infoTMP.text = "";
                _hitCreatureInfo = null;
                scanKeyPrompt.SetActive(false);
                snapKeyPrompt.SetActive(true);
            }
        }
    }

    public void OnGoggles(InputAction.CallbackContext context){
        if(context.started){
            //add animation judgement
            if (!mask.gameObject.activeSelf)
            {
                mask.gameObject.SetActive(true);
            }else{
                mask.gameObject.SetActive(false);
            }
        }
    }

    public void OnScan(InputAction.CallbackContext context) {
        if (context.started && _hitCreatureInfo != null && _hitCreatureInfo.discoveredState == DiscoveryState.Undiscovered) {
            print($"{_hitCreatureInfo.creatureName} scanned");
            _hitCreatureInfo.discoveredState = DiscoveryState.Completed;
        }
    }

    public void OnSnap(InputAction.CallbackContext context){
        if (context.started && _hitCreatureInfo != null) {
            StartCoroutine(Capture(_hitCreatureInfo));
        }
    }

    IEnumerator Capture(CreatureInfoSO info){
        yield return new WaitForEndOfFrame();
        tex = ScreenCapture.CaptureScreenshotAsTexture();
        compendium.SaveImage(CropTexture(tex), info.creatureName);
    }

    Texture2D CropTexture(Texture2D tex) {
        Texture2D final = new Texture2D(captureWidth, captureWidth);
        Color[] colors = tex.GetPixels(capturePosX, capturePosY, captureWidth, captureWidth);
        final.SetPixels(colors);
        final.Apply();
        return final;
    }
}
