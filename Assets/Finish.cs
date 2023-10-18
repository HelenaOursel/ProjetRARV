using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Finish : NetworkBehaviour
{
    private GameObject player;
    private Countdown Countdown;
    private float time;
    private void OnTriggerEnter(Collider other)
    {
        if (IsOwner)
        {
            player = other.gameObject;

            Countdown = GameObject.Find("Timer").GetComponent<Countdown>();
            time = Countdown.timer;

            player.GetComponentInChildren<Rigidbody>().isKinematic = true;

            int minutes = Mathf.FloorToInt(time / 60F);
            int seconds = Mathf.FloorToInt(time - minutes * 60);

            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            Debug.Log(niceTime);
        }
    }
}
