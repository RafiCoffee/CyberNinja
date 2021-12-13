using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager estadoJuego;

    public static Vector2 spawnPlayer;

    public bool isStatic;
    private bool isPaused = false;
    private bool cannotPause = false;

    private GameObject player;
    private GameObject pause;

    private Animator gameOverAnim;
    private Animator fade;

    private CheckpointManager checkpointManagerScript;
    private NinjaController playerScript;
    private BossBehaviour bossScript;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Destroy(gameObject);
        }

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

            Debug.Log(spawnPlayer);

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
            bossScript = GameObject.Find("Boss").GetComponent<BossBehaviour>();
            fade = GameObject.Find("Fade").GetComponent<Animator>();
            pause = GameObject.Find("Pause");

            pause.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Destroy(gameObject);
        }

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
                cannotPause = true;
                StartCoroutine(GameOver());
            }

            if (bossScript.vida <= 0)
            {
                fade.SetTrigger("Fade");
                cannotPause = true;
                StartCoroutine(Win());
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && !cannotPause)
            {
                pause.SetActive(true);
                Time.timeScale = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && isPaused && !cannotPause)
            {
                pause.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(1);
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(2.6f);
        SceneManager.LoadScene(0);
    }
}
