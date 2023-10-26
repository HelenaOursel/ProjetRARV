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
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : NetworkBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 15;
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

    private Vector2 m_v2Move;
    private Vector2 m_v2Rotation;
    private bool m_bIsJump = false;
    private PlayerInput input;
    private Vector3 m_v3MoveDirection = Vector3.zero;
    public float Gravity = 20.0f;


    private List<Vector3> checkpoints = new List<Vector3>();
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
        input = GetComponent<PlayerInput>();
        input.enabled = false;

        if (IsOwner)
        {
            body = transform.Find("Body").gameObject;
            playerRb = GetComponent<Rigidbody>();
            Camera.gameObject.SetActive(true);
            audioListener.enabled = true;

            input = GetComponent<PlayerInput>();
            input.enabled = true;

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
            SetPositionClientRpc();
        }
    }

    [ClientRpc]
    private void SetPositionClientRpc()
    {
        transform.position = GameObject.Find("Spawn"+ (OwnerClientId+1)).transform.position;
        canMove = true;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_v2Move = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                m_bIsJump = true;
                break;
            case InputActionPhase.Canceled:
                m_bIsJump = false;
                break;
        }
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = speed * m_v2Move.y;
        float curSpeedY = speed * m_v2Move.x;
        float movementDirectionY = m_v3MoveDirection.y;
        m_v3MoveDirection = (forward * curSpeedX) + (right * curSpeedY); ;

        if (m_bIsJump && canMove == true && isOnGround == true)
        {
            m_v3MoveDirection.y = jumpForce;
        }
        else
        {
            m_v3MoveDirection.y = 0;
        }

        if (!isOnGround)
        {
            m_v3MoveDirection.y -= Gravity * Time.deltaTime;
        }

        if (canMove == true)
        {
            body.transform.rotation = Quaternion.LookRotation(new Vector3(m_v3MoveDirection.x, 0.0f, m_v3MoveDirection.z) * Time.deltaTime);
            //playerRb.MovePosition(transform.position + m_v3MoveDirection * Time.deltaTime);
            playerRb.position = transform.position + m_v3MoveDirection * Time.deltaTime;
        }

        //if(IsOwner)
        //{
        //    horizontalInput = Input.GetAxis("Horizontal");
        //    forwardInput = Input.GetAxis("Vertical");

        //    Vector3 movement = new Vector3(horizontalInput, 0.0f, forwardInput);

        //    if (movement.magnitude > 0)
        //    {
        //        body.transform.rotation = Quaternion.LookRotation(movement);
        //    }

        //    if(canMove == true)
        //    {
        //        transform.Translate(movement * speed * Time.deltaTime, Space.World);
        //    }

        //    if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        //    {
        //        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //        isOnGround = false;
        //    }

        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        Application.Quit();
        //    }

        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        Application.Quit();
        //    }
        //}
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
