using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private PoolableObject prefab;
    public List<PoolableObject> availableObjects;

    private ObjectPool(PoolableObject prefab, int size)
    {
        this.prefab = prefab;
        availableObjects = new List<PoolableObject>(size);
    }


    //Create object pool
    public static ObjectPool CreateInstance(PoolableObject prefab, int size, string poolName, Transform parent)
    {
        //Start new pool
        ObjectPool pool = new ObjectPool(prefab, size);

        //Create the parent that stores all the objects give it the prefab name pool
        GameObject poolObject = new GameObject(poolName);
        poolObject.transform.SetParent(parent);
        pool.CreateObjects(poolObject.transform, size);

        return pool;
    }

    private void CreateObjects(Transform parent, int size)
    {
        //For each object to make
        for (int i = 0; i < size; i++)
        {
            //Create the object
            PoolableObject poolableObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent.transform);
            //Make it a child of the pool container
            poolableObject.parent = this;
            //Give each clone a numbered name for tracking
            poolableObject.name = poolableObject.name + i;
            //Set the object inactive
            poolableObject.gameObject.SetActive(false);
            availableObjects.Add(poolableObject);
        }
    }

    //After the object is "despawned" return it to the pool
    public void ReturnObjectToPool(PoolableObject poolableObject)
    {
        availableObjects.Add(poolableObject);
    }

    public PoolableObject GetObject()
    {
        if (availableObjects.Count > 0)
        {
            int availableSlot = CheckForAvailableSlot();
            if (availableSlot != -1)
            {
                PoolableObject instance = availableObjects[availableSlot];
                availableObjects.RemoveAt(availableSlot);
                instance.gameObject.SetActive(true);

                return instance;
            }

            return null;
            
        }
        else
        {
            Debug.Log($"No \"{prefab.name}\" available from pool ");
            return null;
        }
    }

    public int CheckForAvailableSlot()
    {
        for (int i = 0; i < availableObjects.Count; i++)
        {
            if (availableObjects[i].isBeingUsed == false)
            {
                availableObjects[i].isBeingUsed = true;
                return i;
            }
        }
        return -1;
    }
}
