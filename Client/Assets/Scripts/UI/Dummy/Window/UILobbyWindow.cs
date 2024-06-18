using UnityEngine;

namespace Flux.UI.Window
{
    // 로비 메인 창
    public class UILobbyWindow : UIWindow
    {
        public void ShowLayout()
        {
            Debug.Log($"{this.GetType().Name} : ShowLayout");
            OnRefresh();
        }
        
        public void OnTouchInventory()
        {
            if (UINavigator.Instance.TryOpenWindow<UIInventoryWindow>(out var uiInventory))
                uiInventory.ShowLayout();
        }

        public void OnTouchDummy()
        {
            
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