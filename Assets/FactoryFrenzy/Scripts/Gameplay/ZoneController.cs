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
            Debug.Log("CC");
            var body = other.gameObject.GetComponentInChildren<Rigidbody>();
            var player = other.transform.root.gameObject;

            body.velocity = Vector3.zero;
            body.isKinematic = true;

            player.transform.position = new Vector3(0f, 1f, 0f);
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
