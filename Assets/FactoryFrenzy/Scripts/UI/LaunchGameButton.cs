using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LaunchGameButton : NetworkBehaviour
{
    public string GameSceneName = "Game";
    public string GameSceneWithJSON = "GameLoadScene";
    public LevelSelect LevelSelectScript;

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
        if (LevelSelectScript.GetLevelNumber() == 1)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(GameSceneWithJSON, UnityEngine.SceneManagement.LoadSceneMode.Single);

        } else
        {
            NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
