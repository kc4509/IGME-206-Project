//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Base Unit or sprites abstract class 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    public Tile occupiedTile; //tile that sprite is on
    public Faction faction; //faction (hero/enemy)
    public string unitName;
    public int attack;
    public SpriteRenderer spriteRenderer;
    public int moveSpaces;
    public float currentHealth { get; set; }

    public int defense { get; set; }

    public int attackMax;
    public static int attackMin = 0;

    public abstract float Health();

    public abstract void Move();


}
