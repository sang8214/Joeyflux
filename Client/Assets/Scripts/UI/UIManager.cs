using System.Collections.Generic;
using UnityEngine;
using Flux.CommonInstance;
using Flux.Bundle;

namespace Flux.UI
{
    public enum eWINDOW_TYPE
    {
        None,
        Lobby,
        Inventory,
        Character,
        Max
    }
    
    public class UIManager : MonoSingleton<UIManager>
    {
        private Dictionary<string, UIWindow> _mapWindowObject = new Dictionary<string, UIWindow>();
        private readonly string UI_PREFIX = "UI/";

        public Transform windowTrans;
        public Transform topWindowTrans;
        public Transform popupTrans;
        public Transform topPopupTrans;

        public System.Action<eWINDOW_TYPE> onEventChangeWindow = null;

        private string GetKey(string typeName)
        {
            return $"{UI_PREFIX}{typeName}";
        }

        // UI Window을 가져오는 함수
        public bool TryGetUIWindow<T>(out T uiWindow) where T : UIWindow
        {
            uiWindow = null;
            var typeName = typeof(T).Name;
            if (_mapWindowObject.TryGetValue(typeName, out var prevUIWindow))
            {
                if (prevUIWindow is T prevWindow)
                {
                    uiWindow = prevWindow;
                    return true;
                }
            }

            var addressName = GetKey(typeName);
            var bundleObj = BundleManager.Instance.LoadBundleSync(addressName);
            
            if (bundleObj is GameObject bundle == false)
                return false;

            var obj = Instantiate(bundle);
            obj.SetActive(false);

            if (!obj.TryGetComponent<T>(out var window)) 
                return false;

            window.transform.SetParent(window.isTop ? topWindowTrans : windowTrans, false);

            _mapWindowObject.Add(typeName, window);
            uiWindow = window;
            
            return true;
        }
        
        // UI Popup을 가져오는 함수
        public bool TryGetUIPopup<T>(out T uiPopup) where T : UIPopup
        {
            var typeName = typeof(T).Name;
            var addressName = GetKey(typeName);

            uiPopup = null;
            var obj = ObjectPoolManager.Instance.GetObjectSync(addressName);

            if (!obj)
                return false;

            obj.SetActive(false);

            if (!obj.TryGetComponent<T>(out var popup))
                return false;
            
            popup.transform.SetParent(popup.isTop ? topPopupTrans : popupTrans, false);

            uiPopup = popup;
               
            return true;
        }
        
        // UI Component 가져오는 함수
        public bool TryGetUIComponent<T>(Transform root, out T uiComponent) where T : UIComponent
        {
            uiComponent = null;
            
            if (root is null)
                return false;

            var typeName = typeof(T).Name;
            var addressName = GetKey(typeName);
            var obj = ObjectPoolManager.Instance.GetObjectSync(addressName);

            if (!obj)
                return false;

            obj.transform.SetParent(root, false);
            
            if (!obj.TryGetComponent<T>(out var component))
                return false;

            uiComponent = component;
            return true;
        }
        
        // UI Popup을 ObjectPool에 반환하는 함수
        public void ReturnUIPopup(UIPopup uiPopup)
        {
            if (!uiPopup)
                return;
        
            var typeName = uiPopup.GetType().Name;
            var addressName = GetKey(typeName);

            ObjectPoolManager.Instance.ReturnObject(addressName, uiPopup.gameObject);
        }
        
        // UI Component를 ObjectPool에 반환하는 함수
        public void ReturnUIComponent(UIComponent uiComponent)
        {
            if (!uiComponent)
                return;

            var typeName = uiComponent.GetType().Name;
            var addressName = GetKey(typeName);

            ObjectPoolManager.Instance.ReturnObject(addressName, uiComponent.gameObject);
        }
    }
}
