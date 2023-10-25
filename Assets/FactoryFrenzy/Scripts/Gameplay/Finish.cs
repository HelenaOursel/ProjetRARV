using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject backToLobby;

    public GameObject countdownTen;
    private NetworkVariable<float> TenSeconds = new();

    private int nbPlayer;

    private NetworkVariable<bool> FirstPlayerFinished = new(false);

    public override void OnNetworkSpawn()
    {
        TenSeconds.Value = 10f;
        FirstPlayerFinished.Value = false;
        UIClassement.SetActive(false);
        countdownTen.SetActive(false);

        if (!IsServer)
        {
            backToLobby.SetActive(false);
        }
        else
        {
            nbPlayer = NetworkManager.ConnectedClients.Count;
            backToLobby.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Body")
        {
            player = other.gameObject.transform.root.gameObject;
            Countdown = GameObject.Find("Timer").GetComponent<Countdown>();
            time = Countdown.timer.Value;

            int minutes = Mathf.FloorToInt(time / 60F);
            int seconds = Mathf.FloorToInt(time - minutes * 60);

            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            var text = player.transform.Find("Body/PlayerHUD/Background/Name").gameObject.GetComponent<TextMeshProUGUI>().text;
            OwnerId = text.Substring(text.IndexOf(' ') + 1);

            if (!playerIds.Contains(OwnerId))
            {
                StartCoroutine(StopMoving(player));
                //player.transform.root.transform.GetComponent<ParticleSystem>().Play();

                SaveTimeServerRpc(niceTime, OwnerId);
            }
        }
    }

    IEnumerator StopMoving(GameObject player)
    {
        yield return new WaitForSeconds(0.5f);
        player.gameObject.GetComponent<PlayerController>().canMove = false;
    }

    [ServerRpc(RequireOwnership =false)]
    private void SaveTimeServerRpc(string time, string playerId)
    {
        playerTimes.Add(time);
        playerIds.Add(playerId);
    }

    private void Update()
    {
        if (IsServer)
        {
            if (playerIds.Count == nbPlayer && FirstPlayerFinished.Value == false)
            {
                StartCoroutine(TimeEnd(1));
                FirstPlayerFinished.Value = true;
            }
            else if (playerIds.Count == 1 && FirstPlayerFinished.Value == false)
            {
                StartCoroutine(TimeEnd(10));
                FirstPlayerFinished.Value = true;
                TenSecondsClientRpc();

            }
            else if (FirstPlayerFinished.Value == true)
            {
                TenSeconds.Value -= Time.deltaTime;
            }
        }

        if (FirstPlayerFinished.Value == true && TenSeconds.Value > 0)
        {
            int minutes = Mathf.FloorToInt(TenSeconds.Value / 60F);
            int seconds = Mathf.FloorToInt(TenSeconds.Value - minutes * 60);

            string tenseconds = string.Format("{0:0}:{1:00}", minutes, seconds);

            countdownTen.GetComponent<TextMeshProUGUI>().text = tenseconds;
        }
    }

    [ClientRpc]
    private void TenSecondsClientRpc()
    {
        countdownTen.SetActive(true);
    }

    IEnumerator TimeEnd(int seconds)
    {
        yield return new WaitForSeconds(seconds);

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

        textClassement.text = classementServer; 
    }

    public void BackToLobby()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Lobby", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
