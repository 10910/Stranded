using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlatform : MonoBehaviour
{
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 3f, mask);
        //if (hit.collider == null) {
        //    print("movable");
        //    transform.SetParent(hit.transform);
        //}
    }
}
