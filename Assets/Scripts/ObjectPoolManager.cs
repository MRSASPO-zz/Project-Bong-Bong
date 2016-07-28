using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ObjectPoolManager{
    private static volatile ObjectPoolManager instance;

    private Dictionary<string, ObjectPool> objectPools;

    private static object syncRoot = new System.Object(); //locking

    //Constructor
    private ObjectPoolManager() {
        this.objectPools = new Dictionary<string, ObjectPool>();
    }

    public static ObjectPoolManager Instance {
        get {
            //check if does not exists
            if(instance == null) {
                //lock access, if locked, wait
                lock (syncRoot) {
                    if(instance == null) {
                        instance = new ObjectPoolManager();
                    }
                }
            }
            return instance;
        }
    }

    public bool createPool(GameObject objToPool, int initialPoolSize, int maxPoolSize) {
        if (ObjectPoolManager.Instance.objectPools.ContainsKey(objToPool.name)) {
            return false;
        } else {
            ObjectPool nPool = new ObjectPool(objToPool, initialPoolSize, maxPoolSize);
            ObjectPoolManager.Instance.objectPools.Add(objToPool.name, nPool);
            return true;
        }
    }

    public GameObject GetObject(string objName) {
        return ObjectPoolManager.Instance.objectPools[objName].getObject();
    }

    public void resetPool(string objName) {
        ObjectPoolManager.Instance.objectPools[objName].Reset();
    }
}
