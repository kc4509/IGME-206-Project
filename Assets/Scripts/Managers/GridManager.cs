///Kelly Chen IGME 206
///Manages the game grid

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Runtime.CompilerServices;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    [SerializeField] private int width, height;
    [SerializeField] private Tile grassTile, mountainTile;

    [SerializeField] private Transform cam;
    [SerializeField] private MenuManager HUDManager;

    //private Dictionary<Vector2, Tile> tiles;
    private Tile[,] tiles; 
    public GameManager gameManager;


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //Generates grid
    public void GenerateGrid()
    {
        do
        {
            tiles = new Tile[9, 16];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var randomTile = UnityEngine.Random.Range(0, 6) == 3 ? mountainTile : grassTile;
                    var spawnedTile = Instantiate(randomTile, new Vector3(x, height - 1 - y), Quaternion.identity);
                    spawnedTile.name = $"Tile  {y} {x}";
                    spawnedTile.gameManager = gameManager;
                    spawnedTile.gridManager = this;
                    spawnedTile.HUDManager = HUDManager;
                    spawnedTile.x = x;
                    spawnedTile.y = y;
                    spawnedTile.visited = false;
                    tiles[y, x] = spawnedTile;
                }
            }
        } while(!ValidGridGeneration());
        

        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);

        GameManager.instance.ChangeState(GameState.SpawnHeroes);
    }

    //Gets the tile that the hero spawns on
    public Tile GetHeroSpawnTile()
    {
        // Tile tile = tiles[UnityEngine.Random.Range(0, 9), UnityEngine.Random.Range(0, 16)];
        Tile tile = tiles[0, 0];
        while (!tile.Walkable)
        {
            tile = tiles[UnityEngine.Random.Range(0, 9), UnityEngine.Random.Range(0, 16)];
        }
        return tile;
    }

    //Gets the Enemy spawn tile
    public Tile GetEnemySpawnTile()
    {
        //Tile tile = tiles[UnityEngine.Random.Range(0, 9), UnityEngine.Random.Range(0, 16)];
        Tile tile = tiles[0, 1];
        while (!tile.Walkable || tile.occupiedUnit != null)
        {
            tile = tiles[UnityEngine.Random.Range(0, 9), UnityEngine.Random.Range(0, 16)];
        }
        return tile;
    }

    public Tile[,] GetGrid() { return tiles; }

    //checks whether the path is valid and how many tiles need to be avalible
    public bool ValidGridGeneration()
    {
        int totalTileNum = tiles.GetLength(0) * tiles.GetLength(1);
        int mountainTiles = 0;

        foreach (Tile tile in tiles)
        {
            if (tile.GetComponent<MountainTile>() != null)
            {
                mountainTiles++;
            }
        }

        int grassTiles = totalTileNum - mountainTiles;

        //start on a random grass cell

        Tile currentTile;

        do
        {
            int maxRow = tiles.GetLength(0);
            int maxCol = tiles.GetLength(1);
            int row = UnityEngine.Random.Range(0, maxRow);
            int col = UnityEngine.Random.Range(0, maxCol);

            currentTile = tiles[row, col];

        } while (currentTile.GetComponent<MountainTile>() != null);

        //find check to see if all the grass Cells are avaible
        
        //DEPTH FIRST SEARCH


        Queue<Tile> queue = new Queue<Tile>();

        queue.Enqueue(currentTile);

        currentTile.visited = true;

        int visitNum = 1;


        //keep going unitl there is nothing in the queue
        while (queue.Count != 0)
        {
            Tile nextTile = GetUnvisitedNeighbors(queue.Peek());

            if (nextTile != null)
            {
                queue.Enqueue(nextTile);
                nextTile.visited = true;
                visitNum++;
            }

            else
            {
                queue.Dequeue();
            }
        }

        return visitNum == grassTiles;
    }

    private Tile GetUnvisitedNeighbors(Tile tile)
    {
        if (tile == null)
        {
            return null;
        }


        List<Tile> neightbors = tile.GetChildren();
        List<Tile> neighborsWithoutMountains = neightbors.Where(tile => !tile.visited).ToList();

        if (neighborsWithoutMountains.Count == 0)
        {
            return null;
        }

        return neighborsWithoutMountains.First();
    }
}
