using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalyseFruit : Usable, IShootable, IInteractable
{
    public float ShootingForce { get; set; }
    public float Velocity;
    bool _IsShooting;
    public Transform FPSCamera;
    public Shooter Shooter;
    public float ShootingDistance;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void Use() {
        Shoot();
    }

    public void Shoot() {
        Vector3 Forward = FPSCamera.forward;
        print("shoot paralyse");
        transform.SetParent(null);
        transform.DOMove(transform.position + ShootingDistance * Forward, 1.5f).OnComplete(()=>Destroy(gameObject));
    }

    public void Interact() {
        print("Interact with paralyse fruit");
        transform.SetParent(FPSCamera, false);
        //gameObject.layer = LayerMask.GetMask("Hands");
        transform.localPosition = new Vector3(-1.427f, -2.261f, -0.964f);
        Shooter.stored.Push(this);
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Creature")) {
            print("hit" + collision.gameObject.GetComponent<Creature>().name);
        }
    }
}
