using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LaunchGameButton : NetworkBehaviour
{
    public string GameSceneName = "Game";
    // Start is called before the first frame update
    void Start()
    {
        if (!IsServer) { 
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void LaunchGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
