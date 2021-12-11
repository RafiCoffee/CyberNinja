using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CloseBoss : MonoBehaviour
{
    public GameObject bossCameraPoint;

    public Material OFF;

    public MeshRenderer keyLock;
    public Animator doorAnim;

    public CinemachineVirtualCamera camara;

    private BossBehaviour boss;

    void Start()
    {
        boss = GameObject.Find("Boss").GetComponent<BossBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            keyLock.material = OFF;
            doorAnim.SetTrigger("Close");
            boss.start = true;
            camara.Follow = bossCameraPoint.transform;
        }
    }
}
