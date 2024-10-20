using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public Vector2 moveDir;
    public bool isTop;
    public bool isGround;
    public bool isCrouching;

    [SerializeField]
    private Vector2 lookDir;

    public bool isRunPressed;
    public bool isJumpPressed;

    public float topOffset;  //distance above player's head to block jumping
    public float bottomOffset;   //distance below player's feet to enable jumping and falling
    public float downSlopeOffset;   //distance below player's feet to considered as a downhill slope 
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float gravity;
    public float velocity;
    public float lookSensX;
    public float lookSensY;
    public float jumpInitialVelocity;

    public Camera childCamera;
    public CharacterController characterController;
    public MeshRenderer crouchMesh;
    public MeshRenderer standMesh;

    public Vector2 JumpVelocity;
    public Vector3 moveDelta;
    void Start() {
        standMesh = GetComponent<MeshRenderer>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        jumpInitialVelocity = 5;
    }

    void Update() {
        isGround = characterController.isGrounded;
        isTop = isTopBlocked();
        float t = Time.deltaTime;
        transform.Rotate(Vector3.up, lookDir.x * lookSensX * Time.deltaTime);
        rotateCamera();
    }

    public void rotateCamera() {
        //camera pitch & restrict the max degree
        float rotateDelta = lookDir.y * lookSensY * Time.deltaTime;
        float plannedRotate = childCamera.transform.localEulerAngles.x - rotateDelta;
        int range = (int)(plannedRotate / 90);
        //camera's transform.localEulerAngles.x is 0 - 90 degree when look down, 270 - 360 when look up
        //1 means larger than 90 degree(look down), 2 means lower than 270(look up)
        if (range == 1) {
            plannedRotate = 90f;
        }
        if (range == 2) {
            plannedRotate = -90f;
        }
        childCamera.transform.localEulerAngles = new Vector3(plannedRotate, 0, 0);
    }

    public void OnMove(InputAction.CallbackContext callbackContext) {
        moveDir = callbackContext.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext callbackContext) {
        lookDir = callbackContext.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext callbackContext) {
        if (callbackContext.started && characterController.isGrounded) {
            velocity = jumpInitialVelocity;
            JumpVelocity = transform.right * moveDir.x + transform.forward * moveDir.y;
            if(isRunPressed){
                JumpVelocity *= runSpeed;
            }else{
                JumpVelocity *= walkSpeed;
            }
            isJumpPressed = true;
        }else{
            isJumpPressed = false;
        }
        
    }

    public void OnCrouch(InputAction.CallbackContext callbackContext) {
        if (callbackContext.started && characterController.isGrounded) {
            isCrouching = true;
            characterController.height = 1;
            characterController.center -= new Vector3(0, 0.5f, 0);
            standMesh.enabled = false;
            crouchMesh.enabled = true;
            childCamera.transform.position -= new Vector3(0, 0.75f, 0);
        }
        else if (callbackContext.canceled) {
            isCrouching = false;
            characterController.height = 2;
            characterController.center = Vector3.zero;
            crouchMesh.enabled = false;
            standMesh.enabled = true;
            childCamera.transform.position += new Vector3(0, 0.75f, 0);
        }
    }

    public void OnRun(InputAction.CallbackContext callbackContext) {
        if (callbackContext.started && characterController.isGrounded) {
            isRunPressed = true;
        }
        else if (callbackContext.canceled) {
            isRunPressed = false;
        }
    }

    bool isGrounded() {
        return Physics.Raycast(transform.position, Vector3.down, characterController.height / 2 + bottomOffset);
        //Ray ray = new Ray(transform.position + Vector3.down * characterController.height / 4, Vector3.down);
        //return Physics.SphereCast(ray, 0.2f, bottomOffset);
    }

    public bool isTopBlocked() {
        return Physics.Raycast(transform.position, Vector3.up, topOffset + characterController.height / 2);
    }

    void handleSlope(ref Vector3 move) {
        Vector3 nextPos = transform.position + move;
        if (Physics.Raycast(nextPos + Vector3.down *(characterController.height / 2), 
            Vector3.down, out RaycastHit hitinfo, downSlopeOffset)) {
            move += Vector3.down * downSlopeOffset;
        }
    }
}
