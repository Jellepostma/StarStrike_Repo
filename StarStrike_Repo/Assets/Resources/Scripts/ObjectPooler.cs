using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class ObjectPoolItem
    {
        public int amountToPool;
        public GameObject objectToPool;
        public bool expands;
    }

    public static ObjectPooler sharedInstance;

    public List<GameObject> pooledObjects;
    public List<ObjectPoolItem> itemsToPool;

    private Transform poolParent;

    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        poolParent = new GameObject("Pool").transform;
        poolParent.transform.parent = transform;
        pooledObjects = new List<GameObject>();

        foreach (ObjectPoolItem item in itemsToPool)
        {
            Transform objParent;
            if (!poolParent.Find("Pool_" + item.objectToPool.tag))
            {
                objParent = new GameObject("Pool_" + item.objectToPool.tag).transform;

            }
            else
            {
                objParent = poolParent.Find("Pool_" + item.objectToPool.tag).transform;
            }
            objParent.parent = poolParent;
            for (int i = 0; i < item.amountToPool; i++)
            {
                AddPooledObject(item);
            }

        }
    }

    private GameObject AddPooledObject(ObjectPoolItem item)
    {
        GameObject obj = (GameObject)Instantiate(item.objectToPool);
        obj.SetActive(false);
        obj.transform.parent = poolParent.Find("Pool_" + item.objectToPool.tag).transform;
        obj.GetComponent<PoolableObject>().poolParent = obj.transform.parent;
        pooledObjects.Add(obj);
        return obj;
    }

    public GameObject GetPooledObject(string itemTag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == itemTag)
            {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == itemTag)
            {
                if (item.expands)
                {
                    return AddPooledObject(item);
                }
            }
        }
        return null;
    }
}