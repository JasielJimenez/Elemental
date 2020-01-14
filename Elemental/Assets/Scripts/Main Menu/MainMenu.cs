using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public bool atMainMenu = true;
    public GameObject mainMenu;
    public GameObject menuChoices;
    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        menuChoices.SetActive(false);
        atMainMenu = true;
    }

    void endGame()
    {
        #if UNITY_EDITOR
          UnityEditor.EditorApplication.isPlaying = false;
        #else
          Application.Quit();
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey && atMainMenu == true)
        {
            mainMenu.SetActive(false);
            menuChoices.SetActive(true);
            atMainMenu = false;
        }
        else if(Input.GetKeyUp("escape") && atMainMenu == false)
        {
            mainMenu.SetActive(true);
            menuChoices.SetActive(false);
            atMainMenu = true;
        }
    }
}
