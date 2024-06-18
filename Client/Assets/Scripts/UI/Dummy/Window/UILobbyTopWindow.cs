using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Flux.UI.Window
{
    // 로비 상단 UI (백버튼, 홈버튼 등 존재)
    public class UILobbyTopWindow : UIWindow
    {
        [SerializeField] private GameObject _buttonGroup;
        [SerializeField] private TextMeshProUGUI _windowNameText;
        
        public void ShowLayout()
        {
            UIManager.Instance.onEventChangeWindow = OnEventChangeWindow;
        }

        private void OnEventChangeWindow(eWINDOW_TYPE windowType)
        {
            // 더미 표기 진행(테이블을 이용한 파싱형태로 커스터 마이징 필요(로컬라이징))
            switch (windowType)
            {
                case eWINDOW_TYPE.Lobby:
                    _buttonGroup.SetActive(false);
                    _windowNameText.text = "Lobby";
                    break;
                case eWINDOW_TYPE.Inventory:
                    _buttonGroup.SetActive(true);
                    _windowNameText.text = "Inventory";
                    break;
                case eWINDOW_TYPE.Character:
                    _buttonGroup.SetActive(true);
                    _windowNameText.text = "Character";
                    break;
                case eWINDOW_TYPE.None:
                case eWINDOW_TYPE.Max:
                default:
                    break;
            }
        }

        public void OnTouchBackButton()
        {
            UINavigator.Instance.OnProcessBackButton();
        }
        public void OnTouchHomeButton()
        {
            UINavigator.Instance.OnProcessHomeButton();
        }
        
        #region override
        public override void OnRefresh()
        {
            Debug.Log($"{this.GetType().Name} : OnRefresh");

            // Todo : Do Something
            UIManager.Instance.onEventChangeWindow?.Invoke(windowType);
        }
        public override void OnClose()
        {
            Debug.Log($"{this.GetType().Name} : OnClose");
            
            // Todo : Do Something
        }
        #endregion            
    }
}
