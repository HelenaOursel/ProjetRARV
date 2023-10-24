using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PingPong : NetworkBehaviour
{
    public Transform a, b;

    [Range(0, 1)]
    public float speed = 1;
    void Update()
    {
        if (IsServer)
        {
            var rb = GetComponent<Rigidbody>();
            float pingPong = Mathf.PingPong(Time.time * speed, 1);
            rb.transform.position = Vector3.Lerp(a.position, b.position, pingPong);
        }
    }
}
