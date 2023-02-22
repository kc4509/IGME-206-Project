//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Golem enemy
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Golem : Enemy, ICharacter
{
    public void Awake()
    {
        currentHealth = 100;
        attack = 5;
        avaliableTiles = new List<Tile>();
        openList = new List<Tile>();
        closedList = new List<Tile>();
    }

    //attack
    public override void Attack(BaseUnit hero)
    {
        hero.currentHealth -= Mathf.Max(0, attack - defense);
    }

    //health
    public override float Health()
    {
        return currentHealth;
    }

    //move
    public override void Move()
    {
        moveSpaces = 2;
    }

    //find path to player
    public override void FindPathPlayer(Player player, GridManager gridManager)
    {
        Tile[,] tiles = gridManager.GetGrid();

        foreach (Tile tile in tiles)
        {
            tile.ResetAStar();
            tile.FindHeuristicCost(player);
        }

        openList.Clear();
        closedList.Clear();

        playerLastSeen = player.occupiedTile;
        Tile startingTile = occupiedTile;
        startingTile.GCost = 0;
        openList.Add(startingTile);
        Tile currentTile = startingTile;

        while (!openList.Contains(playerLastSeen))
        {
            List<Tile> children = currentTile.GetChildren();

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            for (int i = children.Count - 1; i > -1; i--)
            {
                Tile child = children[i];
                if (closedList.Contains(child))
                {
                    children.Remove(child);
                    continue;
                }

                if (child.parent == null)
                {
                    child.parent = currentTile;
                }
                else
                {
                    if (child.GCost > currentTile.GCost + 1)
                    {
                        child.parent = currentTile;
                    }
                }

                if (!openList.Contains(child))
                {
                    openList.Add(child);
                }

                child.GCost = child.parent.GCost + 1;

                child.totalCost = child.GCost + child.heuristicCost;
            }
            currentTile = Tile.FindLowestCostTile(openList);
        }
    }

    //checks for avaliable spaces around
    public override void GetAvalibleSpaces(GridManager gridManager)
    {
        List<Tile> children = new List<Tile>();
        Tile[,] tiles = gridManager.GetGrid();


        //up
        for (int i = 1; i <= moveSpaces; i++)
        {
            int newY = occupiedTile.y - i;
            if (newY < 0 || tiles[newY, occupiedTile.x].occupiedUnit is Enemy || tiles[newY, occupiedTile.x].GetComponent<MountainTile>() != null)
            {
                break;
            }
            children.Add(tiles[newY, occupiedTile.x]);
        }

        //down
        for (int i = 1; i <= moveSpaces; i++)
        {
            int newY = occupiedTile.y + i;
            if (newY > 8 || tiles[newY, occupiedTile.x].occupiedUnit is Enemy || tiles[newY, occupiedTile.x].GetComponent<MountainTile>() != null)
            {
                break;
            }
            children.Add(tiles[newY, occupiedTile.x]);
        }

        //right
        for (int i = 1; i <= moveSpaces; i++)
        {
            int newX = occupiedTile.x + i;
            if (newX > 15 || tiles[occupiedTile.y, newX].occupiedUnit is Enemy || tiles[occupiedTile.y, newX].GetComponent<MountainTile>() != null)
            {
                break;
            }
            children.Add(tiles[occupiedTile.y, newX]);
        }

        //right
        for (int i = 1; i <= moveSpaces; i++)
        {
            int newX = occupiedTile.x - i;
            if (newX < 0 || tiles[occupiedTile.y, newX].occupiedUnit is Enemy || tiles[occupiedTile.y, newX].GetComponent<MountainTile>() != null)
            {
                break;
            }
            children.Add(tiles[occupiedTile.y, newX]);
        }
        avaliableTiles = children;
    }

    //finds best tile to go next
    public override Tile FindBestTile(Player player, GridManager gridManager)
    {
        try
        {
            List<Tile> pathToPlayer = new List<Tile>();
            List<Tile> actualAvaliableSpaces = new List<Tile>();
            Dictionary<Tile, int> cost = new Dictionary<Tile, int>();
            Tile currentTile = player.occupiedTile;
            Tile[,] tiles = gridManager.GetGrid();
            do
            {
                pathToPlayer.Add(currentTile);
                currentTile = currentTile.parent;
            }
            while (currentTile != occupiedTile);

            actualAvaliableSpaces = pathToPlayer.Where(tile => avaliableTiles.Contains(tile)).ToList();

            foreach (Tile tile in actualAvaliableSpaces)
            {
                int tileCost = Mathf.Abs(occupiedTile.y - tile.y) + Mathf.Abs(occupiedTile.x - tile.x);
                cost.Add(tiles[tile.y, tile.x], tileCost);
            }

            int highestCost = cost.Values.Max();

            foreach (KeyValuePair<Tile, int> pair in cost)
            {
                if (pair.Value == highestCost)
                {
                    return pair.Key;
                }
            }

            return cost.First().Key;
        }

        catch
        {
            return null;
        }

    }
}
