using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static UnityEngine.GraphicsBuffer;

public class Turret : NetworkBehaviour
{
    private GameObject player;
    private PlayerController controller;
    public float speedRotation;

    public GameObject rotator;
    [SerializeField] float shootDelay = 1f;
    private float T_ShootDelay;

    private NetworkVariable<bool> isTrigger = new();

    public GameObject projectile;
    public GameObject canon;
    public float launchVelocity;

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            isTrigger.Value = false;
            T_ShootDelay = shootDelay;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Body" && isTrigger.Value == false)
        {
            if(IsOwner)
            {
                controller = other.gameObject.transform.root.gameObject.GetComponent<PlayerController>();
                IsTriggerServerRpc(controller);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void IsTriggerServerRpc(NetworkBehaviourReference playerReference)
    {
        if (playerReference.TryGet<PlayerController>(out PlayerController controllerRef))
        {
            controller = controllerRef.GetComponent<PlayerController>();
            isTrigger.Value = !isTrigger.Value;
        }
        else
        {
            Debug.LogError("Didn't get shoot");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (IsOwner)
        {
            if (other.tag == "Body" && isTrigger.Value == true && IsServer)
            {
                controller = other.gameObject.transform.root.gameObject.GetComponent<PlayerController>();
                IsTriggerServerRpc(controller);
            }
        }
    }

    private void LateUpdate()
    {
        if (isTrigger.Value == true && IsServer)
        {
            ShootClientRpc(controller);
        }
    }

    [ClientRpc]
    private void ShootClientRpc(NetworkBehaviourReference playerReference)
    {
        if (playerReference.TryGet<PlayerController>(out PlayerController player))
        {
            Vector3 targetPostition = new Vector3(player.transform.position.x,
                                        transform.position.y + 0.5f,
                                        player.transform.position.z);
            rotator.transform.LookAt(targetPostition);

            if (T_ShootDelay < shootDelay)
            {
                T_ShootDelay += Time.deltaTime;
            }

            if (T_ShootDelay >= shootDelay)
            {
                T_ShootDelay = 0;
                GameObject ball = Instantiate(projectile, canon.transform.position,
                                                         canon.transform.rotation);
                ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, launchVelocity, 0));
            }
        }
    }
}
