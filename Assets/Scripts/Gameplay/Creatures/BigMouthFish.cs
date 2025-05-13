using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            print("catched");
            Destroy(Bait);
            //GetComponent<CapsuleCollider>().enabled = false;
            //GameManager.instance.movement.canMove = false;
            Sequence seq = DOTween.Sequence();
            seq.Append(GameManager.instance.Player.DOMove(TrapPoint.position, 1.5f).SetEase(Ease.InCubic))
                .AppendInterval(0.5f)
                //.AppendCallback(() => GameManager.instance.movement.canMove = true)
                .Append(transform.DOMoveY(transform.position.y + MoveDistance, MoveDuration).SetEase(Ease.OutQuint))
                .AppendCallback(() => GameManager.instance.Player.position = DestPos.position)
                .Append(transform.DOMoveY(transform.position.y, MoveDuration).SetEase(Ease.InQuint));
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Creature")) {
            print("catched animal");
            Destroy(Bait);
            var ai = other.GetComponent<AnimalAI>();
            if (ai){
                ai.enabled = false;
                ai.GetComponent<NavMeshAgent>().enabled = false;
            }
            //GetComponent<CapsuleCollider>().enabled = false;
            Sequence seq = DOTween.Sequence();
            seq//Append(other.gameObject.transform.DOMove(TrapPoint.position, 1.5f).SetEase(Ease.InCubic))
                .AppendInterval(0.5f)
                .Append(transform.DOMoveY(transform.position.y + MoveDistance, MoveDuration).SetEase(Ease.OutQuint))
                .InsertCallback(1f, () => Destroy(other.gameObject))
                .Append(transform.DOMoveY(transform.position.y, MoveDuration).SetEase(Ease.OutQuint));
        }
    }
}
