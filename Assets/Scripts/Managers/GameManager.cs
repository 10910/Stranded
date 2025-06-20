using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform FPSCamera;
    public Shooter shooter;
    public GameObject deathUI;
    public Transform Player, RespawnPoint, IntroTargetPoint, BigMantaRay, MantaRayTarget, MantaRayTarget2;
    public List<Transform> spawnPoints;
    [Range(0, 5)]
    public int spawnPointIndex;
    public Movement movement;
    public ParticleSystem volcanoBurst;
    public PlayerInput input;
    public GameObject crosshair;
    public PlayerState playerState;
    public Image blackScreen;
    private void Awake() {
        instance = this;
    }

    private void Start() {
        if(spawnPointIndex >= spawnPoints.Count){
            throw new System.ArgumentOutOfRangeException("spawnPointIndex");
        }
        //DeathUI.SetActive(false);
        Player.position = spawnPoints[spawnPointIndex].position;
        //PlayIntro().Forget();
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
        deathUI.SetActive(true);
        movement.enabled = false;
    }

    public async UniTask PlayIntro(){
        movement.canMove = false;
        Vector3 Dir = IntroTargetPoint.position - Player.position;
        Dir.y = 0;
        Dir = Dir.normalized;
        Vector3 flyPoint = Player.position + Dir * 2f + Vector3.up * 3f;
        Player.DOMove(flyPoint, 1f).SetEase(Ease.OutCubic).OnComplete(()=>
            Player.DOMove(IntroTargetPoint.position, 8f).SetEase(Ease.Linear).OnComplete(() => movement.canMove = true)
            );
        await UniTask.WaitForSeconds(3f);

        volcanoBurst.Play();
        BigMantaRay.LookAt(MantaRayTarget.position);
        Sequence seq = DOTween.Sequence();
        seq.Append(BigMantaRay.DOMove(MantaRayTarget.position, 3f)
            .SetEase(Ease.Linear));
        seq.Append(BigMantaRay.DOMove(MantaRayTarget2.position, 2f).SetEase(Ease.Linear));
        seq.Join(BigMantaRay.DOLocalRotate(new Vector3(85, 0, 0), 2f));
    }
}
