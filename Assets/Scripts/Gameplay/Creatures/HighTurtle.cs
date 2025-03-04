using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HighTurtle : Creature
{
    public Transform head;
    public float height;
    public float duration;
    public float headTimePercent;
    public float headDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void RetreatShell(){
        transform.DOMoveY(transform.position.y - height, duration).SetEase(Ease.OutQuad);
        float headTime = duration * 1000 * headTimePercent;
        float headDuration = duration * 1000 * (1 - headTimePercent);
        await Task.Delay((int)headTime);
        head.transform.DOLocalMoveX(head.transform.localPosition.x - headDistance, headDuration);
    }
}
