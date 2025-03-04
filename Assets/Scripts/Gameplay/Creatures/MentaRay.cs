using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MentaRay : Creature, IInteractable
{
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
    }

    public async void Interact() {
        GameManager.instance.movement.gravity = 2f;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
        await Task.Delay(5000);
        GameManager.instance.movement.gravity = 15f;
        Destroy(gameObject);

    }

}
