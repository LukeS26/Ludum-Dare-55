using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour {
    public GameObject model;
    public GameObject summoningCirclePrefab;

    Quaternion movementRotation;
    Quaternion slerpPoint = Quaternion.Euler(0, 0, 0);

    Vector2 playerMovementVector = Vector2.zero;
    Vector2 playerLookVector = Vector2.zero;
    Vector2 camVals = Vector2.zero;

    CharacterController controller;
    Animator animator;
    SpriteRenderer renderer;

    Inventory inventory;

    bool sprinting;
    bool crouching;
    float gravity = 0;
    float moveSpeed = 10;

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

        Vector3 movement = movementRotation * transform.forward * playerMovementVector.magnitude * moveSpeed * (sprinting ? 2 : 1) * (crouching ? 0.5f : 1);
        movement -= Vector3.up * gravity;

        controller.Move(movement * Time.fixedDeltaTime);
        if (controller.isGrounded) {
            gravity = 0;
        }

        gravity += Time.fixedDeltaTime * 15;

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

        if(camVals.y > 2f) { camVals.y = 2f; }
        if(camVals.y < 0f) { camVals.y = 0f; }
    }

    void FixRotation() {
        Vector3 cameraPos = new Vector3();

        cameraPos.x = Mathf.Cos(camVals.x) * 1.5f;
        cameraPos.z = Mathf.Sin(camVals.x) * 1.5f;
        cameraPos.y = camVals.y;

        Camera.main.transform.localPosition = (cameraPos * (2.5f + (0.5f * cameraPos.y * cameraPos.y) ));

        Vector3 lookPos = new Vector3(0, 1, 0) - Camera.main.transform.localPosition;

        Camera.main.transform.rotation = Quaternion.LookRotation(lookPos, transform.up);

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
        if (inventory == null) { inventory = GetComponent<Inventory>(); }
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

    // public void CrouchAction(InputAction.CallbackContext obj) {
    //     crouching = obj.performed;

    //     if(obj.performed) { sprinting = false; }
    // }

    public void JumpAction(InputAction.CallbackContext obj) {
        if (!obj.performed) { return; }
        if (!controller.isGrounded) { return; }
        gravity = -7f;
    }

    public void DrawAction(InputAction.CallbackContext obj) {
        if(!obj.performed) { return; }
        if(!controller.isGrounded) { return; }

        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up, -Vector3.up, out hit, Mathf.Infinity, 1 << 9, QueryTriggerInteraction.Ignore)) {
            Instantiate(summoningCirclePrefab, hit.point + Vector3.up * 0.01f, Quaternion.Euler(90, 0, 0), hit.transform);
        }
    }

    public void InteractAction(InputAction.CallbackContext obj) {
        if(!obj.performed) { return; }

        Collider[] nearbyInteractables = Physics.OverlapSphere(transform.position, 1, 1 << 10 | 1 << 8, QueryTriggerInteraction.Collide);
        
        float closestDist = Mathf.Infinity;
        Collider closestCollider = null;

        for (int i = 0; i < nearbyInteractables.Length; i++) {
            float dist = (nearbyInteractables[i].transform.position - transform.position).magnitude;

            if(dist < closestDist) {
                closestDist = dist;
                closestCollider = nearbyInteractables[i];
            }
        }

        if (!closestCollider) { return; }

        Item item = closestCollider.GetComponent<Item>();
        SummoningCircle circle = closestCollider.GetComponent<SummoningCircle>();

        if(item) {
            inventory.PickupItem(item);
            item.transform.parent = transform.GetChild(2);
        }

        if(circle) {
            circle.Activate();
        }
    }

    public void DropAction(InputAction.CallbackContext obj) {
        if(!obj.performed) { return; }

        Item droppedItem = inventory.DropItem();

        if(!droppedItem) { return; }
        
        // Calc distance to nearest summoning circle
        Collider[] summoningCircles = Physics.OverlapSphere(transform.position, 1, 1 << 10, QueryTriggerInteraction.Collide);
        
        float closestDist = Mathf.Infinity;
        Transform closestNode = null;
        int index = -1;

        for (int i = 0; i < summoningCircles.Length; i++) {
            for (int j = 0; j < 4; j++) {
                float dist = (summoningCircles[i].transform.GetChild(2).GetChild(j).position - transform.position).magnitude;

                if(dist < closestDist && summoningCircles[i].GetComponent<SummoningCircle>().SpotFree(j)) {
                    index = j;
                    closestDist = dist;
                    closestNode = summoningCircles[i].transform.GetChild(2).GetChild(j);
                }
            }
        }

        if(closestNode != null) {
            droppedItem.transform.parent = closestNode;

            closestNode.parent.parent.GetComponent<SummoningCircle>().FillSpot(index, droppedItem);

            droppedItem.transform.localPosition = new Vector3(0, 0, -1);

            droppedItem.GetComponent<Rigidbody>().isKinematic = true;
        } else {
            droppedItem.transform.parent = null;
        }
    }
}