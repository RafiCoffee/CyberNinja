using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UDebug = UnityEngine.Debug;

public class EnemyBehaviour : MonoBehaviour
{
    private Vector2 followPlayer;

    public int vida;

    public float coolDown;

    public bool canShoot = false;
    public bool haveShield;
    private bool invencible = false;

    private GameObject bullet;
    private GameObject player;
    public GameObject spine;
    public GameObject cañon;

    public Animator enemyAnim;
    public Animator shieldAnim;

    private Stopwatch timer = new Stopwatch();

    private BulletPool bulletPoolScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("EnemyPoint");
        bulletPoolScript = GetComponentInChildren<BulletPool>();

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
            if (vida < 3)
            {
                shieldAnim.SetTrigger("Quit");
                gameObject.layer = 8;
                haveShield = false;
            }
        }

        if (vida == 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (!invencible)
            {
                StartCoroutine(Invencible());
                vida--;
            }
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
        invencible = true;
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(true);
        invencible = false;
    }
}
