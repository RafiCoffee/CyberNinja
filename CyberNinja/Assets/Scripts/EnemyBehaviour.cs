using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    Vector2 followPlayer;

    private GameObject bullet;
    private GameObject player;

    private BulletPool bulletPoolScript;

    // Start is called before the first frame update
    void Start()
    {
        bulletPoolScript = GameObject.Find("EnemyBulletPool").GetComponent<BulletPool>();
        player = GameObject.Find("Circle");

        followPlayer = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        followPlayer = player.transform.position - transform.position;

        transform.GetChild(1).up = followPlayer;

        if (player.transform.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

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
