using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Flux.CommonInstance;

namespace Flux.Bundle
{
    public enum eLOADTYPE
    {
        None,           // object
        GameObject,
        Texture2D,       // texture2d
        Material,
        Audio,
    }
    
    public class BundleElement
    {
        public string AssetName;
        public eLOADTYPE LoadType;
        public Action<object> OnFinished;
    }

    public class BundleManager : MonoImmortal<BundleManager>
    {
        private readonly BundleLoader _bundleLoader = new();

        private void Awake()
        {
            _bundleLoader.Init(this);
        }

        private void Start()
        {
            ResourceManager.ExceptionHandler = OnLoadFailedByException;
        }

        private static void OnLoadFailedByException(AsyncOperationHandle handle, Exception e)
        {
            if (e.GetType() != typeof(InvalidKeyException))
                Addressables.LogException(handle, e);
        }

        #region bundle_general
        
        // sync로 로딩해야 할 것들(UI Prefab, Effect 등등)
        public object LoadBundleSync(string assetName, eLOADTYPE loadType = eLOADTYPE.GameObject)
        {
            return _bundleLoader.LoadBundleSync(assetName, loadType);
        }

        // 3D 모델링이나, Scene Transition간 필요한 리소스 로드
        public void LoadBundleAsync(string assetName, eLOADTYPE loadType = eLOADTYPE.None, System.Action<object> onResult = null)
        {
            _bundleLoader.LoadBundleAsync(assetName, loadType, onResult);
        }
        
        #endregion

        private void OnDestroy()
        {
            ReleaseHandleMap();
        }

        private void ReleaseHandleMap()
        {
            StopAllCoroutines();
            _bundleLoader.Clear();
            Caching.ClearCache();
        }
    }
}
