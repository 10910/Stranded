using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InfoLoader : MonoBehaviour
{
    public TextMeshPro projection;
    public Transform hand;
    public Transform dest;
    public Quaternion originRot;
    public Vector3 originPos;
    public float speed;
    public bool isAnimating;
    public float timer;
    public float t;
    public float distance;
    public AnimationCurve curve;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        projection.enabled = false;
        isAnimating = false;
        originPos = hand.localPosition;
        originRot = hand.localRotation; 
        projection.transform.localScale = Vector3.zero;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimating && timer <= 1f)
        {
            timer += Time.deltaTime * speed;
            t = curve.Evaluate(timer);
            if (projection.enabled){
                hand.localPosition = Vector3.Lerp(originPos, dest.transform.localPosition, t);
                hand.localRotation = Quaternion.Lerp(originRot, dest.transform.localRotation, t);
                projection.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 2.8f, t);
            }else{
                hand.localPosition = Vector3.Lerp(dest.localPosition, originPos, t);
                hand.localRotation = Quaternion.Lerp(dest.localRotation, originRot, t);
                projection.transform.localScale = Vector3.zero;
            }
        }else{ 
            isAnimating = false;
            if (projection.enabled) {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distance, layerMask))
                {
                    if (hit.collider.gameObject.layer == 8)
                    {
                        LogTextSO data = hit.collider.gameObject.GetComponent<Log>().data;
                        projection.text = data.texts[0];
                    }
                    else
                    {
                        projection.text = null;
                    }
                }else{
                    projection.text = null;
                }
            }
        }
    }

    public void OnLoadLog(InputAction.CallbackContext callbackContext){
        if (!isAnimating && callbackContext.started)
        {
            isAnimating = true;
            timer = 0;
            if (!projection.enabled)
            {
                projection.enabled = true;
            }else{
                projection.text= null;
                projection.enabled = false;
            }
        }
    }
}
