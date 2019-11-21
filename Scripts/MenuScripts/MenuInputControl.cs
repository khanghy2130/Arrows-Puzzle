using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuInputControl : MonoBehaviour
{
    ChangeScene ChangeScene;
    public GameObject removelv4;
    public GameObject removelv8;

    public GameObject PackReadyMark;
    public GameObject GiftReadyMark;

    public Image soundButton;
    public Sprite soundOnImage;
    public Sprite soundOffImage;

    private void Start()
    {
        ChangeScene = GameObject.Find("_SceneManager").GetComponent<ChangeScene>();
        ChangeScene.tutorAllowed = true;

        // set sound image
        if (PlayerPrefs.GetString("sound").Equals("off"))
        {
            soundButton.sprite = soundOffImage;
        }

        // set active mark for pack ready
        if (PlayerPrefs.GetInt("stars") >= (PlayerPrefs.GetInt("packs") * 50) + 200)
        {
            PackReadyMark.SetActive(true);
        }

        // set active mark for free reward ready
        if (PlayerPrefs.GetString("lastClaimTime").Equals("none")) // default?
        {
            GiftReadyMark.SetActive(true);
        }
        else
        {
            ulong lastClaimTime = ulong.Parse(PlayerPrefs.GetString("lastClaimTime"));
            ulong currentTime = (ulong)DateTime.Now.Ticks / 10000000;
            if (currentTime - lastClaimTime > 3600)
            {
                GiftReadyMark.SetActive(true);
            }
        }

        // remove play locks
        if (PlayerPrefs.GetInt("level") >= 4) { Destroy(removelv4); }
        if (PlayerPrefs.GetInt("level") >= 8) { Destroy(removelv8); }

        // if new player? ask to open tutorial
        if (PlayerPrefs.GetString("welcome").Equals("on"))
        {
            PlayerPrefs.SetString("welcome", "off");
            GameObject.Find("_SceneManager").GetComponent<AlertScript>().activateAlert(1);
        }

        // if rate on  AND  over lv5
        if (PlayerPrefs.GetString("rate").Equals("on") && PlayerPrefs.GetInt("level") >= 5)
        {
            PlayerPrefs.SetString("rate", "off");
            rate();
        }
    }

    public void ChangeToPlay(int boardSize)
    {
        if (boardSize == 4)
        {
            if (PlayerPrefs.GetInt("level") >= 4)
            {
                ChangeScene.ChangeToPlay(boardSize);
            }
        }
        else if (boardSize == 5)
        {
            if (PlayerPrefs.GetInt("level") >= 8)
            {
                ChangeScene.ChangeToPlay(boardSize);
            }
        }
        else { ChangeScene.ChangeToPlay(boardSize); }
    }

    public void GoToTutorial()
    {
        if (ChangeScene.tutorAllowed)
        {
            ChangeScene.tutorialIndex = 1;
            ChangeScene.ChangeToScene("Tutorial");
        }
    }

    public void GoToShop() { ChangeScene.ChangeToScene("Store"); }

    public void GoToStats() { ChangeScene.ChangeToScene("Stats"); }

    public void GoToGift() { ChangeScene.ChangeToScene("Gift"); }

    public void soundToggle()
    {
        if (PlayerPrefs.GetString("sound") == "on")
        {
            PlayerPrefs.SetString("sound", "off");
            soundButton.sprite = soundOffImage;
        }
        else
        {
            PlayerPrefs.SetString("sound", "on");
            soundButton.sprite = soundOnImage;
        }
    }

    public void rate() { GameObject.Find("_SceneManager").GetComponent<AlertScript>().activateAlert(2); }

    public void goToPolicy() { Application.OpenURL("https://khanghy2130.github.io/arrows_puzzle_privacy_policy/"); }

    //public void removeAllPrefs() { PlayerPrefs.DeleteAll(); Debug.Log("All keys deleted."); }
    }
