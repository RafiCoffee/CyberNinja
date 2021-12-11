using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Vector2 spawnPoint;

    public int checkpoint = 0;


    // Update is called once per frame
    void Update()
    {
        if (checkpoint < transform.childCount)
        {
            if (transform.GetChild(checkpoint).GetComponent<Checkpoint>().isEnabled)
            {
                spawnPoint = transform.GetChild(checkpoint).position;
                checkpoint++;
            }
        }

        switch(checkpoint)
        {
            case 2:
                transform.GetChild(0).gameObject.SetActive(false);
                break;

            case 3:
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
                break;
        }
    }
}
