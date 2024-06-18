using System.Collections.Generic;
using UnityEngine;
using Flux.CommonInstance;

public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>
{
    private Dictionary<string, ObjectPool> _mapObjectPool = new();

    #region Sync : UI Prefab
    public void Generate(string addressName, bool isAsync)
    {
        if(_mapObjectPool.ContainsKey(addressName))
            return;

        var go = new GameObject($"{addressName}_Pool");
        go.transform.SetParent(this.transform);

        var objectPool = new ObjectPool();
        objectPool.SetInfo(addressName, go.transform, isAsync);
        _mapObjectPool.Add(addressName, objectPool);
    }
    public GameObject GetObjectSync(string addressName)
    {
        if (!_mapObjectPool.ContainsKey(addressName))
            Generate(addressName, false);

        return _mapObjectPool[addressName].GetObjectSync();
    }
    #endregion

    #region Async : Effect Prefab
    public void GetObjectAsync(string addressName, System.Action<GameObject> onFinish = null)
    {
        if(!_mapObjectPool.ContainsKey(addressName))
            Generate(addressName, true);

        _mapObjectPool[addressName].GetObjectAsync(onFinish);
    }
    #endregion

    public void RemovePool(string addressName)
    {
        if(!_mapObjectPool.TryGetValue(addressName, out var pool))
            return;

        if(pool != null)
            Destroy(pool.PoolTransform.gameObject);

        _mapObjectPool.Remove(addressName);            
    }

    public void Clear()
    {
        var iterator = _mapObjectPool.GetEnumerator();

        while (iterator.MoveNext())
        {
            var current = iterator.Current.Value;

            if(current != null)
                Destroy(current.PoolTransform.gameObject);
        }

        _mapObjectPool.Clear();
    }

    public void ReturnObject(string addressName, GameObject poolObject)
    {
        if (_mapObjectPool.ContainsKey(addressName))
            _mapObjectPool[addressName].ReturnObject(poolObject);
    }
}
