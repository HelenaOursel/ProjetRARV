using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Countdown : NetworkBehaviour
{
    public NetworkVariable<float> timeRemaining;
    public bool isStarted = false;
    public NetworkVariable<float> timer;
    public TextMeshProUGUI count;
    public GameObject block;

    private AudioSource audioSource;
    private bool AudioIsPlayed = false;

    public override void OnNetworkSpawn()
    {
        timer.Value = 0;
        timeRemaining.Value = 10;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(isStarted == false)
        {
            if (timeRemaining.Value > 0)
            {
                if (IsServer)
                {
                    timeRemaining.Value -= Time.deltaTime;
                }

                int minutes = Mathf.FloorToInt(timeRemaining.Value / 60F);
                int seconds = Mathf.FloorToInt(timeRemaining.Value - minutes * 60);

                string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
                count.text = niceTime;

                if(timeRemaining.Value < 3 && AudioIsPlayed == false) 
                {
                    audioSource.Play();
                    AudioIsPlayed=true;
                }
            }
            else
            {
                count.text = "Go !";
                block.SetActive(false);

                if (IsServer)
                {
                    timer.Value += Time.deltaTime;

                    //Block le joueur avant le start
                    //ChangeMoveClientRpc();
                }

                StartCoroutine(Timer());
            }
        }
        else
        {
            if (IsServer)
            {
                timer.Value += Time.deltaTime;
            }

            int minutes = Mathf.FloorToInt(timer.Value / 60F);
            int seconds = Mathf.FloorToInt(timer.Value - minutes * 60);

            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            count.text = niceTime;
        }
       
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(3);
        isStarted = true;
    }

    [ClientRpc]
    private void ChangeMoveClientRpc()
    {
        var player = NetworkManager.LocalClient.PlayerObject;
        player.GetComponent<PlayerController>().canMove = true;
    }
}
