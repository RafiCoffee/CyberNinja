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

    private MainMenuManager menuManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        menuManagerScript = GameObject.Find("MainMenuManager").GetComponent<MainMenuManager>();

        minPosition = 0;
        maxPosition = buttons.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        selector.transform.position = buttons[position].transform.position + new Vector3(500, 0, 0);

        if (Input.GetKeyDown(KeyCode.S))
        {
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
            MainMenu();
        }
    }

    public void MainMenu()
    {
        switch (position)
        {
            case 0:
                SceneManager.LoadScene("Pruebas");
                break;

            case 1:
                Application.Quit();
                break;
        }
    }
}
