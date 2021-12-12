using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager estadoJuego;
    
    public static Vector2 spawnPlayer;

    public bool isStatic;

    private GameObject player;

    private Animator gameOverAnim;

    private CheckpointManager checkpointManagerScript;
    private NinjaController playerScript;

    private void Awake()
    {
        if (isStatic)
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
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!isStatic)
        {
            playerScript = GameObject.Find("Player").GetComponent<NinjaController>();
            gameOverAnim = GameObject.Find("GameOverScreen").GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStatic)
        {
            checkpointManagerScript = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();

            spawnPlayer = checkpointManagerScript.spawnPoint;
        }
        else
        {
            if (playerScript.vida <= 0)
            {
                gameOverAnim.SetTrigger("Muerte");
                StartCoroutine(GameOver());
            }
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2.6f);
        SceneManager.LoadScene(1);
    }
}
