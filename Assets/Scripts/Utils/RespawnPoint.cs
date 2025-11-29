using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class RespawnPoint : MonoBehaviour
{
    public LocalizedString lName;
    public bool isStarter;
    public int index;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            print("´«ËÍµã¿ªÆô£º " + lName.GetLocalizedString());
            GameManager.instance.respawnButtons.GetChild(index).gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
