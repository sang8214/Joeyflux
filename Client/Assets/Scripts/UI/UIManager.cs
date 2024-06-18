using System.Collections.Generic;
using UnityEngine;
using Flux.CommonInstance;
using Flux.Bundle;

namespace Flux.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private Dictionary<string, UIWindow> _mapWindowObject = new Dictionary<string, UIWindow>();
        private readonly string UI_PREFIX = "UI/";

        public Transform windowTrans;
        public Transform popupTrans;

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

            if (!obj.TryGetComponent<T>(out var component)) 
                return false;
            
            component.transform.SetParent(windowTrans, false);
            _mapWindowObject.Add(typeName, component);
            uiWindow = component;
            
            return true;
        }
        
        // UI Popup을 가져오는 함수
        public bool TryGetUIPopup<T>(out T uiPopup, bool isForceBlurParent = false) where T : UIPopup
        {
            var typeName = typeof(T).Name;
            var addressName = GetKey(typeName);

            uiPopup = null;
            var obj = ObjectPoolManager.Instance.GetObjectSync(addressName);

            if (obj == null)
                return false;

            obj.SetActive(false);

            if (!obj.TryGetComponent<T>(out var popup))
                return false;
            
            popup.transform.SetParent(popupTrans, false);
            uiPopup = popup;
               
            return true;
        }
        
        // UI Component 가져오는 함수
        public T GetUIComponent<T>(Transform root)
        {
            if (root is null)
                return default(T);

            var typeName = typeof(T).Name;
            var addressName = GetKey(typeName);
            var obj = ObjectPoolManager.Instance.GetObjectSync(addressName);

            if (obj is null)
                return default(T);

            obj.transform.SetParent(root.transform, false);

            return !obj.TryGetComponent<T>(out var component) ? default(T) : component;
        }
        
        // UI Popup을 ObjectPool에 반환하는 함수
        public void ReturnUIPopup<T>(T popup) where T : UIPopup
        {
            if (!popup)
                return;
        
            var typeName = typeof(T).Name;
            var addressName = GetKey(typeName);

            ObjectPoolManager.Instance.ReturnObject(addressName, popup.gameObject);
        }
        
        // UI Component를 ObjectPool에 반환하는 함수
        public void ReturnUIComponent<T>(GameObject obj)
        {
            if (!obj)
                return;

            var typeName = typeof(T).Name;
            var addressName = GetKey(typeName);

            ObjectPoolManager.Instance.ReturnObject(addressName, obj);
        }
    }
}
