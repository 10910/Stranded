using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 movingDelta;

    private Vector3 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        movingDelta = transform.position - lastPos;
        lastPos = transform.position;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            print("player entered moving platform");
            //other.transform.SetParent(transform);
            other.gameObject.GetComponent<Movement>().platform = this;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            print("player exit moving platform");
            other.gameObject.GetComponent<Movement>().platform = null;
        }
    }
}
