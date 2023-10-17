using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
namespace PropHunt.Gameplay
{
    public class PlayerPlacer : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            //StartCoroutine(Place());
        }
        IEnumerator Place()
        {
            yield return new WaitForSeconds(0.5f);
            //Place the player on the position 0,0,0. Only orks because the position is client authoritative
            NetworkManager.Singleton.LocalClient.PlayerObject.transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
            NetworkManager.Singleton.LocalClient.PlayerObject.GetComponentInChildren<Rigidbody>().useGravity = true;
        }
    }
}