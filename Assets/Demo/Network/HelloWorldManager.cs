using CoreGame;
using HelloWorld;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Network
{
    public class HelloWorldManager : MonoBehaviour
    {
        public NetworkManager m_NetworkManager;

        public TMP_Text Text;
        void Awake()
        {
            Locator<HelloWorldManager>.Set(this);
            m_NetworkManager = GetComponent<NetworkManager>();
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!m_NetworkManager.IsClient && !m_NetworkManager.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host"))Locator<HelloWorldManager>.Instance. m_NetworkManager.StartHost();
            if (GUILayout.Button("Client"))Locator<HelloWorldManager>.Instance. m_NetworkManager.StartClient();
            if (GUILayout.Button("Server"))Locator<HelloWorldManager>.Instance. m_NetworkManager.StartServer();
        }

        static void StatusLabels()
        {
            var mode = Locator<HelloWorldManager>.Instance.m_NetworkManager.IsHost ?
                "Host" :Locator<HelloWorldManager>.Instance. m_NetworkManager.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                            Locator<HelloWorldManager>.Instance.m_NetworkManager.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        static void SubmitNewPosition()
        {
           
            if (GUILayout.Button(Locator<HelloWorldManager>.Instance.m_NetworkManager.IsServer ? "Move" : "Request Position Change"))
            {
                if (Locator<HelloWorldManager>.Instance.m_NetworkManager.IsServer && !Locator<HelloWorldManager>.Instance.m_NetworkManager.IsClient )
                {
                    Debug.Log("SubmitNewPosition isServer");
                    foreach (ulong uid in Locator<HelloWorldManager>.Instance.m_NetworkManager.ConnectedClientsIds)
                        Locator<HelloWorldManager>.Instance.m_NetworkManager.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HelloWorldPlayer>().Move();
                }
                else
                {
                    Debug.Log("SubmitNewPosition isClient");
                    var playerObject = Locator<HelloWorldManager>.Instance.m_NetworkManager.SpawnManager.GetLocalPlayerObject();
                    var player = playerObject.GetComponent<HelloWorldPlayer>();
                    player.Move();
                }
            }
        }
    }
}