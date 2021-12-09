using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBoss : MonoBehaviour
{
    public Animator doorAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            doorAnim.SetTrigger("Close");
        }
    }
}
