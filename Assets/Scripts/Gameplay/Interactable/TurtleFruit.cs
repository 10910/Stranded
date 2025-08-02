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
        Shooter = GameManager.instance.shooter;
    }

    void Update()
    {
        
    }

    public override void Use() {
        Shoot();
    }

    public void Shoot() {
        Vector3 Forward = FPSCamera.forward;
        print("shoot red fruit");
        transform.SetParent(null);
        gameObject.layer = LayerMask.NameToLayer("Environment");
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = Forward * ShootingVelocity;
        rb.useGravity = true;

    }

    public void Interact() {
        gameObject.layer = LayerMask.NameToLayer("Bullet");
        GetComponent<Fruit>().tree.GrowFruit().Forget();
        //print("Interact with turtle fruit");
        //transform.SetParent(FPSCamera, false);
        //transform.localPosition = new Vector3(0.145f, -0.089f, 0.661f);
        //transform.localEulerAngles = Vector3.zero;
        Shooter.Add(this);
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Creature")) {
            print("hit" + collision.gameObject.GetComponent<Creature>().name);
        }
    }
}
