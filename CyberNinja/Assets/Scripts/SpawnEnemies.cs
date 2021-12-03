using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject pooledEnemy;
    public int pooledSize;
    public GameObject[] poolList;

    private Vector3 randomPos;

    private bool areSpawning = true;
    // Start is called before the first frame update
    void Start()
    {
        poolList = new GameObject[pooledSize];
        for (int i = 0; i < poolList.Length; i++)
        {
            poolList[i] = Instantiate(pooledEnemy, transform);
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
        if (areSpawning)
        {
            for (int i = 0; i < pooledSize; i++)
            {
                if (i > pooledSize - 2)
                {
                    areSpawning = false;
                }
                randomPos.x = Random.Range(-9, 10);
                randomPos.y = Random.Range(-4, 5);

                GetPooledObject().transform.position = transform.position + randomPos;
                GetPooledObject().transform.rotation = Quaternion.identity;

                GetPooledObject().SetActive(true);
            }
        }
    }
}
