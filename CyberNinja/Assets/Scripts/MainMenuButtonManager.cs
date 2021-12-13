using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtonManager : MonoBehaviour
{

    public GameObject[] buttons;
    public GameObject selector;

    public int position = 0;
    private int minPosition;
    private int maxPosition;

    public bool escenario;

    public AudioClip botones;

    private AudioSource menu;

    // Start is called before the first frame update
    void Start()
    {
        menu = GetComponent<AudioSource>();

        minPosition = 0;
        maxPosition = buttons.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        selector.transform.position = buttons[position].transform.position + new Vector3(500, 0, 0);

        if (Input.GetKeyDown(KeyCode.S))
        {
            menu.PlayOneShot(botones, 1f);
            if (position == maxPosition)
            {
                position = minPosition;
            }
            else
            {
                position++;
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            menu.PlayOneShot(botones, 1f);
            if (position == minPosition)
            {
                position = maxPosition;
            }
            else
            {
                position--;
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            menu.PlayOneShot(botones, 1f);
            MainMenu();
        }
    }

    public void MainMenu()
    {
        if (escenario)
        {
            switch (position)
            {
                case 0:
                    SceneManager.LoadScene(1);
                    Time.timeScale = 1;
                    break;

                case 1:
                    SceneManager.LoadScene(0);
                    Time.timeScale = 1;
                    break;
            }
        }
        else
        {
            switch (position)
            {
                case 0:
                    SceneManager.LoadScene(1);
                    Time.timeScale = 1;
                    break;

                case 1:
                    Application.Quit();
                    break;
            }
        }
    }
}
