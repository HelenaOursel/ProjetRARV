using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.GraphicsBuffer;

public class Turret : NetworkBehaviour
{
    private GameObject player;
    private NetworkObject controller;
    public float speedRotation;

    public GameObject rotator;
    [SerializeField] float shootDelay = 1f;
    private float T_ShootDelay;

    private NetworkVariable<bool> isTrigger = new();

    public GameObject projectile;
    public GameObject canon;
    public float launchVelocity;

    private AudioSource audioSource;

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            isTrigger.Value = false;
        }
        T_ShootDelay = shootDelay;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Body" && isTrigger.Value == false)
        {
            if(IsOwner)
            {
                controller = other.gameObject.transform.root.gameObject.GetComponent<NetworkObject>();
                IsTriggerServerRpc(controller);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void IsTriggerServerRpc(NetworkObjectReference playerReference)
    {
        if (playerReference.TryGet(out NetworkObject controllerRef))
        {
            controller = controllerRef;
            isTrigger.Value = !isTrigger.Value;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsOwner)
        {
            if (other.tag == "Body" && isTrigger.Value == true && IsServer)
            {
                controller = other.gameObject.transform.root.gameObject.GetComponent<NetworkObject>();
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
        else if(isTrigger.Value == false && IsServer)
        {
            SearchTargetClientRpc();
        }
    }

    [ClientRpc]
    private void SearchTargetClientRpc()
    {
        rotator.transform.Rotate(Vector3.up * Time.deltaTime * 15, Space.Self);
    }

    [ClientRpc]
    private void ShootClientRpc(NetworkObjectReference playerReference)
    {
        if (playerReference.TryGet(out NetworkObject player))
        {
            var body = player.transform.Find("Body").gameObject;

            Vector3 targetPostition = new Vector3(body.transform.position.x,
                                        transform.position.y + 0.5f,
                                        body.transform.position.z);

            Vector3 targetDir = targetPostition - rotator.transform.position;
            targetDir.y = 0;
            float step = 5 * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(rotator.transform.forward, targetDir, step, 0.0F);

            rotator.transform.rotation = Quaternion.LookRotation(newDir);

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
                audioSource.Play();
            }
        }
    }
}
