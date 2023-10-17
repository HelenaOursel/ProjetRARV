using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

// Use action set asset instead of lose InputActions directly on component.
[RequireComponent(typeof(CharacterController))]
public class MovementController : NetworkBehaviour
{
    private CharacterController controller;
    private Vector2 moveInput;
    public float speed;

    private Vector3 velocity;
    private bool grounded;
    public float gravity = -9.8f;
    public float jumpForce = 2f;

    public Camera cam;
    private Vector2 lookPosition;
    private float xRotation = 0f;
    public float xSens = 30f;
    public float ySens = 30f;

    private bool cursorLock = true;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookPosition = context.ReadValue<Vector2>();
    }

    public void OnCursorLockToggle(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (cursorLock == true)
        {
            Cursor.lockState = CursorLockMode.Confined;
            cursorLock = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            cursorLock = true;
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<AudioListener>().enabled = true;
            GetComponent<CharacterController>().enabled = true;
            controller = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            cam.gameObject.SetActive(true);
            return;
        }
        cam.gameObject.SetActive(false);


    }

    private void Update()
    {
        if (IsOwner)
        {
            grounded = controller.isGrounded;
            Move();
            Look();
        }
    }

    public void Move()
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = moveInput.x;
        moveDirection.z = moveInput.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        if (grounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (grounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -3f * gravity);
        }
    }

    public void Look()
    {
        xRotation -= (lookPosition.y * Time.deltaTime) * ySens;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * (lookPosition.x * Time.deltaTime) * xSens);
    }


}