
using System.Collections.Generic;

namespace Flux.Bundle
{
    public class BundleLoadScheduler
    {
#if UNITY_EDITOR
        private readonly int _loadMultipleLimit = 20;
#else
        private readonly int _loadMultipleLimit = 10;
#endif

        public System.Action<BundleElement> onEventRequestLoad = null;

        // 로딩 대기중인 에셋
        private Queue<BundleElement> _standbyAssetQueue = new();
        // 현재 로딩중인 에셋
        private Dictionary<string, BundleElement> _loadingAssetMaps = new();
        
        public void Regist(string assetName, eLOADTYPE loadType, System.Action<object> onFinished)
        {
            var bundleElement = new BundleElement
            {
                AssetName = assetName,
                LoadType = loadType,
                OnFinished = onFinished
            };

            AddLoadingBundle(bundleElement);
        }

        private void AddLoadingBundle(BundleElement bundleElement)
        {
            if (bundleElement == null)
                return;

            if (!_loadingAssetMaps.ContainsKey(bundleElement.AssetName) && _loadingAssetMaps.Count < _loadMultipleLimit)
            {
                _loadingAssetMaps.Add(bundleElement.AssetName, bundleElement);
                onEventRequestLoad?.Invoke(bundleElement);
            }
            else
            {
                _standbyAssetQueue.Enqueue(bundleElement);
            }
        }

        public void RemoveLoadingBundle(string assetName, object result)
        {
            if (!_loadingAssetMaps.TryGetValue(assetName, out var bundle))
                return;
            
            bundle.OnFinished?.Invoke(result);
            _loadingAssetMaps.Remove(assetName);
        }

        public void Refresh()
        {
            var loopCount = _standbyAssetQueue.Count;

            for (int i = 0; i < loopCount; i++)
            {
                if (_standbyAssetQueue.TryDequeue(out var item))
                    AddLoadingBundle(item);
            }
        }

        public void Clear()
        {
            _standbyAssetQueue.Clear();
            _loadingAssetMaps.Clear();
        }
    }    
}


