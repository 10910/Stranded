using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform FPSCamera;
    public Shooter shooter;
    public GameObject deathUI;
    public Transform Player, RespawnPoint, IntroTargetPoint, BigMantaRay, MantaRayTarget, MantaRayTarget2, MantaRayRespawn;
    public List<Transform> spawnPoints;
    [Range(0, 5)]
    public int spawnPointIndex;
    public Movement movement;
    public ParticleSystem volcanoBurst;
    public PlayerInput input;
    public GameObject crosshair;
    public PlayerState playerState;
    public Image blackScreen;
    public GameObject MantaRayPrfb;

    public Transform respawnButtons;
    public GameObject respawnPanel;
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
        for(int i = 0; i < spawnPoints.Count; i++){
            respawnButtons.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = spawnPoints[i].name;
            int idx = i;
            respawnButtons.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
            {
                respawnPanel.SetActive(false);
                Respawn(idx);
            });
        }
    }

    public void Respawn(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        Player.transform.position = RespawnPoint.position;
        movement.enabled = true;
    }

    public async void Respawn(int idx) {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        movement.canMove = false;
        Player.transform.position = spawnPoints[idx].position;
        await UniTask.WaitForSeconds(0.1f); movement.canMove = true;
    }

    public void PlayerDie(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        deathUI.SetActive(true);
        deathUI.GetComponentInChildren<TextMeshProUGUI>().text = "You Died";
    }

    public void PlayerDie(string deadText) {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        deathUI.SetActive(true);
        deathUI.GetComponentInChildren<TextMeshProUGUI>().text = deadText;
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
        await UniTask.WaitForSeconds(2f);

        

    }

    public void OnRespawnPanel(InputAction.CallbackContext context) {
        if (context.started) {
            if (respawnPanel.activeSelf) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
                respawnPanel.SetActive(false);
            }
            else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                respawnPanel.SetActive(true);
            }
        }
    }

    public void RespawnRay(){
        Instantiate(MantaRayPrfb).transform.position = MantaRayRespawn.position;
    }
}
