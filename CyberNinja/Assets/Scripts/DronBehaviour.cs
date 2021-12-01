using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronBehaviour : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)]
    float speed = 5f;

    private Vector3 initialPosition;
    private Vector3 pointPosition;

    private bool up;
    private bool dontMove;

    private void Awake()
    {
        initialPosition = transform.position;
        dontMove = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        pointPosition = transform.parent.transform.GetChild(1).GetChild(0).position;

        up = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.parent.transform.GetChild(1).GetChild(0).position, transform.position, Color.red);
        Debug.DrawLine(transform.parent.transform.GetChild(1).GetChild(1).position, transform.position, Color.red);

        if (!dontMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointPosition, speed * Time.deltaTime);

            if (transform.position == pointPosition && up)
            {
                pointPosition = transform.parent.transform.GetChild(1).GetChild(1).position;
                up = false;
            }
            else if (transform.position == pointPosition && !up)
            {
                pointPosition = transform.parent.transform.GetChild(1).GetChild(0).position;
                up = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        transform.position = initialPosition;
        dontMove = true;
        StartCoroutine(Invencible());
    }

    IEnumerator Invencible()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        transform.GetChild(0).gameObject.SetActive(true);
        dontMove = false;
    }
}
