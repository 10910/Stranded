using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform FPSCamera;
    public Shooter Shooter;
    public GameObject DeathUI;
    public Transform Player, RespawnPoint;
    public Movement movement;
    public ParticleSystem volcano;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        //DeathUI.SetActive(false);
    }

    public void Respawn(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Player.transform.position = RespawnPoint.position;
        movement.enabled = true;
    }

    public void PlayerDie(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DeathUI.SetActive(true);
        movement.enabled = false;
    }

    public void PlayIntro(){
        
    }
}
