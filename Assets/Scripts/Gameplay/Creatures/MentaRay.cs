using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MentaRay : Creature, IInteractable
{
    public string InteractionText { get; set; } = "pick up";
    public Shooter shooter;
    public DeadZone deadZone;
    private void Awake()
    {
    }   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Use()
    {
        Debug.Log("mentaRay.use()");
        deadZone.gameObject.SetActive(false);
        GameManager.instance.PlayIntro().Forget();
    }

    public async void Interact() {
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        shooter.Add(this);
        await UniTask.WaitForSeconds(5f);

        GameManager.instance.RespawnRay();
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