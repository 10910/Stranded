using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tablet : MonoBehaviour
{
    public CompendiumUI compendiumUI;
    // Start is called before the first frame update
    void Start()
    {
        compendiumUI.CreateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnOpenTablet(InputAction.CallbackContext context) {
        if (context.started) {
            if (gameObject.activeSelf) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
                gameObject.SetActive(false);
                //GameManager.instance.input.SwitchCurrentActionMap("Player");
            }
            else{
                // open panel, stop time, switch to ui input map, show cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                gameObject.SetActive(true);
                //GameManager.instance.input.SwitchCurrentActionMap("UI");
            }
        }
    }
}
