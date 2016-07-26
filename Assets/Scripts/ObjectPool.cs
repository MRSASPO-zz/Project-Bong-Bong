using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool {
    private List<GameObject> pooledObjects;
    private GameObject objectPrefab;
    private int maxPoolSize;
    private int initialPoolSize;

    public ObjectPool(GameObject _obj, int _initialPoolSize, int _maxPoolSize) {
        pooledObjects = new List<GameObject>();

        for(int i = 0; i < _initialPoolSize; i++) {
            GameObject nObj = GameObject.Instantiate(_obj, Vector3.zero, Quaternion.identity) as GameObject;
            nObj.SetActive(false);
            GameObject.DontDestroyOnLoad(nObj);
            pooledObjects.Add(nObj);
        }

        this.maxPoolSize = _maxPoolSize;
        this.objectPrefab = _obj;
        this.initialPoolSize = _initialPoolSize;
    }

    public GameObject getObject() {
        for(int i = 0; i < pooledObjects.Count; i++) {
            if (!pooledObjects[i].activeSelf) {
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }
        }

        if (this.maxPoolSize > this.pooledObjects.Count) {
            GameObject nObj = GameObject.Instantiate(objectPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            nObj.SetActive(true);
            GameObject.DontDestroyOnLoad(nObj);
            pooledObjects.Add(nObj);
            return nObj;
        }
        return null;
    }

    public void Shrink() {
        int objectsToRemoveCount = pooledObjects.Count - initialPoolSize;
        if(objectsToRemoveCount <= 0) {
            return;
        }
        for(int i = pooledObjects.Count -1; i >= 0; i--) {
            if (!pooledObjects[i].activeSelf) {
                GameObject obj = pooledObjects[i];
                pooledObjects.Remove(obj);
            }
        }
    }

    public void Reset() {
        foreach(GameObject obj in pooledObjects) {
            obj.SetActive(false);
        }
    }
}
