using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    [SerializeField] private GameObject crossPrefab;
    [SerializeField] private int poolSize;
    [SerializeField] private bool expandable;

    private List<GameObject> freeList;
    private List<GameObject> usedList;

    private void Awake()
    {
        freeList = new List<GameObject>();
        usedList = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            Setup();   
        }
    }

    private void Setup()
    {
        GameObject g = Instantiate(crossPrefab, transform);
        g.SetActive(false);
        freeList.Add(g);
    }

    public GameObject GetObject()
    {
        if(freeList.Count == 0 && !expandable)
        {
            return null;
        }
        else if(freeList.Count == 0)
        {
            Setup();
        }

        GameObject g = freeList[freeList.Count - 1];
        freeList.RemoveAt(freeList.Count - 1);
        usedList.Add(g);
        g.SetActive(true);

        return g;
    }

    public void ReturnObject(GameObject obj)
    {
        Debug.Assert(usedList.Contains(obj));

        obj.transform.parent = transform;

        obj.SetActive(false);
        usedList.Remove(obj);
        freeList.Add(obj);
    }
}
