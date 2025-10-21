using System.Collections.Generic;
using UnityEngine;

public enum ObjTypePoolling { None,HitBlood,Audio }
public class ManagerPooling : GenericSingleton<ManagerPooling>
{
    [SerializeField] private List<PoolObject_SO> poolObjectsList;

    private Dictionary<ObjTypePoolling, GameObject> spawnDictionaryParent;
    private Dictionary<ObjTypePoolling, Queue<Pool_Obj>> poolDictionary;

    public override void Awake()
    {
        base.Awake();
        GeneratePool();
    }

    private void GeneratePool()
    {
        poolDictionary = new Dictionary<ObjTypePoolling, Queue<Pool_Obj>>();
        spawnDictionaryParent = new Dictionary<ObjTypePoolling, GameObject>();

        if (poolObjectsList.Count <= 0)
        {
            Debug.Log("No obj in ManagerPool ");
            return;
        }

        foreach (PoolObject_SO poolObj in poolObjectsList)
        {
            Queue<Pool_Obj> objectPool = new Queue<Pool_Obj>();
            Queue<GameObject> objToPatent = new Queue<GameObject>();

            GameObject objParent = new GameObject("Pool Parent " + poolObj.ObjTypePoolling);
            objParent.transform.parent = transform;
            objToPatent.Enqueue(objParent);

            for (int i = 0; i < poolObj.StartSize; i++)
            {
                Pool_Obj obj = Instantiate(poolObj.PreFab, objParent.transform);

                obj.SetUp(poolObj.LifeTime, poolObj.ObjTypePoolling);
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(poolObj.ObjTypePoolling, objectPool);
            spawnDictionaryParent.Add(poolObj.ObjTypePoolling, objParent);
        }
    }


    #region GetFromPool
    public Pool_Obj GetObjFromPool(ObjTypePoolling type, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.TryGetValue(type, out Queue<Pool_Obj> pool))
        {
            Debug.LogError("No ObjType Correct From Get");
            return null;
        }

        if (pool.Count > 0)
        {
            Pool_Obj objToSpawn = poolDictionary[type].Dequeue();

            objToSpawn.transform.position = position;
            objToSpawn.transform.rotation = rotation;

            objToSpawn.gameObject.SetActive(true);
            return objToSpawn;
        }
        else return SpawnForPool(type, position, rotation);
    }
    public Pool_Obj GetAudioFromPool(ObjTypePoolling type, Vector3 position, Quaternion rotation,AudioClip audioClip,float volume,float minPitch,float maxPitch,bool isRandomPitch,bool is3DAudio)
    {
        if (!poolDictionary.TryGetValue(type, out Queue<Pool_Obj> pool) && type != ObjTypePoolling.Audio)
        {
            Debug.LogError("No ObjType Correct From Get");
            return null;
        }


        if (pool.Count > 0)
        {
            Pool_Obj objToSpawn = poolDictionary[type].Dequeue();

            if (objToSpawn.TryGetComponent(out AudioSource audio))
            {
                audio.clip = audioClip;
                audio.volume = volume;

                audio.pitch = isRandomPitch ? Random.Range(minPitch, maxPitch) : maxPitch;
                audio.spatialBlend = is3DAudio ? 1 : 0;

                objToSpawn.SetUpNewTime(audio.clip.length);
            }

            objToSpawn.transform.position = position;
            objToSpawn.transform.rotation = rotation;

            objToSpawn.gameObject.SetActive(true);


            return objToSpawn;
        }
        else return SpawnForPoolAudio(type, position, rotation,audioClip,volume,minPitch,maxPitch,isRandomPitch,is3DAudio);
    }
    #endregion

    public void ReturnToPool(ObjTypePoolling type, Pool_Obj obj)
    {
        if (!poolDictionary.TryGetValue(type, out Queue<Pool_Obj> pool))
        {
            Debug.LogError("No ObjType Correct From Return");
            Destroy(obj.gameObject);
            return;
        }

        PoolObject_SO poolList = poolObjectsList.Find(t => t.ObjTypePoolling == type);
        if (pool.Count >= poolList.MaxSizePool)
        {
            //Debug.LogWarning("Pool " + type + " Is Full");
            Destroy(obj.gameObject);
            return;
        }

        obj.gameObject.SetActive(false);
        poolDictionary[type].Enqueue(obj);
    }

    private Pool_Obj SpawnForPool(ObjTypePoolling type, Vector3 position, Quaternion rotation)
    {
        PoolObject_SO poolList = poolObjectsList.Find(t => t.ObjTypePoolling == type);

        Transform parent = spawnDictionaryParent.ContainsKey(type) ? spawnDictionaryParent[type].transform : transform;
        Pool_Obj objToSpawn = Instantiate(poolList.PreFab, position, rotation, parent);
        objToSpawn.SetUp(poolList.LifeTime, poolList.ObjTypePoolling);

        return objToSpawn;
    }

    private Pool_Obj SpawnForPoolAudio(ObjTypePoolling type, Vector3 position, Quaternion rotation, AudioClip audioClip, float volume, float minPitch, float maxPitch, bool isRandomPitch, bool is3DAudio)
    {
        PoolObject_SO poolList = poolObjectsList.Find(t => t.ObjTypePoolling == type);

        Transform parent = spawnDictionaryParent.ContainsKey(type) ? spawnDictionaryParent[type].transform : transform;
        Pool_Obj objToSpawn = Instantiate(poolList.PreFab, position, rotation, parent);

        if (objToSpawn.TryGetComponent(out AudioSource audio))
        {
            audio.clip = audioClip;
            audio.volume = volume;

            audio.pitch = isRandomPitch ? Random.Range(minPitch, maxPitch) : maxPitch;
            audio.spatialBlend = is3DAudio ? 1 : 0;

            objToSpawn.SetUpNewTime(audio.clip.length);
            objToSpawn.SetUp(poolList.LifeTime, poolList.ObjTypePoolling);
        }     
        return objToSpawn;
    }
}
