using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : Usable
{
    public CreatureInfoSO InfoSO;


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<ParalyseFruit>() != null) {
            if (gameObject.GetComponent<SinSwing>() != null) {
                Destroy(gameObject.GetComponent<SinSwing>());
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                //transform.DOMoveY(transform.position.y - 3f, 2f);
            }
        }
    }
}
