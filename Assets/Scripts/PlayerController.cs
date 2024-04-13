using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour {
    public GameObject model;

    Quaternion movementRotation;
    Quaternion slerp_point = Quaternion.Euler(0, 45, 0);

    Vector2 playerMovementVector = Vector2.zero;
    Vector2 playerLookVector = Vector2.zero;
    Vector2 camVals = Vector2.zero;

    CharacterController controller;

    bool sprinting;
    float gravity = 0;
    float moveSpeed = 10;

    void Awake() {
        EnsureComponentsExist();
    }

    void FixedUpdate() {
        EnsureComponentsExist();

        Vector3 movement = movementRotation * transform.forward * playerMovementVector.magnitude * moveSpeed * (sprinting ? 2 : 1);
        movement -= Vector3.up * gravity;

        controller.Move(movement * Time.deltaTime);
        if (controller.isGrounded) {
            gravity = 0;
        }

        gravity += Time.deltaTime * 9.8f;

        HandleRotation();

        if ( movement.magnitude > 0) { 
            if (playerMovementVector.x > 0) {
                slerp_point = Quaternion.Euler(0, -135, 0);
            } else if (playerMovementVector.x < 0) {
                slerp_point = Quaternion.Euler(0, 45, 0);
            }

            Quaternion rotation = Quaternion.Slerp(model.transform.localRotation, slerp_point, 16 * Time.deltaTime);

            model.transform.localEulerAngles = new Vector3(
                0, 
                rotation.eulerAngles.y, 
                0
            );
        }

        
    }

    void HandleRotation() {
        if(playerMovementVector.magnitude > 0) {
            //Looks away from camera
            Vector3 look = Vector3.zero;   
            
            look -= Camera.main.transform.forward * playerMovementVector.y;
            look -= Camera.main.transform.right * playerMovementVector.x;

            Quaternion newRot = Quaternion.LookRotation(-look, transform.up);

            movementRotation = Quaternion.Slerp(movementRotation, newRot, 16 * Time.deltaTime);
            movementRotation = Quaternion.Euler(0, movementRotation.eulerAngles.y, 0);
        }
    }

    void CameraControl(Vector2 input) {
        Vector2 lookVector = input;

        camVals.x += lookVector.x;
        camVals.x %= 2 * Mathf.PI;
        
        camVals.y += lookVector.y;

        if(camVals.y > 2f) { camVals.y = 2f; }
        if(camVals.y < 0f) { camVals.y = 0f; }
    }

    void EnsureComponentsExist() {
        if (!controller) {
            controller = GetComponent<CharacterController>();
        }
    }

    public void MovementAction(InputAction.CallbackContext obj) {
        playerMovementVector = obj.ReadValue<Vector2>();
    }
}
