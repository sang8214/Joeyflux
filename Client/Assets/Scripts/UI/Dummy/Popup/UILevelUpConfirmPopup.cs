using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flux.UI.Popup
{
    public class UILevelUpConfirmPopup : UIPopup
    {
        public void ShowLayout()
        {
            Debug.Log($"{this.GetType().Name} : ShowLayout");
        }

        public void OnTouchClose()
        {
            // 백버튼이나 팝업을 닫는 로직 수행...
            UINavigator.Instance.OnProcessBackButton();
            //UINavigator.Instance.ClosePopup();
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