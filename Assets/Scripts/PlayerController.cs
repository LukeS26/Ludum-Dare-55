using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour {
    public GameObject model;

    Quaternion movementRotation;
    Quaternion slerp_point = Quaternion.Euler(0, 0, 0);

    Vector2 playerMovementVector = Vector2.zero;
    Vector2 playerLookVector = Vector2.zero;
    Vector2 camVals = Vector2.zero;

    CharacterController controller;
    Animator animator;

    bool sprinting;
    bool crouching;
    float gravity = 0;
    float moveSpeed = 10;

    GameObject[] inventory = new GameObject[3];

    void Awake() {
        EnsureComponentsExist();
    }

    void FixedUpdate() {
        EnsureComponentsExist();

        Vector3 movement = movementRotation * transform.forward * playerMovementVector.magnitude * moveSpeed * (sprinting ? 2 : 1) * (crouching ? 0.5f : 1);
        movement -= Vector3.up * gravity;

        controller.Move(movement * Time.deltaTime);
        if (controller.isGrounded) {
            gravity = 0;
        }

        gravity += Time.deltaTime * 9.8f;

        HandleRotation();

        if ( movement.magnitude > 0) { 
            // Flips sprite based on left/right movement
            if (playerMovementVector.x > 0) {
                slerp_point = Quaternion.Euler(0, 0, 0);
            } else if (playerMovementVector.x < 0) {
                slerp_point = Quaternion.Euler(0, 180, 0);
            }

            // Flips sprite based on forward/backward movement
            animator.SetBool("show_back", playerMovementVector.y > 0);

            Quaternion rotation = Quaternion.Slerp(model.transform.localRotation, slerp_point, 10 * Time.deltaTime);

            model.transform.localEulerAngles = new Vector3(
                0, 
                rotation.eulerAngles.y, 
                0
            );
        }        

        animator.SetBool("crouching", crouching);
        animator.SetBool("sprinting", sprinting);
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
    }

    public void MovementAction(InputAction.CallbackContext obj) {
        playerMovementVector = obj.ReadValue<Vector2>();
    }

    public void SprintAction(InputAction.CallbackContext obj) {
        sprinting = !obj.canceled;

        if(!obj.canceled) { crouching = false; }
    }

    public void CrouchAction(InputAction.CallbackContext obj) {
        crouching = !obj.canceled;

        if(!obj.canceled) { sprinting = false; }
    }
}
