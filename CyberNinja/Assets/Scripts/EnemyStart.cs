using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStart : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)]
    float speed = 5f;

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
            transform.position = Vector3.MoveTowards(transform.position, new Vector3 (transform.position.x, transform.position.y, 0), speed * Time.deltaTime);
        }
        else if (transform.position.z == 0)
        {
            transform.GetChild(0).GetComponent<EnemyBehaviour>().canShoot = true;
            enemyBC.enabled = true;
        }
    }
}
