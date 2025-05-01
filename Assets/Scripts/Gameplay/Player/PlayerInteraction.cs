using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public RaycastHit hit;
    public float rayDistance;
    public float sphereRadius;
    public LayerMask hitLayerMask;
    public PromptText promptText;
    public GameObject promptUI;
    public GameObject promptUILMB;
    public TextMeshProUGUI promptTextLMB;

    private Shooter shooter;
    // Start is called before the first frame update
    void Start()
    {
        shooter = GetComponent<Shooter>();
        promptUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        if (shooter.stored.Count != 0){
            promptUILMB.SetActive(true);
            promptTextLMB.text = shooter.stored.Peek().Usage;
        }else{
            promptUILMB.SetActive(false);
            promptTextLMB.text = "";
        }
        
        promptUI.SetActive(false);
        promptText.setText(null);
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.SphereCast(ray, sphereRadius, out hit, rayDistance, hitLayerMask)) {
            promptUI.SetActive(true);
            promptText.setText(hit.collider.GetComponent<IInteractable>().InteractionText);
        }
        else {
            promptUI.SetActive(false);
            promptText.setText(null);
        }
        
    }

    public void OnInteract(InputAction.CallbackContext context){
        if(context.started){
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, hitLayerMask)){
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable")){
                    hit.collider.GetComponent<IInteractable>().Interact();
                    promptUI.SetActive(false);
                    promptText.setText(null);
                    hit.collider.gameObject.layer = LayerMask.NameToLayer("Default");
                }
            }
        }
    }
}
