using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    public GameObject shooter;
    public GameObject hittedgameObj;
    public float rayDistance;
    public RaycastHit hit;
    public LayerMask layerMask;
    public Stack<Usable> stored;
    public TextMeshProUGUI hud;
    public int capacity = 5;
    // Start is called before the first frame update
    void Start()
    {
        //shooter.SetActive(false);
        stored = new Stack<Usable>(capacity);
        hud.text = $"{stored.Count}/{capacity}";
    }

    // Update is called once per frame
    void Update()
    {
        if (shooter.activeSelf)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, layerMask)
                    && (((1 << hit.collider.gameObject.layer) & layerMask.value) != 0))
            {
                hittedgameObj = hit.collider.gameObject;
            }
            else{
                hittedgameObj = null;
            }
        }
    }

    public void OnEquipShooter(InputAction.CallbackContext context){
        //if(context.started){
        //    shooter.SetActive(shooter.activeSelf ? false : true);
        //}
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        Usable usable;
        if (shooter.activeSelf && context.started && stored.TryPop(out usable))
        {
            usable.Use();
            if(stored.TryPeek(out usable)){
                if (usable.GetComponent<Fruit>()) {
                    usable.GetComponent<Renderer>().enabled = true;
                }
                UpdateText(usable.name);
            }
            else {
                hud.text = $"{stored.Count}/{capacity}";
            }
            
        }
    }

    public void OnCatch(InputAction.CallbackContext context){
        if (context.started && hittedgameObj)
        {
            //stored.Push(hittedgameObj);
            hittedgameObj.SetActive(false);
            Debug.Log(hittedgameObj.tag + " catched");
        }
    }

    public bool Add(Usable usable){
        if(stored.Count == capacity){
            return false;
        }else{
            Usable top;
            // hide last fruit in shooter
            if(stored.TryPeek(out top)){
                if (top.GetComponent<Fruit>()) { 
                    top.GetComponent<Renderer>().enabled = false;
                }
            }
            if (usable.GetComponent<Fruit>()) {
                Transform tf = usable.transform;
                tf.SetParent(GameManager.instance.FPSCamera, false);
                tf.localPosition = new Vector3(0.145f, -0.089f, 0.661f);
                tf.localEulerAngles = Vector3.zero;
            }
            stored.Push(usable);
            UpdateText(usable.name);
            return true;
        }
    }

    void UpdateText(string name){
        name = Regex.Replace(name, @"\([^)]*\)", "");
        hud.text = $"{stored.Count}/{capacity}\n{name}";
    }
}
