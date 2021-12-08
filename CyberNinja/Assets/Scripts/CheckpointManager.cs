using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Vector2 spawnPoint;

    private int checkpoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(checkpoint);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetChild(checkpoint).GetComponent<Checkpoint>().isEnabled)
        {
            spawnPoint = transform.GetChild(checkpoint).position;
            if (checkpoint < transform.childCount - 1)
            {
                checkpoint++;
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(checkpoint);
        }

        if (checkpoint != 0)
        {
            for (int i = checkpoint - 1; i >= 0; i--)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
