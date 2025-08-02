using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyUseForNavmesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var collider = GetComponent<Collider>();
        if(collider != null ){
            collider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
