using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMasterScript : MonoBehaviour   
{
    // this script passes the looted data to the cards
    // and controls animation

    public AudioSource clicking;

    private bool canExit;
    private int maxCount = 2;
    private int countingValue = 0;
    private CustomizeStorage cs;
    public GameObject[] cardObjects;
    public cardData[] cardDatas;

    // cards object
    public Sprite[] cardSkins = new Sprite[3];

    private void Start()
    {
        cs = GameObject.Find("_SceneManager").GetComponent<CustomizeStorage>();

        // payment
        int packCost = (PlayerPrefs.GetInt("packs") * 50) + 200;
        PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") - packCost);
        PlayerPrefs.SetInt("packs", PlayerPrefs.GetInt("packs") + 1);

        for (int i = 0; i < 3; i++)
        {
            openOneCard(i);
        }
    }

    // return color, arrow or currency TO SET ACTIVE, sets img and text on its own 
    private void openOneCard(int cardIndex)
    {
        
        int colorIndex = 0;
        bool isArrow = false;

            // arrows
        int pickedArrowIndex = 1;
        bool isNew = true;
        // not new
        int actualRefundValue = 1;

            // currency
        bool isBonus = false;
        // not bonus
        bool isStar = false;
        int actualCurrencyValue = 1;

        if (Random.Range(0, 2) == 0) // arrow
        {
            isArrow = true;

            int randomInt = Random.Range(0, 101); // 0 - 100
            int common = 60;
            int rare = common + 35; // 95
            // epic  5

            if (randomInt <= common) // common arrow
            {
                colorIndex = 0;
                pickedArrowIndex = Random.Range(1, 24);
                actualRefundValue = 50;
            }
            else if (randomInt <= rare) // rare arrow
            {
                colorIndex = 1;
                pickedArrowIndex = Random.Range(24, 40);
                actualRefundValue = 200;
            }
            else  // epic arrow
            {
                colorIndex = 2;
                pickedArrowIndex = Random.Range(40, 48);
                actualRefundValue = 1000;
            }
            // apply bonus
            actualRefundValue = (int)(actualRefundValue * (1f + PlayerPrefs.GetInt("bonus") / 20f));

            // set isNew and save unlock or refund
            char[] owned = PlayerPrefs.GetString("owned").ToCharArray(); // loads owned
            if (owned[pickedArrowIndex].Equals('0'))
            {
                isNew = true;
                owned[pickedArrowIndex] = '1';
                PlayerPrefs.SetString("owned", new string(owned));
            }
            else
            {
                isNew = false;
                PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + actualRefundValue);
            }


        }

        else // currency
        {
            isArrow = false;

            int randomInt = Random.Range(0, 101); // 0 - 100
            int stars = 32;
            int hints = stars + 65; // 97
            // +5% bonus  3

            if (randomInt <= stars) // stars
            {
                isStar = true;
                randomInt = Random.Range(0, 101);
                int commonAmount = 70;
                int rareAmount = commonAmount + 25; // 95
                // epicAmount  5

                if (randomInt <= commonAmount)
                {
                    colorIndex = 0;
                    actualCurrencyValue = Random.Range(20, 50);
                }
                else if (randomInt <= rareAmount)
                {
                    colorIndex = 1;
                    actualCurrencyValue = Random.Range(200, 500);
                }
                else
                {
                    colorIndex = 2;
                    actualCurrencyValue = Random.Range(1500, 2500);
                }
                // apply bonus
                actualCurrencyValue = (int)(actualCurrencyValue * (1f + PlayerPrefs.GetInt("bonus") / 20f));

                PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + actualCurrencyValue);
            }
            else if (randomInt <= hints) // hints
            {
                isStar = false;
                randomInt = Random.Range(0, 101);
                int commonAmount = 60;
                int rareAmount = commonAmount + 35; // 95
                // epicAmount  5

                if (randomInt <= commonAmount)
                {
                    colorIndex = 0;
                    actualCurrencyValue = Random.Range(1, 4);
                }
                else if (randomInt <= rareAmount)
                {
                    colorIndex = 1;
                    actualCurrencyValue = Random.Range(10, 16);
                }
                else
                {
                    colorIndex = 2;
                    actualCurrencyValue = 30;
                }
                // apply bonus
                actualCurrencyValue = (int)( actualCurrencyValue * (1f + PlayerPrefs.GetInt("bonus") / 15f) );

                PlayerPrefs.SetInt("hints", PlayerPrefs.GetInt("hints") + actualCurrencyValue);
            }
            else    // bonus 5%
            {
                colorIndex = 2;
                isBonus = true; // false by default
                PlayerPrefs.SetInt("bonus", PlayerPrefs.GetInt("bonus") + 1);
            }
        }

        // now pass the data onto that card
        cardData cd = cardDatas[cardIndex];
        cd.arrowSkins = cs.arrowImage; // pass customizeStorage.arrowImages
        cd.arrowColors = cs.arrowColors;

        cd.colorIndex = colorIndex;
        cd.isArrow = isArrow;

            // arrows
        cd.pickedArrowIndex = pickedArrowIndex;
        cd.isNew = isNew;
        // not new
        cd.actualRefundValue = actualRefundValue;

            // currency
        cd.isBonus = isBonus;
        // not bonus
        cd.isStar = isStar;
        cd.actualCurrencyValue = actualCurrencyValue;

        cardObjects[cardIndex].SetActive(true);

        /*
        Debug.Log("colorIndex: " + colorIndex + "\nisArrow: " + isArrow + "\npickedArrowIndex: " + pickedArrowIndex + "\nisNew: "
            + isNew + "\nactualRefundValue: " + actualRefundValue + "\nisBonus: " + isBonus + "\nisStar: " + isStar +
            "\nactualCurrencyValue: " + actualCurrencyValue);
        */
    }


    private void Update()
    {
        if (countingValue <= 0)  // count done
        {
            
            if (maxCount < 300)
            {
                maxCount += 2;
                if (maxCount == 140) // final reveal
                {
                    foreach (cardData cd in cardDatas)
                    {
                        cd.reveal = true;
                    }
                    canExit = true;
                }
            }

            if (maxCount <= 30) // still under final maxCount
            {
                countingValue = maxCount;
                if (PlayerPrefs.GetString("sound").Equals("on"))
                { clicking.Play(); } // play sound
                for (int i = 0; i < 3; i++)
                {
                    cardData cd = cardDatas[i];
                    cd.scale = 3;
                    SpriteRenderer sr = cardObjects[i].GetComponent<SpriteRenderer>();

                    int randomInt = Random.Range(0, 6);
                    int fakeIndex = 0;
                    if (randomInt < 3) { fakeIndex = 0; }
                    else if (randomInt < 5) { fakeIndex = 1; }
                    else { fakeIndex = 2; }
                    sr.sprite = cardSkins[fakeIndex];
                }

                if (maxCount >= 30) // set real colors
                {
                    for (int i = 0; i < 3; i++)
                    {
                        cardData cd = cardDatas[i];
                        cd.scale = 3;
                        SpriteRenderer sr = cardObjects[i].GetComponent<SpriteRenderer>();
                        sr.sprite = cardSkins[cd.colorIndex];
                    }
                }
            }
            
        }
        else { countingValue--; }

        
    }

    public void exitToStore()
    {
        if (canExit)
        {
            GameObject.Find("_SceneManager").GetComponent<ChangeScene>().ChangeToScene("Store");
        }
    }
}
