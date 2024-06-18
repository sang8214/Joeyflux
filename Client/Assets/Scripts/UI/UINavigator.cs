using System;
using System.Collections.Generic;
using UnityEngine;
using Flux.CommonInstance;

namespace Flux.UI
{
    public class UINavigator : MonoSingleton<UINavigator>
    {
        private Stack<UIWindow> _windowStack = new();
        private Stack<UIPopup> _popupStack = new();

        public UIWindow CurrentWindow => _windowStack.TryPeek(out var currWindow) ? currWindow : null;

        // UI 윈도우 오픈
        public bool TryOpenWindow<T>(out T uiWindow, bool clearStack = false) where T : UIWindow
        {
            uiWindow = null;

            if (clearStack)
            {
                while (_windowStack.Count > 0)
                    CloseWindow();
            }

            // 현재 활성화된 윈도우와 동일할 경우
            if (_windowStack.Count > 0 && CurrentWindow is T currentWindow)
            {
                uiWindow = currentWindow;
                return true;
            }

            if (!UIManager.Instance.TryGetUIWindow<T>(out var target))
                return false;

            // 활성화 되기전 전처리(이후 3D모델링 로드 및 네트워크 통신 필요시 Async방식으로 처리되어야 함)
            target.OnPreEnable();

            // 이전상태의 윈도우 비활성화 및 새로운 윈도우 활성화        
            if (_windowStack.TryPeek(out var uiPrevWindow))
            {
                uiPrevWindow.OnClose();
                uiPrevWindow.gameObject.SetActive(false);
                target.gameObject.SetActive(true);
                _windowStack.Push(target);
            }
            else
            {
                target.gameObject.SetActive(true);
                _windowStack.Push(target);
            }

            uiWindow = target;
            return true;

        }

        private void CloseWindow()
        {
            if (_windowStack.Count == 0)
                return;

            if (!_windowStack.TryPop(out var uiWindow))
                return;

            uiWindow.OnClose();
            uiWindow.gameObject.SetActive(false);

            if (!_windowStack.TryPeek(out var uiPrevWindow))
                return;

            uiPrevWindow.OnPreEnable();
            uiPrevWindow.OnRefresh();
            uiPrevWindow.gameObject.SetActive(true);
        }

        #region ui_popup

        public bool TryOpenPopup<T>(out T uiPopup, bool clearStack = false) where T : UIPopup
        {
            uiPopup = null;

            if (clearStack)
            {
                while (_popupStack.Count > 0)
                    ClosePopup();
            }

            // UIManager에서 타겟 팝업 가져옴
            if (!UIManager.Instance.TryGetUIPopup<T>(out var target))
                return false;

            target.OnPreEnable();
            target.gameObject.SetActive(true);
            _popupStack.Push(target);
            uiPopup = target;

            return true;
        }

        private void ClosePopup()
        {
            if (_popupStack.Count == 0)
                return;

            if (!_popupStack.TryPop(out var uiPopup))
                return;

            // 팝업 오브젝트 파괴
            uiPopup.OnClose();
            // UIManager를 통해 해당 오브젝트 반환
            UIManager.Instance.ReturnUIPopup(uiPopup);

            if (!_popupStack.TryPeek(out var uiPrevPopup))
                return;

            // 팝업의 경우는 Depth상 뒤에 있을 뿐 활성화 되어 있으므로 OnPreEnable 불필요
            //prePopup.OnPreEnable();
            uiPrevPopup.OnRefresh();
        }

        #endregion

        #region exteral_command

        public void OnProcessBackButton()
        {
            // 백버튼으로 인한 제어 처리(팝업을 먼저 닫고, 윈도우를 닫음)
            if (_popupStack.Count > 0)
                ClosePopup();
            else if (_windowStack.Count > 1)
                CloseWindow();
        }

        public void OnProcessHomeButton()
        {
            // 홈버튼으로 인한 제어 처리(메인창으로 이동)
            TryOpenWindow<Window.UILobbyWindow>(out var uiWindow, true);
        }

        #endregion
    }
}
