using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooling : Singleton<ObjectPooling>
{
    [SerializeField] private List<GameObject> pooledObjects = new List<GameObject>();

    public void InstantiatePoolObject(GameObject prefabObject)
    {
        //create bullet prefab
        GameObject newPoolObject = Instantiate(prefabObject);
        newPoolObject.SetActive(false);

        pooledObjects.Add(newPoolObject);
    }

    public void DestroyBulletByAttacker(Transform attacker)
    {
        GameObject bullet = pooledObjects.Find(bullet => bullet.GetComponent<BulletController>().attacker == attacker);
        if(bullet != null)
        {
            Destroy(bullet);
            pooledObjects.Remove(bullet);
        }
    }

    public GameObject GetPoolObjectByAttacker(Transform attacker, Transform spawnPostion)
    {
        //search bullet prefabs and active it
        GameObject foudedObject = pooledObjects.FirstOrDefault(x => x.GetComponent<BulletController>().attacker == attacker
                                                                    && !x.activeSelf);
        if (foudedObject != null)
        {
            foudedObject.transform.position = spawnPostion.position;
            foudedObject.SetActive(true);
            return foudedObject;
        }
        else
        {
            //Instntiate if foundedObject = null then put it in pool
            GameObject copyBullet = pooledObjects.FirstOrDefault(x => x.GetComponent<BulletController>().attacker == attacker);
            GameObject newBullet = Instantiate(copyBullet);
            pooledObjects.Add(newBullet);
            return newBullet;
        }
    }

    public void ClearPool()
    {
        foreach (GameObject bullet in pooledObjects)
        {
            Destroy(bullet);
        }
        pooledObjects.Clear();
    }
}
