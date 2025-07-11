using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private Vector2 moveDir;
    [SerializeField]
    private bool isTop;
    public bool isGround;
    [SerializeField]
    private bool isCrouching;

    public bool isRunning;
    [SerializeField]
    private Vector2 lookDir;

    public bool isJumping;
    public bool isFalling;
    public bool isJumpPressed;
    public bool canMove = true;
    public bool canLook = true;

    public float topOffset;  //distance above player's head to block jumping
    public float bottomOffset;   //distance below player's feet to enable jumping and falling
    public float downSlopeOffset;   //distance below player's feet to considered as a downhill slope 
    public float moveSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float fallingMoveSpeed;
    public float gravity;
    public float velocity;
    public float lookSensX;
    public float lookSensY;

    public Camera childCamera;
    public CharacterController characterController;
    public MeshRenderer crouchMesh;
    public MeshRenderer standMesh;
    public MovingPlatform platform = null;

    private Vector2 JumpDir;
    void Start() {
        canMove = true;
        canLook = true;
        standMesh = GetComponent<MeshRenderer>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        isGround = characterController.isGrounded;
        isTop = isTopBlocked();
        if (canLook){
            look();
        }
        if (canMove){
            move();
        }
    }

    public void move() {
        //move or run or crouch
        Vector3 moveDelta = transform.right * moveDir.x + transform.forward * moveDir.y;
        float speed;
        if (isRunning) {
            speed = runSpeed;
        }
        else if (isCrouching) {
            speed = crouchSpeed;
        }
        else {
            speed = moveSpeed;
        }
        moveDelta *= speed * Time.deltaTime;
        

        //vertical velocity
        if(!characterController.isGrounded){
            isFalling = true;
        }
        if (isJumping){
            velocity -= Time.deltaTime * gravity * 0.8f;
            if(velocity < 0 || isTopBlocked()) {
                velocity = 0;
                isJumping = false;
                isFalling = true;
            }
            Vector3 verticalDelta = Vector3.up * velocity * Time.deltaTime;
            moveDelta += verticalDelta;
        }else if(isFalling && !characterController.isGrounded){
            velocity -= Time.deltaTime * gravity;
            Vector3 verticalDelta = Vector3.up * velocity * Time.deltaTime;
            moveDelta += verticalDelta;
        }else{
            velocity = 0;
            isFalling= false;
            moveDelta += Vector3.down * 0.2f;
            //handleSlope(ref moveDelta);
        }
        if(platform != null){
            moveDelta += platform.movingDelta;
        }
        characterController.Move(moveDelta);
    }

    public void look() {
        //rotate character by mouseY
        transform.Rotate(Vector3.up, lookDir.x * lookSensX * Time.deltaTime);
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
        //if (canMove) {
            moveDir = callbackContext.ReadValue<Vector2>();
        //}
    }
    public void OnLook(InputAction.CallbackContext callbackContext) {
        lookDir = callbackContext.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext callbackContext) {
        if (callbackContext.started && characterController.isGrounded) {
            velocity = 5f;
            JumpDir = moveDir;
            isJumping = true;
            isJumpPressed = true;
        }else{
            isJumpPressed = false;
        }
        
    }

    public void OnCrouch(InputAction.CallbackContext callbackContext) {
        //if (callbackContext.started && characterController.isGrounded) {
        //    isCrouching = true;
        //    characterController.height = 1;
        //    characterController.center -= new Vector3(0, 0.5f, 0);
        //    standMesh.enabled = false;
        //    crouchMesh.enabled = true;
        //    childCamera.transform.position -= new Vector3(0, 0.75f, 0);
        //}
        //else if (callbackContext.canceled) {
        //    isCrouching = false;
        //    characterController.height = 2;
        //    characterController.center = Vector3.zero;
        //    crouchMesh.enabled = false;
        //    standMesh.enabled = true;
        //    childCamera.transform.position += new Vector3(0, 0.75f, 0);
        //}
    }

    public void OnRun(InputAction.CallbackContext callbackContext) {
        //if (callbackContext.started && characterController.isGrounded) {
        //    isRunning = true;
        //}
        //else if (callbackContext.canceled) {
        //    isRunning = false;
        //}
    }

    //bool isGrounded() {
    //    //return Physics.Raycast(transform.position, Vector3.down, characterController.height / 2 + bottomOffset);
    //    Ray ray = new Ray(transform.position + Vector3.down * characterController.height / 4, Vector3.down);
    //    return Physics.SphereCast(ray, 0.6f, bottomOffset);
    //}

    bool isTopBlocked() {
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
