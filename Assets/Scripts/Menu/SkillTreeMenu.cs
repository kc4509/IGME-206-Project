//Kelly Chen IGME 206 | kc4509@g.rit.edu
//Skilltree Menu

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkillTreeMenu : MonoBehaviour
{

    public static Player player;
    private GameObject skillTreeButton;
    private GameObject skillTree;

    private GameObject movementObject;
    private GameObject defenseObject;
    private GameObject defense2Object;
    private GameObject attackObject;
    private GameObject attack2Object;
    private GameObject healObject;

    private Button movementButton;
    private Button defenseButton;
    private Button defense2Button;
    private Button attackButton;
    private Button attack2Button;

    private Color normalColor;
    private Color pressedColor;
    private Color selectedColor;
    private Color highlightedColor;

    private int movementCost;
    private int attackCost;
    private int attackCost2;
    private int defenseCost;
    private int defenseCost2;

    private int regainHealthCost;

    private Text pointText;


    private void Awake()
    {
        movementCost = 2;
        attackCost = 3;
        attackCost2 = 2;
        defenseCost = 2;
        defenseCost2 = 3;

        regainHealthCost = 1;

        player = UnitManager.instance.selectedPlayer;
        skillTreeButton = transform.Find("SkillTreeButton").gameObject;
        skillTree = skillTreeButton.transform.Find("SkillTree").gameObject;

        movementObject = skillTree.transform.Find("Movement+").gameObject;
        movementButton = movementObject.GetComponent<Button>();
        ChangeButtonText(movementObject, movementCost);

        defenseObject = skillTree.transform.Find("Defense+").gameObject;
        defenseButton = defenseObject.GetComponent<Button>();
        ChangeButtonText(defenseObject, defenseCost);

        defense2Object = skillTree.transform.Find("Defense++").gameObject;
        defense2Button = defense2Object.GetComponent<Button>();
        ChangeButtonText(defense2Object, defenseCost2);


        attackObject = skillTree.transform.Find("Attack+").gameObject;
        attackButton = attackObject.GetComponent<Button>();
        ChangeButtonText(attackObject, attackCost);

        attack2Object = skillTree.transform.Find("Attack++").gameObject;
        attack2Button = attack2Object.GetComponent<Button>();
        ChangeButtonText(attack2Object, attackCost2);


        healObject = skillTree.transform.Find("Heal").gameObject;
        ChangeButtonText(healObject, regainHealthCost);



        normalColor = new Color(0f, 1f, 0f);
        selectedColor = new Color(0f, 1f, 0f);
        highlightedColor = new Color(0f, 1f, 0f);
        pressedColor = new Color(0f, 128 / 255, 0f);

        pointText = skillTree.transform.Find("PointsText").gameObject.GetComponent<Text>();
    }

    //Heal's player
    public void Heal()
    {
        if (player.points >= regainHealthCost)
        {
            player.points -= regainHealthCost;
            UpdatePoints();
            player.Heal();
        }
    }

    //Shows the skill tree
    public void ShowSkillTree()
    {
        skillTree.SetActive(true);
        UpdatePoints();

        if (!player.movementSkillUnlocked)
        {
            DisableButton(defenseButton);
            DisableButton(defense2Button);
            DisableButton(attackButton);
            DisableButton(attack2Button);
        }

        else
        {
            if (!player.defenseSkillUnlocked)
            {
                DisableButton(defense2Button);
            }

            if (!player.attackSkillUnlocked)
            {
                DisableButton(attack2Button);
            }
        }
    }


    //hides the skill tree
    public void HideSkillTree()
    {
        skillTree.SetActive(false);
    }
    
    public void MovementButton()
    {
        if (player.points >= movementCost && !player.movementSkillUnlocked)
        {
            player.points -= movementCost;
            UpdatePoints();
            player.moveSpaces += 1;
            player.movementSkillUnlocked = true;
            ChangeButtonColor(movementButton);
            defenseButton.interactable = true;
            attackButton.interactable = true;
        }
    }

    public void Attack1Button()
    {
        if (player.points >= attackCost && player.movementSkillUnlocked && !player.attackSkillUnlocked)
        {
            player.points -= attackCost;
            UpdatePoints();
            player.attack += 5;
            player.attackSkillUnlocked = true;
            ChangeButtonColor(attackButton);
            attack2Button.interactable = true;
        }
    }

    public void Attack2Button()
    {
        if (player.points >= attackCost2 && player.attackSkillUnlocked && !player.attack2SkillUnlocked)
        {
            player.points -= attackCost2;
            UpdatePoints();
            player.attack += 5;
            player.attack2SkillUnlocked = true;
            ChangeButtonColor(attack2Button);
        }
    }

    public void Defense1Button()
    {
        if (player.points >= defenseCost && player.movementSkillUnlocked && !player.defenseSkillUnlocked)
        {
            player.points -= defenseCost;
            UpdatePoints();
            player.defense += 5;
            player.defenseSkillUnlocked = true;
            ChangeButtonColor(defenseButton);
            defense2Button.interactable = true;
        }
    }

    public void Defense2Button()
    {
        if (player.points >= defenseCost2 && player.defenseSkillUnlocked && !player.defense2SkillUnlocked)
        {
            player.points -= defenseCost2;
            UpdatePoints();
            player.defense += 5;
            player.defense2SkillUnlocked = true;
            ChangeButtonColor(defense2Button);
        }
    }

    private void DisableButton(Button button)
    {
        button.interactable = false;
    }

    private void ChangeButtonColor(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = normalColor;
        colors.highlightedColor = highlightedColor;
        colors.pressedColor = pressedColor;
        colors.selectedColor = selectedColor;

        button.colors = colors;
    }

    //Changes the button color after clicking on it
    private void ChangeButtonText(GameObject gameObject, int cost)
    {
        GameObject textObject = gameObject.transform.Find("Text (TMP)").gameObject;
        TextMeshProUGUI buttonText = textObject.GetComponent<TextMeshProUGUI>();

        buttonText.text += " " + cost + " Points";
    }

    //Current points player has 
    private void UpdatePoints()
    {
        pointText.text = "Points: " + player.points;
    }

    //quit game
    public void QuitGame()
    {
        Application.Quit();
    }

}
