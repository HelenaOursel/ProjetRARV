using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

// Use action set asset instead of lose InputActions directly on component.
public class PlayerController : NetworkBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5;
    public bool isOnGround = true;
    private float horizontalInput;
    private float forwardInput;
    private Rigidbody playerRb;
    private GameObject body;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            playerRb = GetComponent<Rigidbody>();
            body = transform.Find("Body").gameObject;
        }
    }
    private void Update()
    {
        if(IsOwner)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            forwardInput = Input.GetAxis("Vertical");

            transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
            transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);

            Vector3 movement = new Vector3(horizontalInput, 0.0f, forwardInput);
            body.transform.rotation = Quaternion.LookRotation(movement);

            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsOwner)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isOnGround = true;
            }
        }
    }
}
