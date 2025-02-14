using System;
using System.Collections;
using System.Collections.Generic;
using CoreGame;
using Game.Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private NetworkManager _networkManager;
        public NetworkManager NetworkManager => _networkManager;
        [SerializeField] public NetworkObject Parent;
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;
        [SerializeField] private Button _serverButton;
        [SerializeField] private CanvasGroup buttonGroup;

        public Button RandomButton;

        void Awake()
        {
            Locator<GameManager>.Set(this);
        }

        void Start()
        {
            _hostButton.onClick.RemoveAllListeners();
            _hostButton.onClick.AddListener(StartHost);
            _clientButton.onClick.RemoveAllListeners();
            _clientButton.onClick.AddListener(StartClient);
            _serverButton.onClick.RemoveAllListeners();
            _serverButton.onClick.AddListener(StartServer);
            buttonGroup.EnableCanvasGroup(true);
        }

        private void StartHost()
        {
            _networkManager.StartHost();
            buttonGroup.EnableCanvasGroup(false);
        }

        private void StartClient()
        {
            _networkManager.StartClient();
            buttonGroup.EnableCanvasGroup(false);
        }

        private void StartServer()
        {
            _networkManager.StartServer();
            buttonGroup.EnableCanvasGroup(false);
        }
      
    }
}