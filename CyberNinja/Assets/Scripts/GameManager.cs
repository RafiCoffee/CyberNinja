using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager estadoJuego;
    
    public static Vector2 spawnPlayer;

    private GameObject player;

    private CheckpointManager checkpointManagerScript;

    private void Awake()
    {
        if (estadoJuego == null)
        {
            estadoJuego = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (estadoJuego != this)
        {
            Destroy(gameObject);
        }

        checkpointManagerScript = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();

        player = GameObject.Find("Player");

        player.transform.position = spawnPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkpointManagerScript = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }

        spawnPlayer = checkpointManagerScript.spawnPoint;
    }
}
