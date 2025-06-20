using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFruit : Usable, IInteractable
{
    public string InteractionText { get; set; } = "pluck";

    public void Interact() {
        gameObject.layer = LayerMask.NameToLayer("Default");
        GetComponent<Fruit>().tree.GrowFruit().Forget();
        transform.SetParent(GameManager.instance.FPSCamera, false);
        //gameObject.layer = LayerMask.GetMask("Hands");
        transform.localPosition = new Vector3(0.145f, -0.089f, 0.661f);
        transform.localEulerAngles = Vector3.zero;
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
