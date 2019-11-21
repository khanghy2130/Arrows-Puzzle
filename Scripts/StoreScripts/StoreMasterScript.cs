using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreMasterScript : MonoBehaviour
{
    public AudioSource selectSound;
    ChangeScene ChangeScene;
    CustomizeStorage CustomizeStorage;
    AlertScript AlertScript;

    public GameObject SkinButton;
    public GameObject SkinButtonsParent;
    public GameObject ImageSelect;

    public TextMeshPro packCostText;

    public char[] owned;

    public int packCost;

    void Start()
    {
        // get ChangeScene and CustomizeStorage
        GameObject SceneManager = GameObject.Find("_SceneManager");
        ChangeScene = SceneManager.GetComponent<ChangeScene>();
        CustomizeStorage = SceneManager.GetComponent<CustomizeStorage>();
        AlertScript = SceneManager.GetComponent<AlertScript>();

        // set up owned
        owned = PlayerPrefs.GetString("owned").ToCharArray();

        // set up buy button
        packCost = (PlayerPrefs.GetInt("packs") * 50) + 200;
        packCostText.text = packCost + "";

        // create arrow buttons
        GameObject clone;
        Image ImageComponent;
        SkinButtonScript sbs;
        for (int i=0; i < CustomizeStorage.arrowImage.Length; i++)
        {
            clone = Instantiate(SkinButton);
            ImageComponent = clone.GetComponent<Image>();
            sbs = clone.GetComponent<SkinButtonScript>();
            ImageComponent.sprite = CustomizeStorage.arrowImage[i];  // change sprite image
            clone.transform.SetParent(SkinButtonsParent.transform);  // set parent

            sbs.sms = this;
            sbs.idNumber = i; // give ID


            // change color
            if (i < 48 && owned[i].Equals('1')) // owned?
            { ImageComponent.color = CustomizeStorage.arrowColors[i];  }
            else if (i == 48 && allOwned()) // qMark check
            {
                ImageComponent.color = CustomizeStorage.arrowColors[i];
            }
            else { ImageComponent.color = new Color(0,0,0); }
        }

        // set image select shower
        Image img = ImageSelect.GetComponent<Image>();
        img.sprite = CustomizeStorage.arrowImage[CustomizeStorage.selectedArrow];
        img.color = CustomizeStorage.arrowColors[CustomizeStorage.selectedArrow];
        
    }

    private void Update() // just rotating the select Image
    {
        Vector3 rot = ImageSelect.transform.rotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y, rot.z - 2f);
        ImageSelect.transform.rotation = Quaternion.Euler(rot);
    }

    private bool allOwned()
    {
        for (int q = 0; q < owned.Length; q++)
        {
            if (!owned[q].Equals('1')) { return false; }
        }
        return true;
    }

    private int buyingId;
    public void skinClickedHandler(int idNumber)
    {
        if (idNumber == 48 && !allOwned()) // is qMark and not all owned
        {
            AlertScript.activateAlert(6);
        }
        else if (idNumber < 48 && owned[idNumber].Equals('0')) // is other than qMark and not owned
        {
            if (!ChangeScene.isChanging)
            {
                buyingId = idNumber;
                int cost = 300;
                if (idNumber < 24) { cost = 50; } // common cost
                else if (idNumber < 40) { cost = 100; } // rare cost
                AlertScript.activateAlertToBuyItem(cost);
            }
        }
        else   // owned
        {
            if (PlayerPrefs.GetString("sound").Equals("on"))
            { selectSound.Play(); }
            // set and save
            CustomizeStorage.selectedArrow = idNumber;
            PlayerPrefs.SetInt("arrowSelect", idNumber);
            // set image
            Image img = ImageSelect.GetComponent<Image>();
            img.sprite = CustomizeStorage.arrowImage[idNumber];
            img.color = CustomizeStorage.arrowColors[idNumber];
        }
    }

    public void unlockArrowAndReload()
    {
        owned[buyingId] = '1';
        string newOwned = new string(owned);
        PlayerPrefs.SetString("owned", newOwned);
        ChangeScene.ChangeToScene("Store");
    }

    public void buyPack()
    {
        if (PlayerPrefs.GetInt("stars") >= packCost)
        {
            ChangeScene.ChangeToScene("Open");
        }
    }

    public void goToMenu()
    {
        ChangeScene.ChangeToScene("Menu");
    }
}
