//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Enemy abstract class

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : BaseUnit, ICharacter
{
    protected List<Tile> avaliableTiles;
    protected List<Tile> openList;
    protected List<Tile> closedList;
    public Tile playerLastSeen;

    //Find path to player
    public abstract void FindPathPlayer(Player player, GridManager gridManager);
    //Find best tile to go next 
    public abstract Tile FindBestTile(Player player, GridManager gridManager);
    //Checks if surrounding tiles are walkable
    public abstract void GetAvalibleSpaces(GridManager gridManager);
    //attack
    public abstract void Attack(BaseUnit hero);


}
