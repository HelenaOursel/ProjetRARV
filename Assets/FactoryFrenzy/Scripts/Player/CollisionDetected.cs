using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CollisionDetected : NetworkBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<PlayerController>().CollisionDetected(collision);
    }

    //private void OnCollisionStay(Collision other)
    //{
    //    if(IsOwner)
    //    {
    //        if (other.gameObject.tag == "Platform")
    //        {
    //            ChangeParentServerRpc(other.gameObject, gameObject);
    //        }
    //    }
    //}
    //private void OnCollisionExit(Collision other)
    //{
    //    if(IsOwner)
    //    {
    //        if (other.gameObject.tag == "Platform")
    //        {
    //            NullParentServerRpc(gameObject);
    //        }
    //    }
    //}

    [ServerRpc]
    private void ChangeParentServerRpc(NetworkObjectReference other, NetworkObjectReference player)
    {
        if (other.TryGet(out NetworkObject platform) && player.TryGet(out NetworkObject playerOut))
        {
            playerOut.transform.parent = platform.transform;
        }

    }

    [ServerRpc]
    private void NullParentServerRpc(NetworkObjectReference player)
    {
        if (player.TryGet(out NetworkObject playerOut))
        {
            playerOut.transform.parent = null;
        }
    }
}
