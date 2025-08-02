using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour, IInteractable
{
    public string InteractionText { get; set; } = "pick up";

    public void Interact() {
        Debug.Log("game end");
        GameManager.instance.PlayerDie("End");
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
