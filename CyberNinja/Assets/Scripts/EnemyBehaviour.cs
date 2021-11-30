using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class EnemyBehaviour : MonoBehaviour
{
    Vector2 followPlayer;

    public float coolDown;

    private GameObject bullet;
    private GameObject player;

    private Stopwatch timer = new Stopwatch();

    public BulletPool bulletPoolScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        followPlayer = player.transform.position - transform.position;

        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        followPlayer = player.transform.position - transform.position;

        transform.GetChild(1).up = followPlayer;

        if (player.transform.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            //transform.GetChild(1).GetChild(1).rotation = 
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            //transform.GetChild(1).GetChild(1).rotation = 
        }

        if (timer.ElapsedMilliseconds / 1000 > coolDown)
        {
            Shoot();
            timer.Restart();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            gameObject.SetActive(false);
        }
    }

    void Shoot()
    {
        bullet = bulletPoolScript.GetPooledObject();
        bullet.transform.position = transform.GetChild(1).GetChild(0).position;
        bullet.transform.rotation = transform.GetChild(1).GetChild(0).rotation;
        bullet.SetActive(true);
    }
}
