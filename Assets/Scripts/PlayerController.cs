using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour {
    public float walkSpeed = 10, sprintSpeed = 20, crouchSpeed = 5, jumpHeight = 1.5f, gravForce = 19.6f;

    public GameObject model;

    Quaternion movementRotation;
    Quaternion slerpPoint = Quaternion.Euler(0, 0, 0);

    Vector2 playerMovementVector = Vector2.zero;
    Vector2 playerLookVector = Vector2.zero;
    Vector2 camVals = Vector2.zero;

    CharacterController controller;
    Animator animator;
    SpriteRenderer renderer;

    bool sprinting;
    bool crouching;
    float gravity = 0;

    GameObject[] inventory = new GameObject[3];

    void Awake() {
        EnsureComponentsExist();
    }

    void Update() {
        EnsureComponentsExist();

        CameraControl();
        FixRotation();
    }

    void FixedUpdate() {
        EnsureComponentsExist();

        float speed = walkSpeed;
        if (sprinting) speed = sprintSpeed;
        if (crouching) speed = crouchSpeed;

        Vector3 movement = movementRotation * transform.forward * playerMovementVector.magnitude * speed;
        movement -= Vector3.up * gravity;

        controller.Move(movement * Time.fixedDeltaTime);
        if (controller.isGrounded) {
            gravity = 0;
        }

        gravity += Time.fixedDeltaTime * gravForce;

        HandleRotation();
        FixRotation();

        if ( playerMovementVector.magnitude > 0) { 
            // Flips sprite based on left/right movement
            if (playerMovementVector.x > 0) {
                slerpPoint = Quaternion.Euler(0, 0, 0);
            } else if (playerMovementVector.x < 0) {
                slerpPoint = Quaternion.Euler(0, 180, 0);
            }

            // Flips sprite based on forward/backward movement
            animator.SetBool("show_back", playerMovementVector.y > 0);
            renderer.flipX = playerMovementVector.y > 0;
        }

            Quaternion rotation = Quaternion.Slerp(model.transform.localRotation, slerpPoint, 10 * Time.deltaTime);

        model.transform.localEulerAngles = new Vector3(
            0,
            rotation.eulerAngles.y, 
            0
        );

        animator.SetBool("crouching", crouching);
        animator.SetBool("sprinting", sprinting);
        animator.SetBool("moving", playerMovementVector.magnitude > 0.01f);
    }

    void CameraControl() {
        camVals.x += playerLookVector.x;
        camVals.x %= 2 * Mathf.PI;
        
        camVals.y += playerLookVector.y;
        camVals.y = Mathf.Clamp(camVals.y, 0f, 2f);
    }

    void FixRotation() {
        Vector3 cameraPos = new Vector3();

        cameraPos.x = Mathf.Cos(camVals.x) * 1.5f;
        cameraPos.z = Mathf.Sin(camVals.x) * 1.5f;
        cameraPos.y = camVals.y;

        Camera.main.transform.localPosition = (cameraPos * (2.5f + (0.5f * cameraPos.y * cameraPos.y) ));

        Vector3 lookPos = new Vector3(0, 2, 0) - Camera.main.transform.localPosition;
        Vector3 lookOffset = Vector3.up * 0.75f;

        Camera.main.transform.rotation = Quaternion.LookRotation(lookPos, transform.up);
        Camera.main.transform.forward = (transform.position + lookOffset) - Camera.main.transform.position;

        model.transform.parent.localEulerAngles = new Vector3(
            0, 
            Camera.main.transform.rotation.eulerAngles.y, 
            0
        );
    }

    void HandleRotation() {
        if(playerMovementVector.magnitude > 0) {
            //Looks away from camera
            Vector3 look = Vector3.zero;   
            
            look -= Camera.main.transform.forward * playerMovementVector.y;
            look -= Camera.main.transform.right * playerMovementVector.x;

            movementRotation = Quaternion.LookRotation(-look, transform.up);
            movementRotation = Quaternion.Euler(0, movementRotation.eulerAngles.y, 0);
        }
    }

    void EnsureComponentsExist() {
        if (controller == null) { controller = GetComponent<CharacterController>(); }
        if (animator == null) { animator = model.GetComponent<Animator>(); }
        if (renderer == null) { renderer = model.GetComponent<SpriteRenderer>(); }
    }

    public void MovementAction(InputAction.CallbackContext obj) {
        playerMovementVector = obj.ReadValue<Vector2>();
    }

    public void CameraAction(InputAction.CallbackContext obj) {
        playerLookVector = obj.ReadValue<Vector2>();
    }

    public void SprintAction(InputAction.CallbackContext obj) {
        sprinting = obj.performed;

        if(obj.performed) { crouching = false; }
    }

    public void CrouchAction(InputAction.CallbackContext obj) {
        crouching = obj.performed;

        if(obj.performed) { sprinting = false; }
    }

    public void JumpAction(InputAction.CallbackContext obj) {
        if (!obj.performed) { return; }
        if (!controller.isGrounded) { return; }
        gravity = -Mathf.Sqrt(2f * gravForce * jumpHeight);
    }

    public void DrawAction(InputAction.CallbackContext obj) {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Climb"))
        {
            Climb();
        }
    }

    public void Climb()
    {
        
    }
}
