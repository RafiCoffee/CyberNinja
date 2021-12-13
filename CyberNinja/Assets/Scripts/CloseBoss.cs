using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CloseBoss : MonoBehaviour
{
    public GameObject bossCameraPoint;
    public GameObject barraBoss;
    public GameObject luces;

    public Material OFF;

    public Light luz;

    private bool first = false;

    public GameObject pared;

    public AudioClip musicaBoss;

    public MeshRenderer keyLock;
    public Animator doorAnim;
    public AudioSource camaraAudio;

    public CinemachineVirtualCamera camara;

    private BossBehaviour boss;

    void Start()
    {
        boss = GameObject.Find("Boss").GetComponent<BossBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7 && !first)
        {
            luces.SetActive(true);
            luz.color = Color.red;
            keyLock.material = OFF;
            doorAnim.SetTrigger("Close");
            boss.start = true;
            camara.Follow = bossCameraPoint.transform;
            camara.m_Lens.FieldOfView = 70;
            barraBoss.SetActive(true);
            camaraAudio.clip = musicaBoss;
            camaraAudio.Play();
            pared.SetActive(true);
            first = true;
        }
    }
}
