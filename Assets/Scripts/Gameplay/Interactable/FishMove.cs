using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMove : MonoBehaviour
{
    public Transform fish, fishDest;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")){
            fish.DOMove(fishDest.position, 8f).SetEase(Ease.Linear).OnComplete(()=>{ Destroy(fish.gameObject); });
            GetComponent<Collider>().enabled = false;
        }
    }
}
