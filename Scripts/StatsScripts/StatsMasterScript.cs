using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsMasterScript : MonoBehaviour
{
    ChangeScene ChangeScene;
    CustomizeStorage CustomizeStorage;
    AlertScript AlertScript;

    //bar
    public Image BarValue;
    // stats
    public TextMeshProUGUI bonusText;
    public TextMeshProUGUI packText;
    public TextMeshProUGUI easyText;
    public TextMeshProUGUI normalText;
    public TextMeshProUGUI hardText;

    public GameObject blocker;

    public Sprite upgradedImage;
    public Image PremiumAds;
    public GameObject BuyPremiumButton;

    public IAPManager IAPManager;

    void Start()
    {
        GameObject SceneManager = GameObject.Find("_SceneManager");
        ChangeScene = SceneManager.GetComponent<ChangeScene>();
        CustomizeStorage = SceneManager.GetComponent<CustomizeStorage>();
        AlertScript = SceneManager.GetComponent<AlertScript>();

        // set bar
        int expCap = PlayerPrefs.GetInt("level") * 85;
        float capBarValue = 1f / expCap * PlayerPrefs.GetInt("exp");
        BarValue.fillAmount = capBarValue;

        // set stats
        bonusText.text = "Bonus: " + PlayerPrefs.GetInt("bonus") * 5 + "%";
        packText.text = "Packs opened: " + PlayerPrefs.GetInt("packs");
        easyText.text = "Easy solved: " + PlayerPrefs.GetInt("easy");
        normalText.text = "Normal solved: " + PlayerPrefs.GetInt("normal");
        hardText.text = "Hard solved: " + PlayerPrefs.GetInt("hard");

        // if premium?
        if (PlayerPrefs.GetInt("highscore") == 763)
        {
            Destroy(blocker);
            Destroy(BuyPremiumButton);
            PremiumAds.sprite = upgradedImage;
        }
    }

    public void goToMenu()
    {
        ChangeScene.ChangeToScene("Menu");
    }

    public void changeBG(int index)
    {
        // if index above 1 then alert if not premimun
        if (PlayerPrefs.GetInt("highscore") != 763 && index > 1)
        {
            AlertScript.activateAlert(7);
        }
        else
        {
            PlayerPrefs.SetInt("bgSelect", index);
            ChangeScene.changeBackgroundTint(CustomizeStorage.backgroundColors[index]);
        }
    }

    public void buyPremium()
    {
        if (PlayerPrefs.GetInt("highscore") != 763)
        {
            IAPManager.BuyPremium();
        }
    }

    public void successfullyBoughtPremium()
    {
        PlayerPrefs.SetInt("highscore", 763);  // save premium on (removes ads and unlock all backgrounds)

        PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + 5000);
        PlayerPrefs.SetInt("hints", PlayerPrefs.GetInt("hints") + 300);
        PlayerPrefs.SetInt("bonus", PlayerPrefs.GetInt("bonus") + 4);

        ChangeScene.ChangeToScene("Stats"); // reload
        
    }
}
