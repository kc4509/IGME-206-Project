//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Player abstract class

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : BaseUnit, ICharacter
{
    public int points;
    public bool movementSkillUnlocked;
    public bool defenseSkillUnlocked;
    public bool defense2SkillUnlocked;
    public bool attackSkillUnlocked;
    public bool attack2SkillUnlocked;
    public int score;
    public int fullHealth;

    //attack 
    public abstract void Attack(BaseUnit enemy);

    //heal
    public abstract void Heal();
}
