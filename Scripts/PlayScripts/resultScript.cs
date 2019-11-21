using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class resultScript : MonoBehaviour  // add and save stars!
{
    public GameObject LevelManager;
    public Transform transfrom;
    public GameObject RewardHolder;
    public GameObject LevelHolder;
    private PlayInputControl pic;

    public GameObject Currency1;
    public GameObject Currency2;
    public GameObject Currency3;

    public GameObject multiplier;
    public Image BarValue;
    private float barValueShow = 0f;

    private int timer = 0;
    private RectTransform LevelHolderTransform;
    private RectTransform RewardHolderTransform;
    // SET UP VALUES
    void Start()
    {
        pic = LevelManager.GetComponent<PlayInputControl>();
        multiplier.GetComponent<TextMeshPro>().text = "+" + (PlayerPrefs.GetInt("bonus") * 5) + "%";

        Currency1.GetComponent<currencyScript>().setValueToText(pic.solvedStars);
        Currency2.GetComponent<currencyScript>().setValueToText(pic.bonusStars);
        Currency3.GetComponent<currencyScript>().setValueToText(pic.bonusStars2);

        LevelHolderTransform = LevelHolder.GetComponent<RectTransform>();
        RewardHolderTransform = RewardHolder.GetComponent<RectTransform>();
    }

    // REVEAL TIMELINE
    void Update()
    {
        timer++;
        if (timer > 0 && !Currency1.activeSelf) { Currency1.SetActive(true); }
        else if (timer > 30 && !Currency2.activeSelf) { Currency2.SetActive(true); }
        else if (timer > 60 && !Currency3.activeSelf) { Currency3.SetActive(true); }

        // EXP and LV
        if (timer > 80)
        {
            if (barValueShow <= 1f) // run this if not over EXP
            {
                // get the cap value of barValueShow
                float capBarValue = 1f / pic.expCap * pic.finalExp;  // pic.finalExp
                
                if (barValueShow <= capBarValue)
                {
                    barValueShow += 0.015f;
                    BarValue.fillAmount = Mathf.Min(barValueShow, 1f);
                }
            }
            else if (pic.leveledUp) // LEVEL UP ANIMATION when EXP is enough    pic.leveledUp
            {

                if (LevelHolderTransform.position.x < -20)
                {
                    // move self up
                    transform.position += new Vector3(0, 0.6f, 0);

                    // move level holder to the right
                    LevelHolderTransform.position += new Vector3(1.7f, 0, 0);

                    // move reward holder to the left
                    RewardHolderTransform.position -= new Vector3(1.7f, 0, 0);
                }
                
            }
        }
    }
}
