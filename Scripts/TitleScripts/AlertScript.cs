using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertScript : MonoBehaviour
{
    public int messageIndex;

    // get objects
    public ChangeScene ChangeScene;
    public GameObject AlertCanvas;
    public GameObject MessageText;
    public GameObject cancelButton;
    public GameObject okayButton;



    public void activateAlert(int index)
    {
        messageIndex = index;
        string message = "";

        if (index == 1)
        {
            message = "Welcome! Would you like to start the tutorial?";
        }
        else if (index == 2)
        {
            message = "Hope you enjoy the game so far. Would you like to leave a feedback?";
        }
        else if (index == 3)
        {
            message = "Are you sure you want to quit?";
        }
        else if (index == 4) // hint
        {
            int hintCost = 1;
            if (ChangeScene.bSize == 3) { hintCost = 1; }
            else if (ChangeScene.bSize == 4) { hintCost = 2; }
            else if (ChangeScene.bSize == 5) { hintCost = 3; }
            message = "Show a hint?\nCost: " + hintCost + " hint points" ;
        }
        else if (index == 5) // unlock arrow with hints (NOT USED)
        {
            message = "Unlock this item?";
        }
        else if (index == 6) // qMark locked
        {
            message = "Unlock all arrows above to unlock this superior item.";
            cancelButton.SetActive(false);
        }
        else if (index == 7) // bg locked
        {
            message = "Upgrade to Premium to unlock this background color.";
            cancelButton.SetActive(false);
        }

        MessageText.GetComponent<TextMeshProUGUI>().text = message;
        AlertCanvas.SetActive(true);
    }

    private int arrowCost;
    public void activateAlertToBuyItem(int cost) {
        arrowCost = cost;
        messageIndex = 5;
        string message = "Unlock this arrow?\nCost: " + cost + " hints";

        MessageText.GetComponent<TextMeshProUGUI>().text = message;
        AlertCanvas.SetActive(true);
    }
    
    public void okay()
    {
        bool doNothing = false;

        if (messageIndex == 1) // ask to open tutorial
        {
            ChangeScene.tutorialIndex = 1;
            ChangeScene.ChangeToScene("Tutorial");
        }
        else if (messageIndex == 2) // ask to rate
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.LogixIndie.arrowspuzzle");
        }
        else if (messageIndex == 3) // quit to menu
        {
            ChangeScene.ChangeToScene("Menu");
        }
        else if (messageIndex == 4) // hint
        {
            // not enough hint points then DO NOTHING
            int hintHave = PlayerPrefs.GetInt("hints");
            int hintCost = 1;
            if (ChangeScene.bSize == 3) { hintCost = 1; }
            else if (ChangeScene.bSize == 4) { hintCost = 2; }
            else if (ChangeScene.bSize == 5) { hintCost = 3; }
            if (hintHave >= hintCost)
            {
                PlayerPrefs.SetInt("hints", hintHave - hintCost);
                GameObject.Find("CurrencyHint").GetComponent<currencyScript>().LoadValue(false);
                GameObject.Find("_LevelManager").GetComponent<PlayInputControl>().confirmedHintActivation();
            }
            else { doNothing = true; }
        }
        else if (messageIndex == 5) // unlock item with hint points
        {
            // also takes payment!
            int hintHave = PlayerPrefs.GetInt("hints");
            if (hintHave >= arrowCost)
            {
                PlayerPrefs.SetInt("hints", hintHave - arrowCost);
                GameObject.Find("Camera").GetComponent<StoreMasterScript>().unlockArrowAndReload();
            }
            else { doNothing = true; }
        }

        if (!doNothing) { cancel(); } // action proceeded
    }

    public void cancel()
    {
        AlertCanvas.SetActive(false);
        cancelButton.SetActive(true);
        messageIndex = 0;
    }
}
