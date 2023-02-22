//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Manages the menu and displays

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public void Awake()
    {
        instance = this;
    }


    public Player player;
    Text enemyText;
    Text playerText;
    Text playerPoints;

    private void Start()
    {
        enemyText = transform.Find("EnemyText").gameObject.GetComponent<Text>();
        playerText = transform.Find("PlayerText").gameObject.GetComponent<Text>();
        playerPoints = transform.Find("Points").gameObject.GetComponent<Text>();
    }

    //show information of tile on screen
    public void ShowTileInfo(Tile tile)
    {
        if (tile.occupiedUnit is not Enemy)
        {
            DisableTileInfo();
            return;
        }
        Enemy enemy = (Enemy)tile.occupiedUnit;

        enemyText.gameObject.SetActive(true);
        
        enemyText.text = enemy.unitName + "\nHP: " + enemy.currentHealth;
    }

    //disables information on screen
    public void DisableTileInfo()
    {
        enemyText.gameObject.SetActive(false);
    }

    //shows the current health of player
    public void ShowSelectedPlayer()
    {
        playerText.text = "HP: " + player.currentHealth;
    }

    //shows the current points of player
    public void ShowPlayerPoints()
    {
        playerPoints.text = "Points: " + player.points;
    }
}
