//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Magician class
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician : Player
{
    public void Awake()
    {
        attack = 10;
        fullHealth = 100;
        Heal();
        score = 0;
    }

    //attack
    public override void Attack(BaseUnit enemy)
    {
        enemy.currentHealth -= Mathf.Max(0, attack - enemy.defense);
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

    //heal
    public override void Heal()
    {
        currentHealth = fullHealth;
    }
}
