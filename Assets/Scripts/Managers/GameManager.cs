using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform FPSCamera, HandsCamera;
    public Shooter shooter;
    public GameObject deathUI;
    public Transform Player, RespawnPoint, IntroTargetPoint, VolcanoBigRay, PeakBigRay, MantaRayTarget, MantaRayTarget2, MantaRayRespawn;
    public List<Transform> spawnPoints;
    [Range(0, 10)]
    public int spawnPointIndex;
    public Movement movement;
    public ParticleSystem volcanoBurst;
    public PlayerInput input;
    public GameObject crosshair;
    public PlayerState playerState;
    public Image blackScreen;
    public GameObject MantaRayPrfb;
    public LogUI logUI;

    public Transform respawnButtons;
    public GameObject respawnPanel, menuPanel;
    public bool firstGlide = true;
    private void Awake() {
        instance = this;
        SetLanguage();
        logUI.LoadResources();

    }

    private void Start() {

        if (spawnPointIndex >= spawnPoints.Count){
            throw new System.ArgumentOutOfRangeException("spawnPointIndex");
        }
        //DeathUI.SetActive(false);
        Player.position = spawnPoints[spawnPointIndex].position;
        //PlayIntro().Forget();
        for(int i = 0; i < spawnPoints.Count; i++){
            Transform buttonTf = respawnButtons.GetChild(i);
            RespawnPoint rp = spawnPoints[i].GetComponentInChildren<RespawnPoint>();
            buttonTf.GetComponentInChildren<TextMeshProUGUI>().text
                = rp.lName.GetLocalizedString();
            buttonTf.gameObject.SetActive(rp.isStarter);
            rp.index = i;

            int idx = i;

            respawnButtons.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
            {
                respawnPanel.SetActive(false);
                Respawn(idx);
            });
        }
        PeakBigRay.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        menuPanel.SetActive(true);
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
        //BigMantaRay.gameObject.SetActive(true);
        // move up, then move to target
        

        if (firstGlide) {
            firstGlide = false;
            await UniTask.WaitForSeconds(2f);
            Sequence seq = DOTween.Sequence();
            seq.Append(VolcanoBigRay.DOMove(MantaRayTarget.position, 2f).SetEase(Ease.Linear));
            seq.Join(VolcanoBigRay.DOLocalRotate(new Vector3(0, 270, 0), 2f));
            //seq.AppendCallback(() => { BigMantaRay.LookAt(SubBigMantaRay.position); });
            seq.Append(VolcanoBigRay.DOMove(PeakBigRay.position, 10f)
                .SetEase(Ease.Linear));
            seq.AppendCallback(() => { VolcanoBigRay.gameObject.SetActive(false); });

            await UniTask.WaitForSeconds(2f);
            volcanoBurst.Play();

            await UniTask.WaitForSeconds(11f);
            PeakBigRay.gameObject.SetActive(true);

            //Instantiate(MantaRayPrfb).transform.position = MantaRayRespawn.position;
        }
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

    public void OnMenu(InputAction.CallbackContext context) {
        if (context.started) {
            if (menuPanel.activeSelf) {
                CloseMenu();
            }
            else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                menuPanel.SetActive(true);
            }
        }
    }
    public void CloseMenu() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        menuPanel.SetActive(false);
    }

    public void Exit() {
        Application.Quit();
    }

    public void SetLanguage(){
        var locale = LocalizationSettings.AvailableLocales.GetLocale("zh-Hans");
        LocalizationSettings.SelectedLocale = locale;
    }


}
