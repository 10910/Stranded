using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugeMantaRay : MonoBehaviour, IInteractable
{
    public string InteractionText { get; set; } = "grab";
    public Transform volcanoAbove, volcanoInterior, startPos;

    public void Interact() {
        gameObject.layer = LayerMask.NameToLayer("Default");
        GameManager.instance.movement.canMove = false;
        Sequence seq = DOTween.Sequence();
        seq.Append(GameManager.instance.Player.DOMove(startPos.position, 1f).SetEase(Ease.InCubic))
            .AppendCallback(() => { GameManager.instance.Player.transform.SetParent(transform); })
            .AppendInterval(0.5f)
            .Append(transform.DOMove(volcanoAbove.position, 14f).SetEase(Ease.InOutQuad))
            .Append(transform.DOMove(volcanoInterior.position, 4f).SetEase(Ease.Linear))
            .AppendCallback(() => { GameManager.instance.Player.transform.SetParent(transform); })
            .AppendCallback(() => { GameManager.instance.movement.canMove = true; });

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
