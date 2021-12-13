using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStart : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)]
    float speed = 5f;

    public bool isOnRadious = false;

    public Animator enemyAnim;
    private BoxCollider2D enemyBC;

    // Start is called before the first frame update
    void Start()
    {
        enemyBC = transform.GetChild(0).GetComponent<BoxCollider2D>();

        enemyBC.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3 (transform.position.x, transform.position.y, 0), speed * Time.deltaTime);
        }
        else if (transform.position.z == 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            enemyBC.enabled = true;
            if (isOnRadious)
            {
                transform.GetChild(0).GetComponent<EnemyBehaviour>().canShoot = true;
            }
        }

        enemyAnim.SetBool("CanShoot", isOnRadious);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            transform.GetChild(0).GetComponent<EnemyBehaviour>().canShoot = true;
            isOnRadious = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            transform.GetChild(0).GetComponent<EnemyBehaviour>().canShoot = true;
            isOnRadious = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            transform.GetChild(0).GetComponent<EnemyBehaviour>().canShoot = false;
            isOnRadious = false;
        }
    }
}
