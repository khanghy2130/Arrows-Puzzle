using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArrowData : MonoBehaviour
{
    public int cx, cy; // degree and solutionNumber stores in CreatePuzzle
    public bool activated = false; // pressed and is vaild
    private AlertScript AlertScript;
    private CreatePuzzle CreatePuzzle;

    // set up position and rotation
    public void changePos(int x, int y)
    {
        cx = x;
        cy = y;
    }

    private void Start()
    {
        CreatePuzzle = GameObject.Find("_LevelManager").GetComponent<CreatePuzzle>();
        AlertScript = GameObject.Find("_SceneManager").GetComponent<AlertScript>();
    }

    // run when mouse clicks on this item
    public void OnMouseDown()
    {
        // not startAnimating? not won? not already pressed? NOT ALERT?
        if (CreatePuzzle.startAnimateValue <= 0 && !CreatePuzzle.won && !activated && AlertScript.messageIndex == 0)
        {
            GetComponent<UpdateTint>().setShrink(0.6f);
            CreatePuzzle.newPlayMove(GetComponent<ArrowData>(), GetComponent<UpdateTint>());
        }
    }
}
