using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour {
    public float walkSpeed = 10, sprintSpeed = 20, crouchSpeed = 5, jumpHeight = 1.5f, gravForce = 19.6f;

    public GameObject model;
    public GameObject summoningCirclePrefab;

    Quaternion movementRotation;
    Quaternion slerpPoint = Quaternion.Euler(0, 0, 0);

    Vector2 playerMovementVector = Vector2.zero;
    Vector2 playerLookVector = Vector2.zero;
    Vector2 camVals = Vector2.zero;
    Vector3 cameraGimbal = Vector3.zero;

    CharacterController controller;
    Animator animator;
    SpriteRenderer renderer;

    Inventory inventory;

    bool sprinting;
    bool crouching;
    float gravity = 0;
    float lastGroundedElevation = 0f;

    void Awake() {
        EnsureComponentsExist();
    }

    void Update() {
        EnsureComponentsExist();

        Camera.main.transform.parent = null;
        CameraControl();
        SetCameraGimbal();
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
        //FixRotation();

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
        camVals.y = Mathf.Clamp(camVals.y, 0f, 1.5f);
    }

    void FixRotation() {
        Vector3 cameraPos = new Vector3();

        cameraPos.x = Mathf.Cos(camVals.x) * 1.5f;
        cameraPos.z = Mathf.Sin(camVals.x) * 1.5f;
        cameraPos.y = camVals.y;

        Vector3 lookPos = Vector3.ProjectOnPlane(transform.position + cameraGimbal, Vector3.up) + Vector3.up * cameraGimbal.y;
        Camera.main.transform.position = lookPos + (cameraPos * (2.5f + (0.5f * cameraPos.y * cameraPos.y)));

        Vector3 lookDir = lookPos - Camera.main.transform.position;
        Camera.main.transform.rotation = Quaternion.LookRotation(lookDir, transform.up);

        model.transform.parent.localEulerAngles = new Vector3(
            0, 
            Camera.main.transform.rotation.eulerAngles.y, 
            0
        );
    }

    void SetCameraGimbal()
    {
        float length = 0.5f;
        if (sprinting) length = 1.5f;
        if (crouching) length = 3f;

        Vector3 hGimbal = new Vector3(playerMovementVector.x, 0f, playerMovementVector.y).normalized;
        hGimbal = movementRotation * transform.forward * length;

        if (controller.isGrounded) lastGroundedElevation = transform.position.y;
        float depth = Mathf.Min(transform.position.y, lastGroundedElevation) + 0.75f;
        if (Mathf.Abs(transform.position.y - lastGroundedElevation) > 7.5f) depth = transform.position.y + 0.75f;

        float lerpY = Mathf.Min(Mathf.Lerp(cameraGimbal.y, depth, 3f * Time.deltaTime), transform.position.y + 0.75f);
        cameraGimbal = new Vector3(cameraGimbal.x, lerpY, cameraGimbal.z);
        cameraGimbal = Vector3.Lerp(cameraGimbal, hGimbal + Vector3.up * cameraGimbal.y, 1f * Time.deltaTime);
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
        gravity = -Mathf.Sqrt(2f * gravForce * jumpHeight);
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
