using System;
using System.Collections.Generic;
using UnityEngine;
using Flux.Bundle;
using Object = UnityEngine.Object;

public class ObjectPool
{
    private List<System.Action<GameObject>> _asyncList = new();
    private string _addressName = string.Empty;

    private readonly Stack<GameObject> _pool = new();
    private GameObject _bundleObject = null;
    private Transform _poolTransform = null;
    private bool _isAsync = false;

    private Vector3 _originPosition;
    private Quaternion _originRotation;
    private Vector3 _originScale;

    public Transform PoolTransform => _poolTransform;

    public void SetInfo(string addressName, Transform poolTransform, bool isAsync)
    {
        _addressName = addressName;
        _poolTransform = poolTransform;
        _isAsync = isAsync;

        if (!isAsync)
        {
            _bundleObject = BundleManager.Instance.LoadBundleSync(_addressName) as GameObject;

            if(_bundleObject == null)
                Debug.LogError($"Bundle is null = {_addressName}");
            else
                CacheOrigin(_bundleObject.transform);
        }
        else
        {
            BundleManager.Instance.LoadBundleAsync(addressName, eLOADTYPE.GameObject, (obj) =>
                OnLoadCompleted(_bundleObject = obj as GameObject)
            );
        }
    }
    public GameObject GetObject()
    {
        if (!_bundleObject)
            return null;

        var poolObject = Object.Instantiate(_bundleObject, _poolTransform, false);
        poolObject.SetActive(false);

        return poolObject;
    }

    private void CacheOrigin(Transform trans)
    {
        _originPosition = trans.localPosition;
        _originRotation = trans.localRotation;
        _originScale = trans.localScale;
    }

    public void OnLoadCompleted(GameObject obj)
    {
        _bundleObject = obj;

        CacheOrigin(_bundleObject?.transform);

        foreach (var t in _asyncList)
        {
            t?.Invoke(GetObject());
        }

        _asyncList.Clear();
    }

    public GameObject GetObjectSync()
    {
        return _pool.Count > 0 ? _pool.Pop() : GetObject();
    }

    public void GetObjectAsync(System.Action<GameObject> onFinish = null)
    {
        if (_pool.Count > 0)
        {
            onFinish?.Invoke(_pool.Pop());
            return;
        }
        else if(_bundleObject != null)
        {
            onFinish?.Invoke(GetObject());
            return;
        }

        _asyncList.Add(onFinish);
    }

    public void ReturnObject(GameObject poolObject)
    {
        if (!poolObject)
            return;
        
        poolObject.SetActive(false);
        poolObject.transform.position = _originPosition;
        poolObject.transform.rotation = _originRotation;
        poolObject.transform.localScale = _originScale;
        poolObject.transform.SetParent(_poolTransform, false);
        
        _pool.Push(poolObject);
    }
}
