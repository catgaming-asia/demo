using System;
using System.Collections;
using System.Collections.Generic;
using CoreGame;
using Game.Command;
using Game.Player;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private NetworkManager _networkManagerHostClient;
        [SerializeField] private NetworkManager _networkManagerServer;
        [SerializeField] private NetworkManager _networkManager;
        public NetworkObject Parent;
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;
        [SerializeField] private Button _serverButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private CanvasGroup _buttonGroup;

        [SerializeField] private TMP_Text _title;

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
            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(Back);
            HideGamePlay();
        }

        private void Back()
        {
            HideGamePlay();
            _networkManager.Shutdown();
            Destroy(_networkManager.gameObject);
        }

        private void StartHost()
        {
            ShowGamePlay("Host");
            _networkManager = new LoadNetworkCommand().Execute(_networkManagerHostClient);
            _networkManager.StartHost();
        }

        private void StartClient()
        {
            ShowGamePlay("Client");
            _networkManager = new LoadNetworkCommand().Execute(_networkManagerHostClient);
            _networkManager.StartClient();
        }

        private void StartServer()
        {
            ShowGamePlay("Server");
            _networkManager = new LoadNetworkCommand().Execute(_networkManagerServer);
            _networkManager.StartServer();
        }

        private void ShowGamePlay(string title)
        {
            _buttonGroup.EnableCanvasGroup(false);
            _backButton.gameObject.SetActive(true);
            _title.gameObject.SetActive(true);
            _title.text = title;
        }

        private void HideGamePlay()
        {
            _buttonGroup.EnableCanvasGroup(true);
            _backButton.gameObject.SetActive(false);
            _title.gameObject.SetActive(false);
        }
    }
}