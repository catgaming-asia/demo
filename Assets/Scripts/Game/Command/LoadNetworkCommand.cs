using CoreGame;
using Unity.Netcode;
using UnityEngine;

namespace Game.Command
{
    public class LoadNetworkCommand
    {
        GameManager _gameManager=> Locator<GameManager>.Instance;
        public NetworkManager Execute(NetworkManager networkManager)
        {
            var networkMng = GameObject.Instantiate(networkManager);
            return networkMng;
        }
    }
}