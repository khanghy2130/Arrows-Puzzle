using UnityEngine;
using System;
using TMPro;

public class GiftMasterScript : MonoBehaviour
{
    public bool PackReady;
    public bool IsSpecial;

    private ulong lastClaimTime;
    private ulong lastWatchTime;
    private ulong currentTime;

    public int RewardValueStars; // real amount of rewards
    public int RewardValueHints = 15; // real amount of rewards

    // get the stuffs
    public TextMeshPro StarsMessage;
    public TextMeshPro StarsButtonText;
    public TextMeshPro RewardStarsText;

    public GameObject Specialx3Text;
    public TextMeshPro RewardHintsText;

    ChangeScene ChangeScene;

    void Start()
    {
        ChangeScene = GameObject.Find("_SceneManager").GetComponent<ChangeScene>();

        // default?
        if (PlayerPrefs.GetString("lastClaimTime").Equals("none"))
        {
            setPackReady();
        }
        else
        {
            lastClaimTime = ulong.Parse( PlayerPrefs.GetString("lastClaimTime") );
        }

        if (PlayerPrefs.GetString("lastWatchTime").Equals("none"))
        {
            setSpecialReady();
        }
        else
        {
            lastWatchTime = ulong.Parse(PlayerPrefs.GetString("lastWatchTime"));
        }

        // set pack value
        RewardValueStars = 160 + PlayerPrefs.GetInt("packs") * 10 + PlayerPrefs.GetInt("level") * 40 + PlayerPrefs.GetInt("bonus") * 200;
        RewardStarsText.text = RewardValueStars + "";

    }

    
    void Update()
    {
        currentTime = (ulong) DateTime.Now.Ticks / 10000000; // in second

        // check for pack-ready
        if (!PackReady)
        {
            if (currentTime - lastClaimTime > 3600) // has been 1h?
            {
                setPackReady();
            }
            else
            {
                ulong secLeft = 3600 - (currentTime - lastClaimTime);
                string minShow = (secLeft / 60).ToString();
                string secShow = (secLeft % 60).ToString();
                StarsButtonText.text = minShow + "m " + secShow + "s";
            }
        }

        // check for special-ready
        if (!IsSpecial)
        {
            if (currentTime - lastWatchTime > 3600) // has been 1h?
            {
                setSpecialReady();
            }
        }
        
    }


    private void setPackReady()
    {
        PackReady = true;
        StarsButtonText.text = "claim";
        StarsMessage.text = "Ready to claim!";
    }

    private void setSpecialReady()
    {
        IsSpecial = true;
        Specialx3Text.SetActive(true);
        RewardValueHints = 45;
        RewardHintsText.text = RewardValueHints + "";
    }

    public void claimStars()
    {
        if (PackReady && !ChangeScene.isChanging)
        {
            PackReady = false;
            PlayerPrefs.SetString("lastClaimTime", currentTime.ToString()); // save time
            
            PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + RewardValueStars);
            ChangeScene.ChangeToScene("Gift");
        }
    }

    public void watchAds()
    {
        if (!ChangeScene.isChanging)
        {
            if (IsSpecial)
            {
                IsSpecial = false;
                PlayerPrefs.SetString("lastWatchTime", currentTime.ToString()); // save time
            }

            ChangeScene.showRewardedAds();
        }
    }

    public void goToMenu()
    {
        ChangeScene.ChangeToScene("Menu");
    }
}
