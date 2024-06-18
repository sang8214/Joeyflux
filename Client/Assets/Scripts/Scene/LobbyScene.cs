using System;
using UnityEngine;
using Flux.UI;
using Flux.UI.Window;

namespace Flux.Scene
{
    public class LobbyScene : MonoBehaviour
    {
        void Start()
        {
            if (UIManager.Instance.TryGetUIWindow<UILobbyTopWindow>(out var lobbyTopWindow))
            {
                lobbyTopWindow.gameObject.SetActive(true);
                lobbyTopWindow.ShowLayout();
            }

            if (UINavigator.Instance.TryOpenWindow<UILobbyWindow>(out var lobbyWindow))
                lobbyWindow.ShowLayout();
        }

        private void Update()
        {
            // BackButton Test
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                UINavigator.Instance.OnProcessBackButton();
            }
        }
    }
}
