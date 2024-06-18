using UnityEngine;

namespace Flux.UI.Window
{
    public class UILobbyWindow : UIWindow
    {
        public void OnTouchInventory()
        {
            if (UINavigator.Instance.TryOpenWindow<UICharacterWindow>(out var uiInventory))
                uiInventory.ShowLayout("Dummy Character");
        }
        
        #region override
        public override void OnRefresh()
        {
            // Todo : Do Something
            Debug.Log($"{this.GetType().Name} : OnRefresh");
        }
        public override void OnClose()
        {
            // Todo : Do Something
            Debug.Log($"{this.GetType().Name} : OnClose");
        }
        #endregion    
    }
}