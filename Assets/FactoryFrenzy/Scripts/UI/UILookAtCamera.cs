using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
        transform.Rotate(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z);
    }
}
