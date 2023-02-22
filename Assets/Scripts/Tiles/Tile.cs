//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Method involving tiles
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public string TileName;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject highlight;
    [SerializeField] private bool isWalkable;
    [SerializeField] public GameManager gameManager;
    [SerializeField] public GridManager gridManager;
    [SerializeField] public GameObject moveTile;
    [SerializeField] public GameObject attackTile;
    public MenuManager HUDManager;

    public int GCost;
    public int heuristicCost;
    public int totalCost;
    public Tile parent;
    public int x;
    public int y;

    public bool visited;


    public BaseUnit occupiedUnit;
    public bool Walkable => isWalkable && occupiedUnit == null;

    //mouse hover
    void OnMouseEnter()
    {
        highlight.SetActive(true);
        HUDManager.ShowTileInfo(this);
    }

    //mouse unhovers
    void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    //mouse click
    void OnMouseDown()
    {
        if (this.GetComponent<MountainTile>() != null)
        {
            return;
        }

        if (occupiedUnit is not BaseUnit || occupiedUnit == null)
        {
            gameManager.PlayerMove(gameManager.player, this);
        }

        else if (occupiedUnit is Enemy)
        {
            gameManager.PlayerAttack(gameManager.player, this);
        }
    }

    //set unit in right position, sometimes wrong z axis make sprites disappear
    public void SetUnit(BaseUnit unit)
    {
        if (unit.occupiedTile != null) unit.occupiedTile.occupiedUnit = null;
        unit.transform.position = transform.position;
        occupiedUnit = unit;
        unit.occupiedTile = this;
        if (unit is Player)
        {
            unit.transform.position =  new Vector3 (unit.transform.position.x, unit.transform.position.y, -1);
        }
        if (unit is Enemy)
        {
            unit.transform.position = new Vector3(unit.transform.position.x, unit.transform.position.y, -1);
        }
    }

    //restart a*
    public void ResetAStar()
    {
        GCost = 0;
        parent = null;
        heuristicCost = 0;
        totalCost = 0;
    }

    //list of children of a tile
    public List<Tile> GetChildren()
    {
        List<Tile> children = new List<Tile>();
        Tile[,] tiles = gridManager.GetGrid();

    
        //up
        int newY = this.y - 1;
        if (newY >= 0 && tiles[newY, this.x].GetComponent<MountainTile>() == null)
        {
            children.Add(tiles[newY, this.x]);
        }

        //down
        newY = this.y + 1;
        if (newY <= 8 && tiles[newY, this.x].GetComponent<MountainTile>() == null)
        {
            children.Add(tiles[newY, this.x]);
        }

        //right
        int newX = this.x + 1;
        if (newX <= 15 && tiles[this.y, newX].GetComponent<MountainTile>() == null)
        {
            children.Add(tiles[this.y, newX]);
        }

        //left
        newX = this.x - 1;
        if (newX >= 0 && tiles[this.y, newX].GetComponent<MountainTile>() == null)
        {
            children.Add(tiles[this.y, newX]);
        }
        
        return children;
    }
    
    //returns the heuristic cost of the move
    public void FindHeuristicCost(Player player)
    {
        Tile[,] tiles = gridManager.GetGrid();
        int goalRow = -1;
        int goalCol = -1;
        int currentRow = -1;
        int currentCol = -1;
        Tile goalTile = player.occupiedTile;

        for (int row = 0; row < tiles.GetLength(0); row++)
        {
            for (int col = 0; col < tiles.GetLength(1); col++)
            {
               if (goalRow != -1 && goalCol != -1 && currentCol != -1 && currentRow != -1)
                {
                    break;
                }

               if (tiles[row, col] == goalTile)
                {
                    goalRow = row;
                    goalCol = col;
                }

               if (tiles[row, col] == this)
                {
                    currentRow = row;
                    currentCol = col;
                }
            }
        }
        heuristicCost = Mathf.Abs(goalRow - currentRow) + Mathf.Abs(goalCol - currentCol);
    }

    //looks for the lowest cost tile to move to
    public static Tile FindLowestCostTile(List<Tile> tiles)
    {
        List<int> costList = new List<int>();
        
        foreach (Tile tile in tiles)
        {
            costList.Add(tile.totalCost);
        }

        int lowestcost = costList.Min();
        Tile lowestTile = tiles.Where(tile => tile.totalCost == lowestcost).First();
        return lowestTile;
    }
}