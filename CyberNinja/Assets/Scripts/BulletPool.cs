using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject pooledBullet;
    public int pooledSize;
    public GameObject[] poolList;
    // Start is called before the first frame update
    void Start()
    {
        poolList = new GameObject[pooledSize];
        for (int i = 0; i < poolList.Length; i++)
        {
            poolList[i] = Instantiate(pooledBullet, transform);
            poolList[i].SetActive(false);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledSize; i++)
        {
            if (!poolList[i].activeInHierarchy)
            {
                return poolList[i];
            }
        }
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
