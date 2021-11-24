using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    private GameObject bullet;

    private BulletPool bulletPoolScript;

    // Start is called before the first frame update
    void Start()
    {
        bulletPoolScript = GameObject.Find("EnemyBulletPool").GetComponent<BulletPool>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Shoot();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    void Shoot()
    {
        bullet = bulletPoolScript.GetPooledObject();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = bullet.transform.rotation;
        bullet.SetActive(true);
    }
}
