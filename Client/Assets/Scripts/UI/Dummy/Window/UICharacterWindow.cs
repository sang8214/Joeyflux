using System.Collections.Generic;
using Flux.UI.Popup;
using Flux.UI.Component;
using UnityEngine;

namespace Flux.UI.Window
{
    public class UICharacterWindow : UIWindow
    {
        [SerializeField] private Transform characterListRoot;
        private List<UICharacterComponent> _listCharacterComponent = new();
        
        public void ShowLayout()
        {
            Debug.Log($"{this.GetType().Name} : ShowLayout");
            OnRefresh();
        }
        
        private void OnEventSelectCharacter(int id)
        {
            if (UINavigator.Instance.TryOpenPopup<UICharacterPopup>(out var uiCharacterPopup))
                uiCharacterPopup.ShowLayout(id);
        }

        #region override
        public override void OnRefresh()
        {
            Debug.Log($"{this.GetType().Name} : OnRefresh");
            
            // Todo : Do Something
            UIManager.Instance.onEventChangeWindow?.Invoke(windowType);
            
            for (int i = 0; i < 3; i++)
            {
                if (!UIManager.Instance.TryGetUIComponent<UICharacterComponent>(characterListRoot, out var uiComponent))
                    continue;

                uiComponent.onEventSelectCharacter = OnEventSelectCharacter;
                uiComponent.gameObject.SetActive(true);
                uiComponent.ShowLayout(i+1);
                
                _listCharacterComponent.Add(uiComponent);
            }
        }
        public override void OnClose()
        {
            Debug.Log($"{this.GetType().Name} : OnClose");

            // Todo : Do Something
            foreach (var characterComponent in _listCharacterComponent)
            {
                UIManager.Instance.ReturnUIComponent(characterComponent);    
            }

            _listCharacterComponent.Clear();
        }
        #endregion                
    }
}
