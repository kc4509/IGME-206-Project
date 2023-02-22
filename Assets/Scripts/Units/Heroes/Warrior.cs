//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Warrior class
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Player
{
    public void Awake()
    {
        attack = 20;
        fullHealth = 100;
        Heal();
        score = 0;
    }

    //attack
    public override void Attack(BaseUnit enemy)
    {
        enemy.currentHealth -= Math.Max(0, attack - defense);
    }

    //health
    public override float Health()
    {
        return currentHealth;
    }

    //move
    public override void Move()
    {
        moveSpaces = 1;
    }

    //heal
    public override void Heal()
    {
        currentHealth = fullHealth;    
    }

}
