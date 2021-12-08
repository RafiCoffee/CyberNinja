using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UDebug = UnityEngine.Debug;

public class EnemyBehaviour : MonoBehaviour
{
    public Vector2 followPlayer;

    public int vida;

    public float coolDown;

    public bool canShoot = false;
    public bool haveShield;

    private GameObject bullet;
    private GameObject player;

    private Stopwatch timer = new Stopwatch();

    public BulletPool bulletPoolScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        followPlayer = player.transform.position - transform.position;
        canShoot = false;

        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(1).LookAt(player.transform.position, Vector2.up);
        transform.GetChild(1).position = transform.GetChild(0).GetChild(0).position;

        if (player.transform.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (timer.ElapsedMilliseconds / 1000 > coolDown && canShoot)
        {
            Shoot();
            timer.Restart();
        }
        
        if (haveShield)
        {
            if (vida <= 3)
            {
                transform.GetChild(2).gameObject.SetActive(false);
                gameObject.layer = 8;
                haveShield = false;
            }
        }

        if (vida == 0)
        {
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            UDebug.Log(player.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            vida--;
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
