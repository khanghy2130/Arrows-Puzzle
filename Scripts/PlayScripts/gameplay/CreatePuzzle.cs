using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CreatePuzzle : MonoBehaviour
{
    public AudioSource correctSound;
    public AudioSource incorrectSound;

    // for setting up arrow items
    public Sprite image; // getting selected image
    private SpriteRenderer spriteHolder; // sr component
    private ArrowData dataHolder; // arrow data component
    public GameObject arrowButton; // getting original object
    GameObject clone;
    ChangeScene ChangeScene;

    // store
    public int bSize;
    public float cellSize;
    public static int[] degrees = new int[8]; // no change

    public int startAnimateValue;
    public int[,] solution2d; // holds the solutionNumber for all arrows
    public int[,] degree2d; // holds the degree values for all arrows

    // for gameplay
    public int playVisited;
    ArrowData lastPressed;
    UpdateTint lastPressedTintObj = null;
    private int blinkTimer;
    public bool won;

    // for generator
    public Transform ArrowsBoard;
    private bool[,] visitedCells; // visited arrows
    int visitedAmount; // to check if visited all

    // -----------------------------------------------
    // For gameplay >>>>>>

    public void resetAll()
    {
        playVisited = 0;
        lastPressed = null;
        lastPressedTintObj = null;

        var arrows = GameObject.FindGameObjectsWithTag("ArrowItem");
        // loops through all arrowItems
        foreach (var arrowItem in arrows)
        {
            ArrowData dataHolder = arrowItem.GetComponent<ArrowData>();
            if (dataHolder.activated)
            {
                arrowItem.GetComponent<UpdateTint>().setShrink(0.6f);
                dataHolder.activated = false;
            }
        }
    }

    public void newPlayMove(ArrowData obj, UpdateTint tintObj)
    {
        bool correct = false; // qualified to be activated
        // not the first arrow to be pressed? => futher check
        if (playVisited > 0)
        {
            // not within the direction of last object => return
            bool q = Mathf.Abs(lastPressed.cx - obj.cx) == Mathf.Abs(lastPressed.cy - obj.cy);
            int lastArrowDeg = degree2d[lastPressed.cy, lastPressed.cx];
            // up
            if (lastArrowDeg == 90)
            {
                correct = obj.cx == lastPressed.cx && obj.cy < lastPressed.cy;
            }
            // down
            else if (lastArrowDeg == 270)
            {
                correct = obj.cx == lastPressed.cx && obj.cy > lastPressed.cy;
            }
            // left
            else if (lastArrowDeg == 180)
            {
                correct = obj.cy == lastPressed.cy && obj.cx < lastPressed.cx;
            }
            // right
            else if (lastArrowDeg == 0)
            {
                correct = obj.cy == lastPressed.cy && obj.cx > lastPressed.cx;
            }
            // up right
            else if (lastArrowDeg == 45)
            {
                correct = q && obj.cy < lastPressed.cy && obj.cx > lastPressed.cx;
            }
            // down right
            else if (lastArrowDeg == 315)
            {
                correct = q && obj.cy > lastPressed.cy && obj.cx > lastPressed.cx;
            }
            // down left
            else if (lastArrowDeg == 225)
            {
                correct = q && obj.cy > lastPressed.cy && obj.cx < lastPressed.cx;
            }
            // up left
            else if (lastArrowDeg == 135)
            {
                correct = q && obj.cy < lastPressed.cy && obj.cx < lastPressed.cx;
            }

        }
        else { correct = true; }
        if (!correct)
        {
            if (PlayerPrefs.GetString("sound").Equals("on"))
            { incorrectSound.Play(); }
            resetAll();
        }
        else  // CORRECT!
        {
            if (PlayerPrefs.GetString("sound").Equals("on"))
            { correctSound.Play(); }
            obj.activated = true;
            playVisited++;
            lastPressed = obj;
            lastPressedTintObj = tintObj;
            blinkTimer = 150;   //// set timer for blinking

            // check win (IF TUTORIAL ELSE NORMAL PLAY)
            if (ChangeScene.tutorialIndex != 0 && playVisited == 6)
            {
                won = true;
            }
            else if (playVisited == bSize*bSize)
            {
                won = true;
                ChangeScene.adsPopCount -= 1;
                
                // add to solves
                if (bSize == 3) { PlayerPrefs.SetInt("easy" ,PlayerPrefs.GetInt("easy") + 1); }
                else if (bSize == 4) { PlayerPrefs.SetInt("normal", PlayerPrefs.GetInt("normal") + 1); }
                else if (bSize == 5) { PlayerPrefs.SetInt("hard", PlayerPrefs.GetInt("hard") + 1); }
            }
        }
    }

    // For generator >>>>>>

    private void generateOne(int[] currentPos) // [y,x]
    {
        // NEW ATTEMPT TO GET NEW MOVE
        List<int> visitedDirsID = new List<int>(); // list of indices
        int x, y, dirID = 0;
        List<int[]> possMove = new List<int[]>(); // [ [y,x] , [y,x] ]
        
        while (visitedDirsID.Count < 8)
        {
            // NEW ATTEMPT TO GET NEW DIRECTION
            // loop to get new dir
            do
            {
                dirID = Random.Range(0, 8);
            }
            while (visitedDirsID.Contains(dirID)); // already visited?
            x = currentPos[1]; y = currentPos[0];

            if (dirID == 2)
            { // UP c
                while (y > 0)
                {
                    y--;
                    if (!visitedCells[y,x])
                    {
                        int[] item = new int[2];
                        item[0] = y; item[1] = x;
                        possMove.Add(item);
                    }
                }

            }
            else if (dirID == 0)
            { // RIGHT c
                while (x < bSize - 1)
                {
                    x++;
                    if (!visitedCells[y,x])
                    {
                        int[] item = new int[2];
                        item[0] = y; item[1] = x;
                        possMove.Add(item);
                    }
                }
            }
            else if (dirID == 6)
            { // DOWN c
                while (y < bSize - 1)
                {
                    y++;
                    if (!visitedCells[y,x])
                    {
                        int[] item = new int[2];
                        item[0] = y; item[1] = x;
                        possMove.Add(item);
                    }
                }
            }
            else if (dirID == 4)
            { // LEFT c
                while (x > 0)
                {
                    x--;
                    if (!visitedCells[y,x])
                    {
                        int[] item = new int[2];
                        item[0] = y; item[1] = x;
                        possMove.Add(item);
                    }
                }
            }
            else if (dirID == 1)
            { // UP RIGHT c 
                while (y > 0 && x < bSize - 1)
                {
                    y--; x++;
                    if (!visitedCells[y,x])
                    {
                        int[] item = new int[2];
                        item[0] = y; item[1] = x;
                        possMove.Add(item);
                    }
                }
            }
            else if (dirID == 7)
            { // DOWN RIGHT c
                while (y < bSize - 1 && x < bSize - 1)
                {
                    y++; x++;
                    if (!visitedCells[y,x])
                    {
                        int[] item = new int[2];
                        item[0] = y; item[1] = x;
                        possMove.Add(item);
                    }
                }
            }
            else if (dirID == 5)
            { // DOWN LEFT c 
                while (y < bSize - 1 && x > 0)
                {
                    y++; x--;
                    if (!visitedCells[y,x])
                    {
                        int[] item = new int[2];
                        item[0] = y; item[1] = x;
                        possMove.Add(item);
                    }
                }
            }
            else if (dirID == 3)
            { // UP LEFT c 
                while (y > 0 && x > 0)
                {
                    y--; x--;
                    if (!visitedCells[y,x])
                    {
                        int[] item = new int[2];
                        item[0] = y; item[1] = x;
                        possMove.Add(item);
                    }
                }
            }

            // DONE finding on that direction
            if (possMove.Count > 0) { break; } // possMove has something!
            else { visitedDirsID.Add(dirID); } // adds to visited directions
        }

        // possMove has somthing
        if (possMove.Count > 0)
        {
            solution2d[currentPos[0],currentPos[1]] = visitedAmount;
            degree2d[currentPos[0], currentPos[1]] = degrees[dirID]; // set dir for current one
            visitedCells[currentPos[0], currentPos[1]] = true;
            visitedAmount++;
            
            // pick one from possMove for the next move
            int[] picked = possMove[Random.Range(0, possMove.Count)];

            generateOne(picked);
        }
        // possMove has nothing and  >> visited all? then set last arrow
        else if (visitedAmount == bSize * bSize)
        {
            // set inward direction for last arrow
            int bs = bSize;
            while (true)
            {
                dirID = Random.Range(0, 8);
                if (dirID == 2 && currentPos[0] > 0) { break; }
                else if (dirID == 1 && currentPos[0] > 0 && currentPos[1] < bs - 1) { break; }
                else if (dirID == 0 && currentPos[1] < bs - 1) { break; }
                else if (dirID == 7 && currentPos[0] < bs - 1 && currentPos[1] < bs - 1) { break; }
                else if (dirID == 6 && currentPos[0] < bs - 1) { break; }
                else if (dirID == 5 && currentPos[0] < bs - 1 && currentPos[1] > 0) { break; }
                else if (dirID == 4 && currentPos[1] > 0) { break; }
                else if (dirID == 3 && currentPos[0] > 0 && currentPos[1] > 0) { break; }
            }
            solution2d[currentPos[0],currentPos[1]] = visitedAmount;
            degree2d[currentPos[0],currentPos[1]] = degrees[dirID];
            visitedCells[currentPos[0], currentPos[1]] = true;
        }
    }

    // generate a puzzle (give arrows direction)
    private void setUpPlay()
    {
        // (re)set for a new puzzle
        solution2d = new int[bSize,bSize];
        degree2d = new int[bSize, bSize]; 
        visitedAmount = 1;
        visitedCells = new bool[bSize, bSize];

        // set up first arrow 
        int[] first = new int[2];
        first[0] = Random.Range(0, bSize); first[1] = Random.Range(0, bSize); 

        generateOne(first); // starts the generator with first
        if (visitedAmount != bSize * bSize) { setUpPlay(); } // reset if not visited all
    }
    
    // GENERATE puzzle and set up arrows
    void Start()
    {
        CustomizeStorage CustomizeStorage = GameObject.Find("_SceneManager").GetComponent<CustomizeStorage>();

        ChangeScene = GameObject.Find("_SceneManager").GetComponent<ChangeScene>();
        string thisSceneName = SceneManager.GetActiveScene().name;
        if (!thisSceneName.Equals("Tutorial")) { ChangeScene.tutorialIndex = 0; }
        
        bSize = ChangeScene.bSize;  // get bSize
        for (int i = 0; i < 8; i++) {degrees[i] = i * 45; }     // set up degree values

        // if tutorial ?
        int tutorialIndex = ChangeScene.tutorialIndex;
        if (tutorialIndex != 0)
        {
            bSize = 3;
            solution2d = new int[bSize, bSize];
            string tutorMessage = "";
            if (tutorialIndex == 1) {
                degree2d = new int[3, 3] { { -1, 180, 180 }, { -1, -1, 90 }, { 0, 0, 90 } };
                tutorMessage = "Your goal is to activate all arrows, tapping on one will activate it.";
            }
            else if (tutorialIndex == 2)
            {
                degree2d = new int[3, 3] { { 0, 270, 180 }, { -1, 270, -1 }, { 0, -1, 90 } };
                tutorMessage = "It's like jumping from arrow to arrow. You'd be on the last arrow you activated, and you can only jump to the ones which are in your direction, no matter how far.";
            }
            else if (tutorialIndex == 3)
            {
                degree2d = new int[3, 3] { { 315, -1 , 270 }, { -1, 45, 270 }, { 45, -1, 135 } };
                tutorMessage = "They can go in diagonal directions as well! If you activate the invalid arrow then everything will reset.";
            }
            else if (tutorialIndex == 4)
            {
                degree2d = new int[3, 3] { { 315, -1, 270 }, { -1, 45, -1 }, { 0, 180, 135 } };
                tutorMessage = "It never hurts to just sit back and think. Hint: Start with the arrow at the center bottom!";
            }
            else if (tutorialIndex == 5)
            {
                degree2d = new int[3, 3] { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 } };
                tutorMessage = "You have completed the tutorial! Here are some tips:\n\n" +
                    "The last activated arrow would blink.\n\n" +
                    "The puzzles are all random, but you can develop your own strategy to solve any puzzle.\n\n" +
                    "The higher the difficulty, the higher the reward. Want to level up fast? Avoid using hints!\n\n" +
                    "You can directly unlock items in shop with hint points!\n\n" +
                    "For every 5 levels up your bonus multiplier would increase, it can also be found by opening packs.";
            }
            GameObject.Find("solutiontext").GetComponent<TextMeshProUGUI>().text = tutorMessage;
        }
        else {
            setUpPlay();

            /*
            // print solution (keep this) //////////////////////////////
            string solu = "";
            for (int i = 0; i < bSize; i++)
            {
                string line = "";
                for (int j = 0; j < bSize; j++)
                {
                    int l = solution2d[i, j];
                    line += l + ((l < 10) ? "   " : "  ");
                }
                solu += line + "\n";
            }

            GameObject.Find("solutiontext").GetComponent<TextMeshProUGUI>().text = solu;*/
        }

        Color c;
        // get the selected arrow skin and color
        if (thisSceneName.Equals("Tutorial"))
        {
            image = CustomizeStorage.arrowImage[0];
            c = CustomizeStorage.arrowColors[0];
        }
        else
        {
            image = CustomizeStorage.arrowImage[CustomizeStorage.selectedArrow];
            c = CustomizeStorage.arrowColors[CustomizeStorage.selectedArrow];
            
        }

        // cell size
        cellSize = 22f / bSize; // default
        float s = 68 / (bSize - 0.2f); // for positioning

        // set up arrows
        for (int y = bSize-1; y > -1; y--)
        {
            for (int x = 0; x < bSize; x++)
            {
                float deg = degree2d[y,x] + 0.0f;

                // y is SAV*speed +25   (speed is at ABMovement, update yPos)
                startAnimateValue = 20;
                // instantiate => position and rotation
                clone = Instantiate(arrowButton,
                new Vector3(s * (x - (bSize - 1) * 0.5f), -s * (y - (bSize - 1) * 0.5f) + startAnimateValue*2 +25, 0),
                Quaternion.Euler(0, 0, deg));

                // set parent
                clone.transform.SetParent(ArrowsBoard);

                // re-scale
                clone.transform.localScale = new Vector3(cellSize, cellSize, 0);

                // set image 
                spriteHolder = clone.GetComponent<SpriteRenderer>();
                if (CustomizeStorage.selectedArrow == 48 && !thisSceneName.Equals("Tutorial"))  // (if random ? set new img and color)
                {
                    int randomSelect = Random.Range(0, 48);
                    image = CustomizeStorage.arrowImage[randomSelect];
                    c = CustomizeStorage.arrowColors[randomSelect];
                }
                spriteHolder.sprite = image;
                spriteHolder.color = c;

                // set data
                dataHolder = clone.GetComponent<ArrowData>();
                dataHolder.changePos(x, y);

                // TUTORIAL TWIST!
                if (tutorialIndex != 0 && deg == -1)
                {
                    Destroy(clone);
                }
            }
        }
    }

    private void Update()
    {
        if (startAnimateValue > 0) { startAnimateValue--; } // for starting
        // blink
        if (lastPressedTintObj != null && !won) // having an activated arrow and not won
        {
            if (blinkTimer == 0)
            {
                lastPressedTintObj.tintScale = 0.3f;
                blinkTimer = 100; // reset timer loop
            }
            else { blinkTimer--; }
        }
    }

    public float getCellSize()
    {
        return cellSize;
    }
}
