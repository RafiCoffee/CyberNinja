using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;

public class BossBehaviour : MonoBehaviour
{
    public float coolDown;
    public float bulletCoolDown;

    public int comportamientos;
    public int vida = 10;
    public int escudo = 3;

    private bool doMelee = false;
    public bool start = false;
    private bool changePoint = false;
    private bool invencible;

    private GameObject attackTrigger;
    private GameObject bullet;
    public GameObject bulletPoint1;
    public GameObject bulletPoint2;
    public GameObject laserPoint;
    private GameObject cañon;

    public ParticleSystem ChargeLaser1;
    public ParticleSystem ChargeLaser2;
    public ParticleSystem Lasersito;

    public Slider barraVida;

    private Animation bossAnim;
    private Animator shieldAnim;

    public BulletPool bulletPoolScript;
    private NinjaController playerScript;

    private Stopwatch timer = new Stopwatch();
    private Stopwatch bulletTimer = new Stopwatch();

    // Start is called before the first frame update
    void Start()
    {
        bossAnim = GetComponentInChildren<Animation>();

        attackTrigger = GameObject.Find("BossAttackTrigger");
        playerScript = GameObject.Find("Player").GetComponent<NinjaController>();
        shieldAnim = GetComponentInChildren<Animator>();

        attackTrigger.SetActive(false);

        timer.Start();
        bulletTimer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            barraVida.value = vida;

            if (timer.ElapsedMilliseconds / 1000 > coolDown)
            {
                if (!doMelee)
                {
                    coolDown = 3;
                    comportamientos = Random.Range(2, 6);
                }
                else
                {
                    coolDown = 0.1f;
                    comportamientos = Random.Range(0, 2);
                }

                switch (comportamientos)
                {
                    case 0:
                        bossAnim.Play("cabezazo2");
                        StartCoroutine(Cabezazo());
                        break;

                    case 1:
                        bossAnim.Play("pisoton");
                        StartCoroutine(Pisoton());
                        break;

                    case 2:
                        ChargeLaser1.Play();
                        ChargeLaser2.Play();
                        bossAnim.Play("rayolaser");
                        StartCoroutine(Laser());
                        break;

                    case 3:
                        bossAnim.Play("boss");
                        bulletTimer.Restart();
                        StartCoroutine(BossBullets());
                        break;

                    case 4:
                        bossAnim.Play("boss");
                        bulletTimer.Restart();
                        StartCoroutine(BossBullets());
                        break;

                    case 5:
                        bossAnim.Play("boss");
                        bulletTimer.Restart();
                        StartCoroutine(BossBullets());
                        break;
                }

                timer.Restart();
            }

            if (escudo <= 0)
            {
                timer.Stop();
                invencible = false;
                shieldAnim.SetTrigger("Quit");
                bossAnim.Play("marico");
                StartCoroutine(Caida());
            }
            else
            {
                invencible = true;
                shieldAnim.SetTrigger("Put");
            }

            if (vida == 0)
            {
                gameObject.SetActive(false);
            }

            if (playerScript.vida <= 0)
            {
                start = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (!invencible)
            {
                StartCoroutine(Invencible());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            doMelee = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            doMelee = false;
        }
    }

    void Shoot()
    {
        bullet = bulletPoolScript.GetPooledObject();
        bullet.transform.position = cañon.transform.position;
        bullet.transform.rotation = cañon.transform.rotation;
        bullet.SetActive(true);
    }

    IEnumerator Pisoton()
    {
        attackTrigger.transform.localPosition = new Vector2(-7.2f, -0.8f);
        attackTrigger.transform.localScale = new Vector2(6, 2);
        yield return new WaitForSeconds(1);
        attackTrigger.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        attackTrigger.SetActive(false);
        bossAnim.Play("IDLE");
    }

    IEnumerator Cabezazo()
    {
        attackTrigger.transform.localPosition = new Vector2(-5.6f, -0.8f);
        attackTrigger.transform.localScale = new Vector2(3, 2);
        attackTrigger.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        attackTrigger.SetActive(false);
        bossAnim.Play("IDLE");
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
        vida--;
        invencible = false;
    }

    IEnumerator BossBullets()
    {
        do
        {
            if (!changePoint)
            {
                cañon = bulletPoint1;
                changePoint = true;
            }
            else
            {
                cañon = bulletPoint2;
                changePoint = false;
            }
            yield return new WaitForSeconds(0.15f);
            Shoot();
        } while (bulletTimer.ElapsedMilliseconds / 1000 < bulletCoolDown);
        yield return new WaitForSeconds(0.6f);
        bossAnim.Play("IDLE");
    }

    IEnumerator Laser()
    {
        yield return new WaitForSeconds(1f);
        Lasersito.Play();
        yield return new WaitForSeconds(0.6f);
        Lasersito.Stop();
        bossAnim.Play("IDLE");
    }

    IEnumerator Caida()
    {
        yield return new WaitForSeconds(5f);
        timer.Restart();
        escudo = 3;
        bossAnim.Play("IDLE");
    }
}
