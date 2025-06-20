using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleFruit : Usable, IShootable, IInteractable
{
    public string InteractionText { get; set; } = "pluck";
    public float ShootingForce { get; set; }
    public float Velocity;
    bool _IsShooting;
    public Transform FPSCamera;
    public Shooter Shooter;
    public float ShootingDistance;
    public float ShootingVelocity;

    void Start()
    {
        FPSCamera = GameManager.instance.FPSCamera;
        Shooter = GameManager.instance.Shooter;
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
        gameObject.layer = LayerMask.NameToLayer("Environment");
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = Forward * ShootingVelocity;
        rb.useGravity = true;

    }

    public void Interact() {
        gameObject.layer = LayerMask.NameToLayer("Default");
        GetComponent<Fruit>().tree.GrowFruit().Forget();
        print("Interact with paralyse fruit");
        transform.SetParent(FPSCamera, false);
        //gameObject.layer = LayerMask.GetMask("Hands");
        transform.localPosition = new Vector3(0.145f, -0.089f, 0.661f);
        transform.localEulerAngles = Vector3.zero;
        Shooter.stored.Push(this);
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Creature")) {
            print("hit" + collision.gameObject.GetComponent<Creature>().name);
        }
    }
}
