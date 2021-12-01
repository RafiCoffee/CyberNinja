using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronRespawning : MonoBehaviour
{
    [SerializeField, Range(1f, 5f)]
    float respawningTime = 1f;

    // Update is called once per frame
    void Update()
    {
        if (!transform.GetChild(0).gameObject.activeInHierarchy)
        {
            StartCoroutine(Respawning());
        }
    }

    IEnumerator Respawning()
    {
        yield return new WaitForSeconds(respawningTime);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
