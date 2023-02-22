//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Main menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //play game
   public void PlayGame()
    {
        SceneManager.LoadScene("ClassSelection");
    }

    //quit game
    public void QuitGame()
    {
        Application.Quit();
    }
}
