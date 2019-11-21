using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeStorage : MonoBehaviour   // In charge of LOADING ALL DATA...
{
    public ChangeScene ChangeScene;

    // dictionary of BG colors and ARROW SKINS
    public Color[] backgroundColors = new Color[6];
    public Color[] arrowColors = new Color[49];
    public Sprite[] arrowImage = new Sprite[49];

    // global variables for BG and SKIN
    public int selectedBackground;
    public int selectedArrow;

    void Awake()
    {
        //PlayerPrefs.DeleteAll(); // reset save

        // load BG
        if (!PlayerPrefs.HasKey("bgSelect")) { PlayerPrefs.SetInt("bgSelect", 0); } // default value
        selectedBackground = PlayerPrefs.GetInt("bgSelect");
        ChangeScene.changeBackgroundTint(backgroundColors[selectedBackground]);

        // load arrow skin
        if (!PlayerPrefs.HasKey("arrowSelect")) { PlayerPrefs.SetInt("arrowSelect", 0); }
        selectedArrow = PlayerPrefs.GetInt("arrowSelect");

        // load owned (default is "100000000000000000000000000000000000000000000000") 111111111111111111111111111111111111111111111111
        if (!PlayerPrefs.HasKey("owned")) { PlayerPrefs.SetString("owned", "100000000000000000000000000000000000000000000000"); }

        // load stars
        if (!PlayerPrefs.HasKey("stars")) { PlayerPrefs.SetInt("stars", 0); }
        // load hints
        if (!PlayerPrefs.HasKey("hints")) { PlayerPrefs.SetInt("hints", 10); }
        // load exp
        if (!PlayerPrefs.HasKey("exp")) { PlayerPrefs.SetInt("exp", 0); }
        // load bonus
        if (!PlayerPrefs.HasKey("bonus")) { PlayerPrefs.SetInt("bonus", 0); }
        // load level
        if (!PlayerPrefs.HasKey("level")) { PlayerPrefs.SetInt("level", 1); }
        // load packs opened
        if (!PlayerPrefs.HasKey("packs")) { PlayerPrefs.SetInt("packs", 0); }

        // load easy solves
        if (!PlayerPrefs.HasKey("easy")) { PlayerPrefs.SetInt("easy", 0); }
        // load normal solves
        if (!PlayerPrefs.HasKey("normal")) { PlayerPrefs.SetInt("normal", 0); }
        // load hard solves
        if (!PlayerPrefs.HasKey("hard")) { PlayerPrefs.SetInt("hard", 0); }

        // load sound
        if (!PlayerPrefs.HasKey("sound")) { PlayerPrefs.SetString("sound", "on"); }
        // load tutorial
        if (!PlayerPrefs.HasKey("welcome")) { PlayerPrefs.SetString("welcome", "on"); }
        // load rate
        if (!PlayerPrefs.HasKey("rate")) { PlayerPrefs.SetString("rate", "on"); }

        // load free stars
        if (!PlayerPrefs.HasKey("lastClaimTime")) { PlayerPrefs.SetString("lastClaimTime", "none"); }

        // load special watch
        if (!PlayerPrefs.HasKey("lastWatchTime")) { PlayerPrefs.SetString("lastWatchTime", "none"); }

        // load highscore (premimum) 763   Random.Range(250, 731)
        if (!PlayerPrefs.HasKey("highscore")) { PlayerPrefs.SetInt("highscore", Random.Range(250, 731)); }

    }
    

}
