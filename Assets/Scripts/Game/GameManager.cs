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
        [SerializeField] private Button _backButton;
        [SerializeField] private CanvasGroup _buttonGroup;


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
            _buttonGroup.EnableCanvasGroup(true);
            _backButton.gameObject.SetActive(false);
        }

        private void Back()
        {
            _networkManager.Shutdown();
            _buttonGroup.EnableCanvasGroup(true);
            _backButton.gameObject.SetActive(false);
        }

        private void StartHost()
        {
            _networkManager.StartHost();
            _buttonGroup.EnableCanvasGroup(false);
            _backButton.gameObject.SetActive(true);
        }

        private void StartClient()
        {
            _networkManager.StartClient();
            _buttonGroup.EnableCanvasGroup(false);
            _backButton.gameObject.SetActive(true);
        }

        private void StartServer()
        {
            _networkManager.StartServer();
            _buttonGroup.EnableCanvasGroup(false);
            _backButton.gameObject.SetActive(true);
        }
    }
}