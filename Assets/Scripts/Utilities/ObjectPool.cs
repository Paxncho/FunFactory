using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviourSingleton<ObjectPool> {

    Dictionary<int, Queue<GameObject>> pool;

    void Start() {
        pool = new Dictionary<int, Queue<GameObject>>();
    }

    GameObject AddInstanceToPool(GameObject prefab) {
        int instanceID = prefab.GetInstanceID();

        prefab.SetActive(false);
        GameObject instance = Instantiate(prefab);
        prefab.SetActive(true);
        instance.name = prefab.name;
        instance.transform.SetParent(transform);

        if (!pool.ContainsKey(instanceID))
            pool.Add(instanceID, new Queue<GameObject>());

        pool[instanceID].Enqueue(instance);
        return instance;
    }

    GameObject GetInstance(GameObject prefab, Vector3 position, Quaternion rotation) {
        int instanceID = prefab.GetInstanceID();
        GameObject instance = null;
        if (pool.ContainsKey(instanceID) && pool[instanceID].Count != 0) {

            if (!pool[instanceID].Peek().activeSelf) {
                //Debug.Log("Normal Behaviour");
                instance = pool[instanceID].Dequeue();
                pool[instanceID].Enqueue(instance);
            } else {
                //Debug.Log("Adding to the Pool");
                instance = AddInstanceToPool(prefab);
            }
        } else {
            //Debug.Log("Creating the Pool");
            instance = AddInstanceToPool(prefab);
        }

        instance.transform.position = position;
        instance.transform.rotation = rotation;
        instance.SetActive(true);

        return instance;
    }

    public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation) {
        return Instance.GetInstance(prefab, position, rotation);
    }

    public static GameObject Instantiate<T>(GameObject prefab, Vector3 position, Quaternion rotation, System.Action<T> action) {
        GameObject instance = Instance.GetInstance(prefab, position, rotation);

        T[] tComponents = instance.GetComponentsInChildren<T>();
        for (int i = 0; i < tComponents.Length; i++) {
            action(tComponents[i]);
        }

        return instance;
    }

    public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parentTo) {
        GameObject instance = Instance.GetInstance(prefab, position, rotation);
        instance.transform.SetParent(parentTo, true);
        return instance;
    }

    public static void Kill(GameObject prefab) {
        prefab.SetActive(false);
    }
}
