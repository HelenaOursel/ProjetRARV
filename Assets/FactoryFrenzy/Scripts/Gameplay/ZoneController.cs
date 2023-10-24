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
            var body = other.gameObject.GetComponentInChildren<Rigidbody>();
            var player = other.transform.root.gameObject;

            body.isKinematic = true;

            Debug.Log(player.GetComponent<PlayerController>().GetLastCheckpoint());

            Vector3 checkpoint = player.GetComponent<PlayerController>().GetLastCheckpoint();

            player.transform.position = checkpoint;
            body.MovePosition(player.transform.position);

            StartCoroutine(Place(body));
        }
    }

    IEnumerator Place(Rigidbody body)
    {
        yield return new WaitForSeconds(1f);
        body.isKinematic = false;
    }
}
