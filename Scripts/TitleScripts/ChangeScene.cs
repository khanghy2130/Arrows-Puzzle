using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class ChangeScene : MonoBehaviour
{
    // ads count
    public int adsPopCount = 3;

    // GLOBAL
    public int tutorialIndex; // 0 is off
    public int bSize = 3;

    private GameObject Curtain;
    public bool darkening; // going to change scene?
    public bool isChanging;
    public bool tutorAllowed;
    public float curtainOpacity = 1;
    public string targetScene;
    public SpriteRenderer backgroundSprite;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 50;
    }

    private void Start() // starting of the title scene
    {
        Curtain = GameObject.Find("Curtain");
    }

    private void Update()
    {
        // update opacity value then set it onto curtain
        if (darkening)
        {
            // not fully darken yet?   else fully darkened 
            if (curtainOpacity < 1f)
            {
                curtainOpacity += 0.1f;
            }
            else
            {
                // fully darken =>> load target scene & set darkening to false
                darkening = false;
                SceneManager.LoadScene(targetScene);
                showAds();
            }
            
        }
        else if (curtainOpacity > 0f) // undarkening? and not done yet?
        {
            curtainOpacity -= 0.1f; 
            // set curtain to unactive
            if (curtainOpacity <= 0)
            {
                Curtain.SetActive(false);
                isChanging = false;
            }
        }
        curtainOpacity = Mathf.Clamp(curtainOpacity, 0f, 1f);

        // set opacity if alpha not 0
        if (curtainOpacity > 0)
        {
            Curtain.GetComponent<Renderer>().material.color = new Color(1, 1, 1, curtainOpacity);
        }

    }

    // play buttons
    public void ChangeToPlay(int boardSize)
    {
        if (!isChanging)
        {
            bSize = boardSize;
            tutorAllowed = false;
            ChangeToScene("Play");
        }
    }

    public void ChangeToScene(string sceneName)
    {
        if (sceneName.Equals("Tutorial"))
        {
            if (tutorAllowed) { setUpTrans(sceneName); }
        }
        else { setUpTrans(sceneName); }
    }

    private void setUpTrans(string sceneName)
    {
        if (!isChanging)
        {
            isChanging = true;
            darkening = true;
            targetScene = sceneName;
            Curtain.SetActive(true);
        }
    }

    public void changeBackgroundTint(Color c)
    {
        backgroundSprite.color = c;
    }

    private void showAds()
    {
        if (PlayerPrefs.GetInt("highscore") != 763 && adsPopCount <= 0)
        {
            adsPopCount = 3;
            if (Advertisement.IsReady())
            {
                Advertisement.Show("video");
            }
        }
        
    }

    public void showRewardedAds()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleAdResult });
        }
    }

    private void HandleAdResult(ShowResult result)
    {
        switch(result)
        {
            case ShowResult.Finished:
                int RewardValueHints = GameObject.Find("CanvasMaster").GetComponent<GiftMasterScript>().RewardValueHints;
                PlayerPrefs.SetInt("hints", PlayerPrefs.GetInt("hints") + RewardValueHints);
                ChangeToScene("Gift");
                break;
            case ShowResult.Skipped:
                Debug.Log("skip");
                break;
            case ShowResult.Failed:
                Debug.Log("fail");
                break;
        }
    }

}
