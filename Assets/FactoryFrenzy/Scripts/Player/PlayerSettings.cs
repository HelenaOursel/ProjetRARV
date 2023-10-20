using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;

public class PlayerSettings : NetworkBehaviour
{
    [SerializeField] private GameObject body;
    private MeshRenderer meshRenderer;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private Camera cam;
    private NetworkVariable<FixedString128Bytes> networkPlayerName = new NetworkVariable<FixedString128Bytes>(
        "Player : 0", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public List<Color> colors = new List<Color>();

    private void Awake()
    {
        meshRenderer = body.GetComponent<MeshRenderer>();
    }

    public override void OnNetworkSpawn()
    {
        networkPlayerName.Value = "Player: " + (OwnerClientId + 1);
        playerName.text = networkPlayerName.Value.ToString();

        meshRenderer.material.color = colors[(int)OwnerClientId];
    }
}
