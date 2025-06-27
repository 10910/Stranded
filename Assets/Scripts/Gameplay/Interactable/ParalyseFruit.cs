using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalyseFruit : Usable, IShootable, IInteractable
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
        print("shoot paralyse");
        transform.SetParent(null);
        //var rb = GetComponent<Rigidbody>();
        //rb.velocity = Forward * ShootingVelocity;
        //rb.useGravity = true;
        //rb.isKinematic = false;
        transform.DOMove(transform.position + ShootingDistance * Forward, 1.5f).OnComplete(()=>Destroy(gameObject));
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

    public async void Eat(){
        GetComponent<Fruit>().tree.GrowFruit().Forget();
        //await UniTask.WaitForSeconds(5f);
        //Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Creature")) {
            print("hit" + collision.gameObject.GetComponent<Creature>().name);
        }
    }
}
