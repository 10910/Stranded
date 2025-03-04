using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFruit : Usable, IInteractable
{
    
    public void Interact() {
        transform.SetParent(GameManager.instance.FPSCamera, false);
        //gameObject.layer = LayerMask.GetMask("Hands");
        transform.localPosition = new Vector3(-1.57f, -2.51f, -0.89f);
        GameManager.instance.Shooter.stored.Push(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Use() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 8f);
        foreach (Collider collider in colliders) {
            if (collider.CompareTag("HighTurtle")) {
                print(collider.gameObject.name + "shocked");
                collider.GetComponent<HighTurtle>().RetreatShell();
            }
        }
        Destroy(gameObject);
    }
}
