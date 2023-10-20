using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Finish : NetworkBehaviour
{
    private NetworkList<FixedString32Bytes> playerTimes = new();
    private NetworkList<FixedString32Bytes> playerIds = new();
    private GameObject player;
    private Countdown Countdown;
    private float time;
    public GameObject UIClassement;
    public TextMeshProUGUI textClassement;
    private string classementServer;
    private string OwnerId;

    private NetworkVariable<bool> FirstPlayerFinished = new(false);

    public override void OnNetworkSpawn()
    {
        FirstPlayerFinished.Value = false;
        UIClassement.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (IsOwner && other.tag == "Body")
        {
            player = other.gameObject;
            player.GetComponent<Rigidbody>().isKinematic = true;

            Countdown = GameObject.Find("Timer").GetComponent<Countdown>();
            time = Countdown.timer;

            int minutes = Mathf.FloorToInt(time / 60F);
            int seconds = Mathf.FloorToInt(time - minutes * 60);

            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            var text = player.transform.root.transform.Find("Body/PlayerHUD/Background/Name").gameObject.GetComponent<TextMeshProUGUI>().text;
            OwnerId = text.Substring(text.IndexOf(' ') + 1);

            if(!playerIds.Contains(OwnerId))
            {
                SaveTimeServerRpc(niceTime, OwnerId);
            }
        }
    }

    [ServerRpc(RequireOwnership =false)]
    private void SaveTimeServerRpc(string time, string playerId)
    {
        playerTimes.Add(time);
        playerIds.Add(playerId);

    }

    private void Update()
    {
        if (IsServer) {
            if (playerIds.Count == 1 && FirstPlayerFinished.Value == false)
            {
                StartCoroutine(TimeEnd());
                FirstPlayerFinished.Value = true;
            }
        }
    }

    IEnumerator TimeEnd()
    {
        yield return new WaitForSeconds(5);

        for (int i = 0; i < playerIds.Count; i++)
        {
            classementServer = classementServer + "Player " + playerIds[i] + " : " + playerTimes[i] + "<br>";
        }

        DisplayEndClientRpc(classementServer);
    }

    [ClientRpc]
    void DisplayEndClientRpc(string classementServer)
    {
        UIClassement.SetActive(true);

        //var player = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject;

        //player.GetComponentInChildren<Rigidbody>().isKinematic = true;

        textClassement.text = classementServer; 
    }
}
