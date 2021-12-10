using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBoss : MonoBehaviour
{

    public Material OFF;

    public MeshRenderer keyLock;
    public Animator doorAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            keyLock.material = OFF;
            doorAnim.SetTrigger("Close");
        }
    }
}
