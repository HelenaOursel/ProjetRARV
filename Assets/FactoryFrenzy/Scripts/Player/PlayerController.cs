using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.AddressableAssets.Build;
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
    private Camera Camera;

    public bool canMove = true;

    private void Awake()
    {
        if (Camera == null) Camera = GetComponentInChildren<Camera>(true);
    }

    public override void OnNetworkSpawn()
    {
        Camera.gameObject.SetActive(false);

        if (IsOwner)
        {
            body = transform.Find("Body").gameObject;
            playerRb = body.GetComponent<Rigidbody>();
            Camera.gameObject.SetActive(true);

            Camera.gameObject.transform.position = new Vector3(0f, 2.2f, -3.3f);
            Camera.gameObject.transform.Rotate(14.5f, 0, 0);

        }

        if (IsServer)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Lobby") return;
        SetPositionClientRpc();
    }

    [ClientRpc]
    private void SetPositionClientRpc()
    {
        transform.position = new Vector3(0f,1f,0f);
    }

    private void Update()
    {
        if(IsOwner && canMove == true)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            forwardInput = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontalInput, 0.0f, forwardInput);

            if (movement.magnitude > 0)
            {
                body.transform.rotation = Quaternion.LookRotation(movement);
            }

            transform.Translate(movement * speed * Time.deltaTime, Space.World);

            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
            }
        }
    }
    public void CollisionDetected(Collision collision)
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
