using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int speed;

    public bool boss;

    public GameObject hitParticle;

    private NinjaController playerScript;

    private void OnEnable()
    {
        StartCoroutine(Destruir());
    }

    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<NinjaController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 6 || collision.collider.gameObject.layer == 3 || collision.collider.gameObject.layer == 16)
        {
            gameObject.SetActive(false);
            Instantiate(hitParticle, transform.position, Quaternion.identity);
        }

        if (collision.collider.gameObject.layer == 7)
        {
            gameObject.SetActive(false);
            Instantiate(hitParticle, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            if (playerScript.canReturn)
            {
                if (!boss)
                {
                    transform.parent.parent.GetChild(0).GetComponent<EnemyBehaviour>().vida--;
                    Instantiate(hitParticle, transform.parent.parent.position, Quaternion.identity);
                }
                else
                {
                    transform.parent.parent.GetComponent<BossBehaviour>().escudo--;
                    Instantiate(hitParticle, transform.parent.parent.GetComponent<BossBehaviour>().laserPoint.transform.position, Quaternion.identity);
                }
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
                Instantiate(hitParticle, transform.position, Quaternion.identity);
            }
        }
    }

    IEnumerator Destruir()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
