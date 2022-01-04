using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaHitSound : MonoBehaviour
{
    private AudioSource katanaAudio;

    public ParticleSystem hitParticle;
    public AudioClip katanaHit;

    public bool enemies;
    // Start is called before the first frame update
    void Start()
    {
        katanaAudio = GetComponentInParent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemies)
        {
            if (collision.gameObject.layer == 8)
            {
                katanaAudio.PlayOneShot(katanaHit, 0.5f);
                hitParticle.Play();
            }
        }
        else
        {
            if (collision.gameObject.layer != 0 && collision.gameObject.layer != 7 && collision.gameObject.layer != 9 && collision.gameObject.layer != 10 && collision.gameObject.layer != 15 && collision.gameObject.layer != 17)
            {
                katanaAudio.PlayOneShot(katanaHit, 0.5f);
                hitParticle.Play();
            }
        }
    }
}
