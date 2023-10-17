using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ZoneController : NetworkBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (IsLocalPlayer)
        {
            Debug.Log("Je suis local");
            var player = other.gameObject;
            player.transform.position = new Vector3(0f, 1f, 0f);
        }
    }
}
