using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLock : MonoBehaviour
{
    private bool isOpen = false;

    public Material ON;

    public GameObject keyCard;

    private MeshRenderer keyLock;
    public Animator keyCardAnim;
    public Animator doorAnim;

    void Start()
    {
        keyLock = GetComponentInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            keyCard.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isOpen && collision.gameObject.layer == 7)
        {
            keyCard.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isOpen)
            {
                keyCardAnim.SetTrigger("Open");
                StartCoroutine(Validate());
                isOpen = true;
            }
        }
    }

    IEnumerator Validate()
    {
        yield return new WaitForSeconds(2);
        doorAnim.SetTrigger("Open");
        keyLock.material = ON;
    }
}
