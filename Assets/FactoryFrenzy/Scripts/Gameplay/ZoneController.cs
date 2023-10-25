using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Body")
        {
            var player = other.transform.root.gameObject;
            var playerRb = player.GetComponent<Rigidbody>();

            Vector3 checkpointPosition = player.GetComponent<PlayerController>().GetLastCheckpoint();

            playerRb.isKinematic = true;

            player.transform.position = checkpointPosition;//new Vector3(0f, 1f, 0f);

            StartCoroutine(Place(playerRb));
        }
    }

    IEnumerator Place(Rigidbody playerRb)
    {
        yield return new WaitForSeconds(1f);
        playerRb.isKinematic = false;
    }
}
