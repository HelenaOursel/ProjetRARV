using System;
using System.Collections.Generic;
using Unity.Collections;
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
    private Camera Camera;
    private AudioListener audioListener;

    private NetworkVariable<FixedString128Bytes> nameScene = new();
    private Countdown countdown;

    public bool canMove = true;


    private NetworkList<Vector3> checkpoints = new NetworkList<Vector3>();
    private int checkpointNumberPassed;

    private void Awake()
    {
        if (Camera == null) Camera = GetComponentInChildren<Camera>(true);
    }

    public override void OnNetworkSpawn()
    {
        Camera.gameObject.SetActive(false);
        audioListener = GetComponent<AudioListener>();
        audioListener.enabled = false;

        if (IsOwner)
        {
            body = transform.Find("Body").gameObject;
            playerRb = body.GetComponent<Rigidbody>();
            Camera.gameObject.SetActive(true);
            audioListener.enabled = true;

            Camera.gameObject.transform.position = new Vector3(0f, 2.2f, -3.3f);
            Camera.gameObject.transform.Rotate(14.5f, 0, 0);

        }

        if (IsServer)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        if(IsOwner )
        {
            Scene scene = SceneManager.GetActiveScene();

            if (scene.name == "Lobby")
            {
                transform.position = GameObject.Find("Spawn" + (OwnerClientId + 1)).transform.position;
                canMove = true;
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game" || scene.name == "Lobby")
        {
            CantMoveClientRpc();
            SetPositionClientRpc();
        }
    }

    [ClientRpc]
    private void CantMoveClientRpc()
    {
        canMove = false;
    }

    [ClientRpc]
    private void SetPositionClientRpc()
    {
        transform.position = GameObject.Find("Spawn"+ (OwnerClientId+1)).transform.position;
        canMove = true;
    }

    private void Update()
    {
        if(IsOwner)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            forwardInput = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontalInput, 0.0f, forwardInput);

            if (movement.magnitude > 0)
            {
                body.transform.rotation = Quaternion.LookRotation(movement);
            }

            if(canMove == true)
            {
                transform.Translate(movement * speed * Time.deltaTime, Space.World);
            }

            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
    public void CollisionDetected(Collision collision)
    {
        if (IsOwner)
        {
            if (collision.gameObject.CompareTag("Ground") == true || collision.gameObject.CompareTag("Platform") == true)
            {
                isOnGround = true;
            }
        }
    }


    public void SetCheckpointPassedNumber(int num)
    {
        this.checkpointNumberPassed = num;
    }

    public void InsertCheckpoint(Vector3 position)
    {
        this.checkpoints.Add(position);
    }

    public Vector3 GetLastCheckpoint()
    {
        if (this.checkpoints.Count != 0)
        {
            return this.checkpoints[checkpoints.Count - 1];
        }

        return new Vector3(0f, 1f, 0f);
    }
}
