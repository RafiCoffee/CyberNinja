using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int speed;

    private NinjaController playerScript;

    // Start is called before the first frame update
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
        if (collision.collider.gameObject.layer == 7)
        {
            Debug.Log("Te di");
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            if (playerScript.canReturn)
            {
                Debug.Log("Devuelto");
                transform.parent.parent.GetChild(0).GetComponent<EnemyBehaviour>().vida--;
            }
            else
            {
                Debug.Log("No Devuelto");
                gameObject.SetActive(false);
            }
        }
    }
}
