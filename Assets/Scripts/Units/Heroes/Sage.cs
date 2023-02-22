//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Sage class
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sage : Player
{
    public void Awake()
    {
        attack = 15;
        fullHealth = 100;
        currentHealth = fullHealth;
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