using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LogMachine : MonoBehaviour, IInteractable
{
    public string InteractionText { get; set; } = "Open";
    public PlayerInput playerInput;
    public CompendiumUI compendiumUI;
    public GameObject projectionUI;
    void Start()
    {
        projectionUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IInteractable.Interact()
    {
        playerInput.SwitchCurrentActionMap("LogMachineUI");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        compendiumUI.CreateButtons();
        projectionUI.SetActive(true);
    }

    public void OnNavigate(InputAction.CallbackContext context){
        Debug.Log("navi");
    }

    public void OnExit(InputAction.CallbackContext context){
        playerInput.SwitchCurrentActionMap("Player");
        projectionUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        compendiumUI.ClearButtons();
    }
}
