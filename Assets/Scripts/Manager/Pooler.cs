using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pooler : MonoBehaviour
{
    [HideInInspector] public ObjectPool<GameObject> pool;

    [SerializeField] private GameObject crossPrefab;
    [SerializeField] private int poolSize;
    private void Awake()
    {
        pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, 10, poolSize);
    }

    private GameObject CreatePooledItem()
    {
        return Instantiate(crossPrefab, transform);
    }

    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReturnedToPool(GameObject obj)
    {
        obj.transform.parent = transform;
        obj.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }
}
