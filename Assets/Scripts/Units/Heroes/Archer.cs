//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Archer class 
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Archer : Player
{
    public void Awake()
    {
        attack = 5;
        fullHealth = 100;
        currentHealth = fullHealth;
        points = 0;
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
        moveSpaces = 3;
    }

    //heal
    public override void Heal()
    {
        currentHealth = fullHealth;
    }
}
