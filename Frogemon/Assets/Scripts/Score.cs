using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
    int highScore = 0;
    int pikachuCurrentY = 0;
    int levelStartScore = 0;
    int foodBonus = 10;

    public Sprite num0, num1, num2, num3, num4, num5, num6, num7, num8, num9;
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Pikachu moved further, update pikachu Current Y
    public void NewPikachuY(int pikaY)
    {
        pikachuCurrentY = pikaY;
        UpdateCurrentScore();
    }

    // Food was eaten, apply bonus points and set levelStart score.
    public void FoodEaten()
    {
        levelStartScore += pikachuCurrentY + foodBonus;
        pikachuCurrentY = 0;
        UpdateCurrentScore();
    }

    // Pikachu dies, reset current score
    public void PikachuDead()
    {
        pikachuCurrentY = 0;
        levelStartScore = 0;
        UpdateCurrentScore();
    }

    // Update High Score Image
    void UpdateHighScore()
    {
        //Figue out what each digit is
        int ones = highScore % 10;
        int tens = (highScore / 10) % 10;
        int hundreds = (highScore / 100) % 10;
        int thousands = highScore / 1000;

        // change High Score pic.
        transform.GetChild(1).GetChild(1).GetComponent<SpriteRenderer>().sprite = GetNumberSprite(ones);
        transform.GetChild(1).GetChild(2).GetComponent<SpriteRenderer>().sprite = GetNumberSprite(tens);
        transform.GetChild(1).GetChild(3).GetComponent<SpriteRenderer>().sprite = GetNumberSprite(hundreds);
        transform.GetChild(1).GetChild(4).GetComponent<SpriteRenderer>().sprite = GetNumberSprite(thousands);
    }

    //Update Current Score Image
    void UpdateCurrentScore()
    {
        //Figue out what each digit is
        int ones = (pikachuCurrentY + levelStartScore) % 10;
        int tens = ((pikachuCurrentY + levelStartScore) / 10) % 10;
        int hundreds = ((pikachuCurrentY + levelStartScore) / 100) % 10;
        int thousands = (pikachuCurrentY + levelStartScore) / 1000;

        // change Current Score pic.
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = GetNumberSprite(ones);
        transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = GetNumberSprite(tens);
        transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sprite = GetNumberSprite(hundreds);
        transform.GetChild(0).GetChild(3).GetComponent<SpriteRenderer>().sprite = GetNumberSprite(thousands);

        if (highScore < (pikachuCurrentY + levelStartScore))
        {
            highScore = pikachuCurrentY + levelStartScore;
            UpdateHighScore();
        }
    }

    Sprite GetNumberSprite(int number)
    {
        Sprite sprite;
        switch (number)
        {
            case 0:
                sprite = num0;
                break;
            case 1:
                sprite = num1;
                break;
            case 2:
                sprite = num2;
                break;
            case 3:
                sprite = num3;
                break;
            case 4:
                sprite = num4;
                break;
            case 5:
                sprite = num5;
                break;
            case 6:
                sprite = num6;
                break;
            case 7:
                sprite = num7;
                break;
            case 8:
                sprite = num8;
                break;
            case 9:
                sprite = num9;
                break;
            default:
                sprite = num0;
                break;
        }
        return sprite;
    }
}
