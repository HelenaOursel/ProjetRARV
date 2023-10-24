using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class LevelSelect : NetworkBehaviour
{
    public TextMeshProUGUI LevelText;
    private int nbLevel = 0;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        LevelText.text = nbLevel.ToString() + " - Defaut";
    }

    public void OnUpClick()
    {
        if(IsServer)
        {
            nbLevel++;
            OnClientRpc(nbLevel.ToString());
        }
    }

    public void OnDownClick()
    {
        if (IsServer && nbLevel != 0)
        {
            nbLevel--;
            if(nbLevel == 0)
            {
                OnClientRpc((nbLevel.ToString() + " - Defaut"));
            }
            else
            {
                OnClientRpc(nbLevel.ToString());
            }
        }
    }

    [ClientRpc]
    public void OnClientRpc(string text)
    {
        LevelText.text = text;
    }
}
