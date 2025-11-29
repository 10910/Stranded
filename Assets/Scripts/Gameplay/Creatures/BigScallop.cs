using Cysharp.Threading.Tasks;
using DG.Tweening;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BigScallop : MonoBehaviour
{
    public Transform jaw;
    public Vector3 rotationAngle;
    public float rotationDuration;
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
            jaw.DOLocalRotate(rotationAngle, rotationDuration);
            GetComponent<Collider>().enabled = false;
        }
        
    }
}
