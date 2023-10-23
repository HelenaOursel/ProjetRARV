using System.Text.RegularExpressions;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UIElements;

namespace PropHunt.UI
{
    public class MainMenu : MonoBehaviour
    {
        public Button m_hostButton;
        public Button m_clientButton;

        [SerializeField] public TMP_InputField m_ipAddress;
        [SerializeField] public TMP_InputField m_port;

        void Awake()
        {
            m_ipAddress.text = "127.0.0.1";
            m_port.text = "9990";

        }
        /// <summary>
        /// Use sanitized IP and Port to set up the connection.
        /// </summary>
        void SetUtpConnectionData()
        {

            ushort.TryParse(m_port.text, out var port);

            var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            utp.SetConnectionData(m_ipAddress.text, port);
        }

        /// <summary>
        /// Starts the host using the given connection data.
        /// </summary>
        public void StartHost()
        {
            SetUtpConnectionData();
            var result = NetworkManager.Singleton.StartHost();
            if (result)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("Lobby", UnityEngine.SceneManagement.LoadSceneMode.Single);
                return;
            }
        }


        /// <summary>
        /// Starts the Client using the given connection data.
        /// </summary>
        public void StartClient()
        {
            SetUtpConnectionData();
            NetworkManager.Singleton.StartClient();
        }

    }
}