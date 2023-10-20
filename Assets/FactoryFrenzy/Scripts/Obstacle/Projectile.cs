using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 3);
    }
}
