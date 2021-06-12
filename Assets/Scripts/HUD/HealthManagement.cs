using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManagement : MonoBehaviour
{
    public List<Image> hearts;

    public Sprite heartFull;
    public Sprite heart34;
    public Sprite heart12;
    public Sprite heart14;
    public Sprite heart0;

    //Where health is 0-12
    public void UpdateHealthDisplay(int health)
    {
        if(health > 8)
        {
            hearts[0].sprite = heartFull;
            hearts[1].sprite = heartFull;
            hearts[2].sprite = CalculateRemainder(health % 4);
        }
        else if(health > 4)
        {
            hearts[0].sprite = heartFull;
            hearts[1].sprite = CalculateRemainder(health % 4);
            hearts[2].sprite = heart0;
        }
        else if(health > 0)
        {
            hearts[0].sprite = CalculateRemainder(health);
            hearts[1].sprite = heart0;
            hearts[2].sprite = heart0;
        }
        else
        {
            hearts[0].sprite = heart0;
            hearts[1].sprite = heart0;
            hearts[2].sprite = heart0;
        }
    }

    private Sprite CalculateRemainder(int health)
    {
        switch (health)
        {
            case 0:
                return heartFull;
            case 1:
                return heart14;
            case 2:
                return heart12;
            case 3:
                return heart34;
            case 4:
            default:
                return heartFull;
        }
    }
}
