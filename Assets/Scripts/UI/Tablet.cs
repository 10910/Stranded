using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tablet : MonoBehaviour
{
    public CompendiumUI compendiumUI;
    public Tab tab;
    public LogUI log;
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.SetActive(false);
        compendiumUI.CreateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnOpenTablet(InputAction.CallbackContext context) {
        if (context.started) {
            ToggleUI();
        }
    }

    public void OpenLog(LogTextSO so){
        // open log page
        tab.Toggle(2);
        log.ToggleBySO(so);
        ToggleUI();
    }

    public void ToggleUI(){
        if (gameObject.activeSelf) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            //GameManager.instance.input.SwitchCurrentActionMap("Player");
        }
        else {
            // open panel, stop time, switch to ui input map, show cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            gameObject.SetActive(true);
            //GameManager.instance.input.SwitchCurrentActionMap("UI");
        }
    }
}
