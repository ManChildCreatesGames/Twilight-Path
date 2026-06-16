using UnityEngine;
using System.Collections.Generic;
using System.Collections;



[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public Transform parent;
    public int size;
    public float spawnDelay;
}

[System.Serializable]
public class FireBallPool
{
    public float upperLeftBoundary;
    public float upperRightBoundary;
    public float lowerLeftBoundary;
    public float lowerRightBoundary;
    public GameObject fireBallPrefab;
    public Transform fireBallParent;
}

public class GameManager : MonoBehaviour
{
    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        BarrelPoolonAwake();
        //implement FIRST
        // FireBallPoolonAwake(); // Implement this method similarly to BarrelPoolonAwake if you want to manage fireballs with pooling as well

        // implement SECOND
        // scoring and lives management can be implemented here as well
    }

    void Start()
    {
        // Start spawning barrels for each pool with delay
        foreach (Pool pool in pools)
        {
            StartCoroutine(SpawnWithDelay(pool));
        }
    }

    IEnumerator SpawnWithDelay(Pool pool)
    {
        for (int i = 0; i < pool.size; i++)
        {
            SpawnFromPool(pool.tag, pool.parent);

            //wait between spawns
            yield return new WaitForSeconds(pool.spawnDelay);
        }
    }

    public GameObject SpawnFromPool(string tag, Transform parent)
    {
        string uniqueKey = tag + "_" + parent.name;

        if (!poolDictionary.ContainsKey(uniqueKey))
        {
            Debug.LogWarning($"Pool with key '{uniqueKey}' doesn't exist.");
            return null;
        }

        GameObject obj = poolDictionary[uniqueKey].Dequeue();

        // ✅ Reactivate the object
        obj.SetActive(true);

        // Reset position & rotation
        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        // Put it back into the queue
        poolDictionary[uniqueKey].Enqueue(obj);

        return obj;
    }

    public void ReturnToPool(GameObject obj, float delay)
    {
        StartCoroutine(ReturnAfterDelay(obj, delay));
    }

    private IEnumerator ReturnAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        var pooledObj = obj.GetComponent<PooledObject>();
        if (pooledObj != null)
        {
            obj.SetActive(false); // Deactivate
            // Spawn a replacement after the pool's spawn delay
            StartCoroutine(SpawnReplacementBarrelAfterDelay(pooledObj));
        }
    }

    private IEnumerator SpawnReplacementBarrelAfterDelay(PooledObject pooledObj)
    {
        // Find the pool's spawn delay from the list
        float delay = 0f;
        foreach (var pool in pools)
        {
            if (pool.tag == pooledObj.poolKey.Split('_')[0] && pool.parent == pooledObj.spawnParent)
            {
                delay = pool.spawnDelay;
                break;
            }
        }

        yield return new WaitForSeconds(delay);

        SpawnFromPool(pooledObj.poolKey.Split('_')[0], pooledObj.spawnParent);
    }

    public void BarrelPoolonAwake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);

                if (pool.parent != null)
                    obj.transform.SetParent(pool.parent, false);

                obj.SetActive(false);

                // Attach pooled object info
                var pooledObj = obj.AddComponent<PooledObject>();
                pooledObj.poolKey = pool.tag + "_" + pool.parent.name;
                pooledObj.spawnParent = pool.parent;
                pooledObj.gameManager = this;

                objectPool.Enqueue(obj);
            }

            string uniqueKey = pool.tag + "_" + pool.parent.name;
            poolDictionary.Add(uniqueKey, objectPool);
        }
    }
}