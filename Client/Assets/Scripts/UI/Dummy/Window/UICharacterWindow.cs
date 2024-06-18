using System.Collections;
using System.Collections.Generic;
using Flux.UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace Flux.UI.Window
{
    public class UICharacterWindow : UIWindow
    {
        [SerializeField] private Text characterNameText;
        
        public void ShowLayout(string characterName)
        {
            characterNameText.text = characterName;
            Debug.Log($"{this.GetType().Name} : ShowLayout");
            OnRefresh();
        }
        
        public void OnTouchLevelUp()
        {
            if (UINavigator.Instance.TryOpenPopup<UILevelUpPopup>(out var uiLevelUpPopup))
                uiLevelUpPopup.ShowLayout();
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
