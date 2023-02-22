//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Manages the units (player and enemies)
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    private List<ScriptableUnit> units;

    public Player selectedPlayer;
    public List<Enemy> enemyList;

    public Player archerPrefab;
    public Player warriorPrefab;
    public Player magicianPrefab;
    public Player sagePrefab;

    public GridManager gridManager;
    public GameManager gameManager;
    public MenuManager hudManager;
    [SerializeField]

    private void Awake()
    {
        instance = this;
        enemyList = new List<Enemy>();
        units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
        DontDestroyOnLoad(this.gameObject);
    }

    //generates the player
    public Player SpawnHeroes(string name)
    {
        if (name == "Archer")
        {
            selectedPlayer = Instantiate(archerPrefab);
        }
        if (name == "Warrior")
        {
            selectedPlayer = Instantiate(warriorPrefab);
        }
        if (name == "Sage")
        {
            selectedPlayer = Instantiate(sagePrefab);
        }
        if (name == "Magician")
        {
            selectedPlayer = Instantiate(magicianPrefab);
        }
        var randomSpawnTile = GridManager.instance.GetHeroSpawnTile();
        randomSpawnTile.SetUnit(selectedPlayer);


        GameManager.instance.ChangeState(GameState.SpawnEnemies);
        return selectedPlayer;
    }

    //spawns the enemies
    public void SpawnEnemies(int enemyNum)
    {
        for (int i = 0; i < enemyNum; i++)
        {
            Enemy enemyPrefab = GetRandomUnit<Enemy>(Faction.Enemy);
            Enemy spawnedEnemy = Instantiate(enemyPrefab);
            Tile randomSpawnTile = GridManager.instance.GetEnemySpawnTile();
            enemyList.Add(spawnedEnemy);

            randomSpawnTile.SetUnit(spawnedEnemy);
            
        }

        GameManager.instance.ChangeState(GameState.PlayerTurn);
    }

    //Player's turn checks for avaliable boxes to travel to 
    public void PlayerTurn()
    {
        Tile[,] tiles = gridManager.GetGrid();
        selectedPlayer.Move();
        Tile playerTile = selectedPlayer.occupiedTile;
        
        //up
        for(int i = 1; i <= selectedPlayer.moveSpaces; i++)
        {
            int newY = playerTile.y - i;
            if (newY < 0 || tiles[newY, playerTile.x].GetComponent<MountainTile>() != null)
            {
                break;
            }
            if (tiles[newY, playerTile.x].occupiedUnit is Enemy)
            {
                tiles[newY, playerTile.x].attackTile.SetActive(true);
            }
            else { tiles[newY, playerTile.x].moveTile.SetActive(true); }
            
        }

        //down
        for (int i = 1; i <= selectedPlayer.moveSpaces; i++)
        {
            int newY = playerTile.y + i;
            if (newY > 8 || tiles[newY, playerTile.x].GetComponent<MountainTile>() != null)
            {
                break;
            }
            if (tiles[newY, playerTile.x].occupiedUnit is Enemy)
            {
                tiles[newY, playerTile.x].attackTile.SetActive(true);
            }
            else { tiles[newY, playerTile.x].moveTile.SetActive(true); }
        }

        //left
        for (int i = 1; i <= selectedPlayer.moveSpaces; i++)
        {
            int newX = playerTile.x - i;
            if (newX < 0 || tiles[playerTile.y, newX].GetComponent<MountainTile>() != null)
            {
                break;
            }
            if (tiles[playerTile.y, newX].occupiedUnit is Enemy)
            {
                tiles[playerTile.y, newX].attackTile.SetActive(true);
            }
            else { tiles[playerTile.y, newX].moveTile.SetActive(true); }
        }

        //right
        for (int i = 1; i <= selectedPlayer.moveSpaces; i++)
        {
            int newX = playerTile.x + i;
            if (newX > 15 || tiles[playerTile.y, newX].GetComponent<MountainTile>() != null)
            {
                break;
            }
            if (tiles[playerTile.y, newX].occupiedUnit is Enemy)
            {
                tiles[playerTile.y, newX].attackTile.SetActive(true);
            }
            else { tiles[playerTile.y, newX].moveTile.SetActive(true); }
        }
    }

    //Enemy's turn uses A* to find closest path to player
    public void EnemyTurn(Player player, Enemy enemy)
    {
        if (enemy.occupiedTile != null)
        {
            //find the path to the player if it's outdated or not initialized
            if (enemy.playerLastSeen == null || enemy.playerLastSeen != player.occupiedTile)
            {
                enemy.Move();
                enemy.FindPathPlayer(player, gridManager);
            }

            enemy.GetAvalibleSpaces(gridManager);
            Tile movingTile = enemy.FindBestTile(player, gridManager);

            if (movingTile != null)
            {
                //attack if moving tile is playerSpace
                if (movingTile == player.occupiedTile)
                {
                    enemy.Attack(player);
                    if (player.currentHealth <= 0)
                    {
                        SceneManager.LoadScene("GameOver");
                    }
                }

                else
                {
                    enemy.occupiedTile.occupiedUnit = null;
                    enemy.occupiedTile = null;
                    movingTile.SetUnit(enemy);
                }
            }
        }

        hudManager.ShowSelectedPlayer();
        hudManager.ShowPlayerPoints();
        gameManager.ChangeState(GameState.PlayerTurn);
    }


   //Generating random units
    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        return (T)units.Where(u => u.faction == faction).OrderBy(o => Random.value).First().unitPrefab;
    } 


    //Set the selected player
    public void SetSelectedPlayer(Player player)
    {
        selectedPlayer = player;
        MenuManager.instance.ShowSelectedPlayer();
    }
}
