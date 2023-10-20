using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CollisionDetected : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        transform.parent.GetComponent<PlayerController>().CollisionDetected(collision);
    }
}
