using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class MentaRay : Creature, IInteractable
{
    public string InteractionText { get; set; } = "pick up";
    public Shooter shooter;
    private void Awake()
    {
        
    }   
    // Start is called before the first frame update
    void Start()
    {
        shooter = GameManager.instance.shooter;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Use()
    {
        Debug.Log("mentaRay.use()");
        GameManager.instance.movement.canMove = false;
        Vector3 Dir = GameManager.instance.IntroTargetPoint.position - GameManager.instance.Player.position;
        Dir.y = 0;
        Dir = Dir.normalized;
        Vector3 flyPoint = GameManager.instance.Player.position + Dir * 2f + Vector3.up * 3f;
        GameManager.instance.Player.DOMove(flyPoint, 1f).SetEase(Ease.OutCubic).OnComplete(() =>
             GameManager.instance.Player.DOMove(GameManager.instance.IntroTargetPoint.position, 10f).SetEase(Ease.Linear).OnComplete(() => {
                 GameManager.instance.movement.canMove = true;
                 Destroy(gameObject);
             })
            );
        GameManager.instance.PlayIntro().Forget();
    }

    public void Interact() {
        //GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponentInChildren<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        //GetComponent<RayAI>().enabled = false;
        //GetComponent<NavMeshAgent>().enabled = false;
        shooter.Add(this);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<ParalyseFruit>() != null) {
            var ai = GetComponent<RayAI>();
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            //transform.DOMoveY(transform.position.y - 3f, 2f);
            if (ai.IsHoldingFood()){
                var food = ai.GetHoldingFood();
                var rb = food.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = true;
            }
            Destroy(ai);
            Destroy(other.gameObject);
            gameObject.layer = LayerMask.NameToLayer("Interactable");
        }
    }
}