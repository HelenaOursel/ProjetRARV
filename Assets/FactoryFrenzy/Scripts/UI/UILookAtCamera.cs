using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UILookAtCamera : NetworkBehaviour
{
    public Camera Camera;
    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.transform.forward);
        transform.Rotate(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z);
    }
}
