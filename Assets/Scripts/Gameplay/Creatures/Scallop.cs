using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scallop : Creature, IInteractable
{
    public string InteractionText { get; set; } = "pick up";
    public Vector3 rotationAngle;
    public float rotationDuration;
    public Transform jaw;
    public void Interact() {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
        GameManager.instance.shooter.stored.Push(this);
    }

    public override void Use() {
        GameManager.instance.playerState.isProtected = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Eaten();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Eaten(){
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        jaw.DOLocalRotate(jaw.localEulerAngles + rotationAngle, rotationDuration);
    }
}
