using System.Collections;
using System.Collections.Generic;
using Flux.UI.Component;
using Flux.UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace Flux.UI.Window
{
    public class UIInventoryWindow : UIWindow
    {
        public void ShowLayout()
        {
            Debug.Log($"{this.GetType().Name} : ShowLayout");
            OnRefresh();
        }

        public void OnTouchCharacter()
        {
            if (UINavigator.Instance.TryOpenWindow<UICharacterWindow>(out var uiInventory))
                uiInventory.ShowLayout();
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