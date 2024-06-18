using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Flux.Yield;

namespace Flux.Bundle
{
    public class BundleLoader
    {
        private BundleManager _managerInstance = null;
        private readonly Dictionary<string, AsyncOperationHandle> _mapAddressableHandle = new();

        // Schedule 관련 로직
        private readonly BundleLoadScheduler _loadScheduler = new();
        private Coroutine _crRefreshSchedule;
        
        public void Init(BundleManager bundleManager)
        {
            _managerInstance = bundleManager;
        }
        
        private bool TryGetResource(string assetName, out object result)
        {
            if (_mapAddressableHandle.TryGetValue(assetName, out var handle))
            {
                result = handle.Result;
                return true;
            }
            result = null;
            return false;
        }
        private bool TryGetHandle(eLOADTYPE loadType, string assetName, out AsyncOperationHandle handle)
        {
            handle = loadType switch
            {
                eLOADTYPE.Texture2D => Addressables.LoadAssetAsync<Texture2D>(assetName),
                eLOADTYPE.GameObject => Addressables.LoadAssetAsync<GameObject>(assetName),
                eLOADTYPE.Material => Addressables.LoadAssetAsync<Material>(assetName),
                eLOADTYPE.Audio => Addressables.LoadAssetAsync<AudioClip>(assetName),
                _ => Addressables.LoadAssetAsync<object>(assetName)
            };

            return true;
        }
        
        public object LoadBundleSync(string assetName, eLOADTYPE loadType)
        {
            if (_mapAddressableHandle.TryGetValue(assetName, out var cachingHandle))
                return cachingHandle.Result;

            if (!TryGetHandle(loadType, assetName, out var handle))
                return null;

            if (!handle.IsValid())
                return null;

            _mapAddressableHandle.TryAdd(assetName, handle);
            return handle.WaitForCompletion();
        }
        
        public void LoadBundleAsync(string assetName, eLOADTYPE loadType, Action<object> onResult)
        {
            Debug.Log($"Loading Step : Load Asset Async = {assetName}");
            
            StartScheduler();

            if (TryGetResource(assetName, out var handleResult))
            {
                onResult?.Invoke(handleResult);
            }
            else
            {
                _loadScheduler.Regist(assetName, loadType, onResult);
            }
        }
        
        private void StartScheduler()
        {
            _loadScheduler.onEventRequestLoad ??= RequestLoad;
            _crRefreshSchedule ??= _managerInstance.StartCoroutine(CRRefreshSchedule());
        }
        
        private void RequestLoad(BundleElement bundle)
        {
            if (bundle == null)
               return;

            _managerInstance.StartCoroutine(CRLoadBundle(bundle));
        }

        private IEnumerator CRRefreshSchedule()
        {
            while (true)
            {
                _loadScheduler.Refresh();
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }
        private IEnumerator CRLoadBundle(BundleElement bundle)
        {
            if (bundle == null)
                yield break;

            if (TryGetResource(bundle.AssetName, out var bundleResult))
            {
                _loadScheduler.RemoveLoadingBundle(bundle.AssetName, bundleResult);
            }
            else
            {
                if (!TryGetHandle(bundle.LoadType, bundle.AssetName, out var handle)) 
                    yield break;
                
                yield return handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    _loadScheduler.RemoveLoadingBundle(bundle.AssetName, handle.Result);
                    _mapAddressableHandle.TryAdd(bundle.AssetName, handle);
                }
                else
                {
                    _loadScheduler.RemoveLoadingBundle(bundle.AssetName, handle.Result);
                }
            }
        }
        public void Clear()
        {
            _loadScheduler.Clear();
            _crRefreshSchedule = null;
            
            foreach (var data in _mapAddressableHandle)
            {
                Addressables.Release(data.Value);
            }

            _mapAddressableHandle.Clear();
        }
    }
}

