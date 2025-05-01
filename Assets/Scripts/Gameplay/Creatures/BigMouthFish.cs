using Cysharp.Threading.Tasks;
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
    public Transform TrapPoint;
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            print("catched");
            Destroy(Bait);
            GameManager.instance.movement.canMove = false;
            GameManager.instance.Player.DOMove(TrapPoint.position, 1.5f).SetEase(Ease.InCubic);
            await UniTask.WaitForSeconds(1.8f);
            GameManager.instance.movement.canMove = true;
            transform.DOMoveY(transform.position.y + MoveDistance, MoveDuration)
                .SetEase(Ease.OutQuint)
                .OnComplete(()=>
                {
                    GameManager.instance.Player.position = DestPos.position;
                    transform.DOMoveY(transform.position.y - MoveDistance, MoveDuration).SetEase(Ease.InQuint);
                });
        }
    }
}
