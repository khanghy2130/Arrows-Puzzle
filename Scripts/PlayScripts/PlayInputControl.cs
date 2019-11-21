using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayInputControl : MonoBehaviour
{
    // for hints
    public GameObject hintObject;
    public GameObject hintButton;
    public GameObject ArrowsBoard;
    public int hintRevealNum = 1; // next highest revealed
    public CreatePuzzle CreatePuzzle;
    public Canvas CanvasHintNum;
    public GameObject Result;

    // for result
    public GameObject PostGameplayButtons;

    private GameObject SceneManager;
    private ChangeScene ChangeScene;

    public GameObject RewardBonusImage;
    public GameObject RewardStarsHolder;
    public GameObject RewardStarsValue;

    private void Start()
    {
        SceneManager = GameObject.Find("_SceneManager");
        ChangeScene = SceneManager.GetComponent<ChangeScene>();
    }

    // for animation
    private int timer = 0;

    private void Update()
    {
        // result timeline controls
        if (CreatePuzzle.won)
        {
            timer++;
            if (ChangeScene.tutorialIndex == 0)
            {
                // SET UP after 120    (AND NOT TUTORIAL)
                if (timer == 120 && ChangeScene.tutorialIndex == 0)
                {
                    winSetUp();
                }
                // update PostGameplayButtons position
                if (timer > 150 && timer < 190)
                {
                    PostGameplayButtons.transform.position += new Vector3(0, 1, 0);
                }
            }
            else if (timer == 80){ ChangeScene.tutorialIndex++; ChangeScene.ChangeToScene("Tutorial"); }
        }
    }

    public int solvedStars;
    public int bonusStars;
    public int bonusStars2;
    public int finalExp;
    public int expCap;
    public int currentLv; // last to update
    public bool leveledUp; // if true then currentEXP and expCap are equal
    // called when win
    public void winSetUp()
    {
        PostGameplayButtons.SetActive(true);
        Destroy(ArrowsBoard);

        // add exp and lv
        // need to know level cap, hints used, bSize

        if (ChangeScene.bSize == 3) // easy
        {
            setExpAndLv(50, 5, 20);
            setStarValues(50, 10, 5, 20);
        }
        else if (ChangeScene.bSize == 4) // normal
        {
            setExpAndLv(500, 30, 200);
            setStarValues(500, 50, 50, 200);
        }
        else if(ChangeScene.bSize == 5) // hard
        {
            setExpAndLv(2800, 70, 1500);
            setStarValues(3000, 100, 200, 500);
        }

        Result.SetActive(true);
    }

    private void setStarValues(int baseValue, int hintReduce, int minBonus, int maxBonus)
    {
        solvedStars = baseValue - (hintReduce * (hintRevealNum - 1));
        bonusStars = Random.Range(minBonus, maxBonus + 1);
        float bonusPercent = PlayerPrefs.GetInt("bonus") / 20f; // convert bonus into percent
        bonusStars2 = (int) (bonusStars * bonusPercent);

        PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + solvedStars + bonusStars + bonusStars2);
    }

    private void setExpAndLv(int baseValue, int hintReduce, int noHintBonus)
    {
        if (hintRevealNum > 1) { noHintBonus = 0; }

        currentLv = PlayerPrefs.GetInt("level");
        expCap = currentLv * 85;
        finalExp = PlayerPrefs.GetInt("exp") + baseValue + noHintBonus - (hintReduce * (hintRevealNum - 1));
        if (finalExp >= expCap)
        {
            leveledUp = true;
            currentLv++;
            PlayerPrefs.SetInt("level", currentLv);
            PlayerPrefs.SetInt("exp", 0);

            if (currentLv % 5 == 0) // bonus
            {
                RewardBonusImage.SetActive(true);
                PlayerPrefs.SetInt("bonus", PlayerPrefs.GetInt("bonus") + 1);
            }
            else // stars
            {
                RewardStarsHolder.SetActive(true);
                int rewardAmount = 100 * (currentLv-1) + 200 * PlayerPrefs.GetInt("bonus");
                PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + rewardAmount);
                RewardStarsValue.GetComponent<TextMeshPro>().text = "+" + rewardAmount;
            }

        }
        else { PlayerPrefs.SetInt("exp", finalExp); }
    }

    public void confirmedHintActivation()
    {
        int bSize = CreatePuzzle.bSize;

        float cellSize = CreatePuzzle.getCellSize() / 7.5f; // default
        float s = 68 / (bSize - 0.2f); // for positioning

        bool foundIt = false;
        // find the pos of next hint
        for (int y = bSize - 1; y > -1; y--)
        {
            if (!foundIt)
            {
                for (int x = 0; x < bSize; x++)
                {
                    if (CreatePuzzle.solution2d[y, x] == hintRevealNum)
                    {
                        Canvas clone;
                        clone = Instantiate(CanvasHintNum,
                            new Vector3(s * (x - (bSize - 1) * 0.5f), -s * (y - (bSize - 1) * 0.5f) + 25, 0),
                            Quaternion.Euler(0, 0, 0));

                        // set parent
                        clone.transform.SetParent(ArrowsBoard.transform);

                        // re-scale
                        clone.transform.localScale = new Vector3(cellSize, cellSize, 0);

                        // set text number
                        clone.GetComponentInChildren<TextMeshProUGUI>().text = "" + hintRevealNum;

                        hintRevealNum++;
                        foundIt = true;
                        break;
                    }
                }
            }
        }

        int limit = bSize;
        if (limit == 5) { limit = 7; }
        if (hintRevealNum > limit) {
            hintObject.SetActive(false);
            hintButton.SetActive(false);
        }
    }

    // activate hint
    public void activateHint()
    {
        if (!CreatePuzzle.won && CreatePuzzle.startAnimateValue <= 0){
            SceneManager.GetComponent<AlertScript>().activateAlert(4);
        }
    }

    // quit (during play)
    public void quitToMenu() 
    {
        if (!CreatePuzzle.won)
        {
            SceneManager.GetComponent<AlertScript>().activateAlert(3);
        }
    }

    // quit (during result)
    public void goToMenu()
    {
        SceneManager.GetComponent<ChangeScene>().ChangeToScene("Menu");
        ChangeScene.tutorialIndex = 0;
    }

    // replay
    public void replay()
    {
        SceneManager.GetComponent<ChangeScene>().ChangeToScene("Play");
    }
}
