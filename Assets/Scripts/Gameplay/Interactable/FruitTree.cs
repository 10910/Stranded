using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTree : MonoBehaviour
{
    public Transform fruitSpawnPoint;
    public GameObject fruitPrfb;
    public float growTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     public async UniTask GrowFruit(){
        await UniTask.WaitForSeconds(growTime);
        GameObject fruit = Instantiate(fruitPrfb, fruitSpawnPoint.position, Quaternion.Euler(-90, 0, 0), transform);
        fruit.GetComponent<Fruit>().tree = this;

     }

}
