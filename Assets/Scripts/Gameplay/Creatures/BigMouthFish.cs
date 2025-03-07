using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BigMouthFish : MonoBehaviour
{
    public GameObject Bait;
    public float MoveDistance = 17.0f;
    public float MoveDuration = 2.0f;
    public Transform DestPos;
    //public LayerMask Mask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async void OnTriggerEnter(Collider other) {
        print("entered");
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            print("catched");
            Destroy(Bait);
            await Task.Delay(1000);
            transform.DOMoveY(transform.position.y + MoveDistance, MoveDuration)
                .SetEase(Ease.OutQuint)
                .OnComplete(()=>
                { 
                    other.gameObject.transform.position = DestPos.position;
                    transform.DOMoveY(transform.position.y - MoveDistance, MoveDuration).SetEase(Ease.InQuint);
                });
        }
    }
}
