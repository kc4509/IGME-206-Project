//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Class Selection Menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//registers player's choice of class
public class ClassSelectionMenu : MonoBehaviour
{
    public static ClassSelectionMenu instance;
    public string playerChoice;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void SelectArcher()
    {
        playerChoice = "Archer";
        SceneManager.LoadScene("Game");
    }
    public void SelectWarrior()
    {
        playerChoice = "Warrior";
        SceneManager.LoadScene("Game");
    }
    public void SelectSage()
    {
        playerChoice = "Sage";
        SceneManager.LoadScene("Game");
    }
    public void SelectMagician()
    {
        playerChoice = "Magician";
        SceneManager.LoadScene("Game");
    }
}
