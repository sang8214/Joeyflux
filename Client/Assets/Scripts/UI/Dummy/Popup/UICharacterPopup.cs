using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flux.UI.Component;

namespace Flux.UI.Popup
{
    public class UICharacterPopup : UIPopup
    {
        [SerializeField] private Transform characterRoot;
        private UICharacterComponent _characterComponent = null;
        private int _characterId = 0;
        
        public void ShowLayout(int id)
        {
            Debug.Log($"{this.GetType().Name} : ShowLayout");
            
            // Todo : Do Something
            _characterId = id;
            
            if (!UIManager.Instance.TryGetUIComponent<UICharacterComponent>(characterRoot, out var uiComponent)) 
                return;

            uiComponent.gameObject.SetActive(true);
            uiComponent.ShowLayout(id);
            
            _characterComponent = uiComponent;
        }

        public void OnTouchStatus()
        {
            if (!UINavigator.Instance.TryOpenPopup<UICharacterStatusPopup>(out var uiCharacterStatusPopup)) 
                return;
            
            uiCharacterStatusPopup.ShowLayout(_characterId);
        }
        
        public void OnTouchClose()
        {
            // back button or close popup
            UINavigator.Instance.OnProcessBackButton();
            //UINavigator.Instance.ClosePopup();
        }
        
        #region override
        public override void OnRefresh()
        {
            Debug.Log($"{this.GetType().Name} : OnRefresh");
            
            // Todo : Do Something
        }
        public override void OnClose()
        {
            Debug.Log($"{this.GetType().Name} : OnClose");
            
            // Todo : Do Something
            if(_characterComponent)
                UIManager.Instance.ReturnUIComponent(_characterComponent);    
        }
        #endregion    
    }
}

