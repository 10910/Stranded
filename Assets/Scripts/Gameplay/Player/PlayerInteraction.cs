using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public RaycastHit hit;
    public float rayDistance;
    public LayerMask hitLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract(InputAction.CallbackContext context){
        if(context.started){
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, hitLayerMask)){
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable")){
                    hit.collider.GetComponent<IInteractable>().Interact();
                }
            }
        }
    }
}
