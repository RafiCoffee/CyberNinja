using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    private GameManager gameManagerStaticScript;
    private void Awake()
    {
        gameManagerStaticScript = GameObject.Find("GameManagerStatic").GetComponent<GameManager>();

        if (gameManagerStaticScript.firstTime)
        {
            Destroy(gameObject);
        }
    }
}
