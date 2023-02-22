
//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Game over menu 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    Text scoreText;
    void Awake()
    {
        scoreText = transform.Find("ScoreText").gameObject.GetComponent<Text>();

        scoreText.text = "Score: " + GameManager.instance.player.score;
    }

    //reset game button
    public void ResetGame()
    {
        SceneManager.LoadScene("ClassSelection");
    }

    //exit game button
    public void ExitGame()
    {
        Application.Quit();
    }
}
