using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flux.UI.Window
{
    public class UILobbyTopWindow : UIWindow
    {


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
