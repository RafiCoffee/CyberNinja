using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLock : MonoBehaviour
{
    private bool isOpen = false;

    public Material ON;
    public Light luz;

    public GameObject keyCard;
    public GameObject E;

    public AudioClip door;

    private MeshRenderer keyLock;
    public Animator keyCardAnim;
    public Animator doorAnim;
    private AudioSource keyLockAudio;

    private NinjaController playerScript;

    void Start()
    {
        keyLock = GetComponentInChildren<MeshRenderer>();
        keyLockAudio = GetComponent<AudioSource>();
        playerScript = GameObject.Find("Player").GetComponent<NinjaController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7 && playerScript.haveKey)
        {
            E.SetActive(true);
            keyCard.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isOpen && collision.gameObject.layer == 7 && playerScript.haveKey)
        {
            E.SetActive(false);
            keyCard.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7 && playerScript.haveKey)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isOpen)
            {
                E.SetActive(false);
                keyCardAnim.SetTrigger("Open");
                StartCoroutine(Validate());
                isOpen = true;
            }
        }
    }

    IEnumerator Validate()
    {
        yield return new WaitForSeconds(2);
        keyLockAudio.Play();
        yield return new WaitForSeconds(2);
        keyLockAudio.PlayOneShot(door, 0.5f);
        doorAnim.SetTrigger("Open");
        keyLock.material = ON;
        luz.color = Color.green;
    }
}
