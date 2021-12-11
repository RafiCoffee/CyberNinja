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
    public GameObject spine;
    public GameObject cañon;

    public Animator enemyAnim;

    private Stopwatch timer = new Stopwatch();

    public BulletPool bulletPoolScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("EnemyPoint");

        followPlayer = player.transform.position - transform.position;
        canShoot = false;

        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        enemyAnim.SetBool("CanShoot", canShoot);

        if (canShoot)
        {
            enemyAnim.SetLayerWeight(1, 1);
            spine.transform.LookAt(player.transform.position);

            if (player.transform.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
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
            StartCoroutine(Invencible());
            vida--;
        }
    }

    void Shoot()
    {
        bullet = bulletPoolScript.GetPooledObject();
        bullet.transform.position = cañon.transform.position;
        bullet.transform.rotation = cañon.transform.rotation;
        bullet.SetActive(true);
    }

    IEnumerator Invencible()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
