using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFruit : Usable, IInteractable
{
    public string InteractionText { get; set; } = "pluck";

    public void Interact() {
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null){
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        gameObject.layer = LayerMask.NameToLayer("Default");
        GetComponent<Fruit>().tree.GrowFruit().Forget();
        
        GameManager.instance.shooter.Add(this);
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
            if (!collider.isTrigger && collider.GetComponent<HighTurtle>()) {
                var turtle = collider.GetComponent<HighTurtle>();
                print(collider.gameObject.name + "shocked");
                turtle.RetreatShell();
            }
        }
        Destroy(gameObject);
    }

    public void Eat() {
        GetComponent<Fruit>().tree.GrowFruit().Forget();
    } 
}
