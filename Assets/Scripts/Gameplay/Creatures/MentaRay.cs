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

    public void Interact() {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
        shooter.stored.Push(this);
    }

}
