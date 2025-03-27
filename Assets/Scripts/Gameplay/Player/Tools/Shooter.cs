using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
        //shooter.SetActive(false);
        stored = new Stack<Usable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shooter.activeSelf)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, layerMask)
                    && hit.collider.gameObject.layer == 6)
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
        Usable shooted;
        if (shooter.activeSelf && context.started && stored.TryPop(out shooted))
        {
            shooted.GetComponent<Usable>().Use();
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


}
