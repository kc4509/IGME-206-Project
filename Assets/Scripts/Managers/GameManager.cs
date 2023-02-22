//Kelly Chen
//Manages the game 

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState gameState;


    //public static event Action<GameState> gamestateChanged;

    private string choice; //placeholder for the string from selectionMenu
    public GridManager gridManager;
    public Player player;
    public List<Enemy> enemyList;
    public MenuManager HUDManager;

    public int enemySpawnNum;


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    /// Start is called before the first frame update
    void Start()
    {
        choice = ClassSelectionMenu.instance.playerChoice;
        ChangeState(GameState.GenerateGrid);
    }

    /// <summary>
    /// switches through the different states of the game
    /// </summary>
    /// <param name="newState"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void ChangeState(GameState newState)
    {
        gameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.instance.GenerateGrid();
                HUDManager.DisableTileInfo();
                break;
            case GameState.SpawnHeroes:
                player = UnitManager.instance.SpawnHeroes(choice);
                HUDManager.player = player;
                SkillTreeMenu.player = player;
                HUDManager.ShowSelectedPlayer();
                break;
            case GameState.SpawnEnemies:
                UnitManager.instance.SpawnEnemies(enemySpawnNum);
                enemyList = UnitManager.instance.enemyList;
                break;
            case GameState.PlayerTurn:
                UnitManager.instance.PlayerTurn();
                break;
            case GameState.EnemyTurn:
                foreach (Enemy enemy in enemyList)
                { 
                    UnitManager.instance.EnemyTurn(player, enemy);
                }
                ChangeState(GameState.PlayerTurn);

                break;
            //default:
                //throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }
    }

    /// Update is called once per frame
    void Update()
    {
        
    }

    //Player move method
    public void PlayerMove(Player selectedPlayer, Tile tile)
    {
        if (GameManager.instance.gameState != GameState.PlayerTurn) return;

        if (player.points == 40 && player.currentHealth > 0)
        {
            GameManager.instance.gameState = GameState.Victory;
        }

        if (!tile.moveTile.activeSelf)
        {
            return;
        }
        
        else
        {
            selectedPlayer.occupiedTile.occupiedUnit = null;
            selectedPlayer.occupiedTile = null;
            tile.SetUnit(selectedPlayer);

        }
        foreach (Tile clearTile in gridManager.GetGrid())
        {
            if (clearTile is GrassTile)
            {
                clearTile.moveTile.SetActive(false);
                clearTile.attackTile.SetActive(false);
            }
        }
        ChangeState(GameState.EnemyTurn);
    }

    //Play attack method
    public void PlayerAttack(Player selectedPlayer, Tile tile)
    {
        if (GameManager.instance.gameState != GameState.PlayerTurn) return;

        if (!tile.attackTile.activeSelf)
        {
            return;
        }
        
        if (tile.occupiedUnit is Enemy)
        {

            selectedPlayer.Attack((Enemy)tile.occupiedUnit);
            HUDManager.ShowTileInfo(tile);

            if (tile.occupiedUnit.currentHealth <= 0)
            {
                enemyList.Remove((Enemy)tile.occupiedUnit);

                if (tile.occupiedUnit.occupiedTile != null)
                {
                    tile.occupiedUnit.occupiedTile.moveTile.SetActive(true);
                    tile.occupiedUnit.occupiedTile = null;
                    tile.occupiedUnit.spriteRenderer.sprite = null;
                    tile.occupiedUnit = null;
                }

                selectedPlayer.score++;
                selectedPlayer.points += 10;


            }


        }
        foreach (Tile clearTile in gridManager.GetGrid())
        {
            if (clearTile is GrassTile)
            {
                clearTile.moveTile.SetActive(false);
                clearTile.attackTile.SetActive(false);
            }
        }

        Debug.Log("Enemy Count: " + enemyList.Count);

        if (enemyList.Count == 0)
        {
            UnitManager.instance.SpawnEnemies(4);
        }

         
        ChangeState(GameState.EnemyTurn);
       

    }


}

//Holds all the different gamestates
public enum GameState
{ 
    GenerateGrid,
    SpawnHeroes,
    SpawnEnemies,
    PlayerTurn, 
    EnemyTurn, 
    Victory, 
    Lose
}