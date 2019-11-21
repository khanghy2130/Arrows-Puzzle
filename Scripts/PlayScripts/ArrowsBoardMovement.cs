using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsBoardMovement : MonoBehaviour
{
    public Canvas GameplayButtons;

    private CreatePuzzle CreatePuzzle;
    private int wonDelay = 150;
    private float yVelocity = 2.5f;

    void Start()
    {
        CreatePuzzle = GameObject.Find("_LevelManager").GetComponent<CreatePuzzle>();
    }

    void Update()
    {
        // start up
        if (CreatePuzzle.startAnimateValue > 0) // animating ?
        {
            transform.position -= new Vector3(0, 2f, 0);
        }

        if (CreatePuzzle.won && wonDelay > 0)
        {
            wonDelay--;
            // update arrows board position
            if (wonDelay < 150 - 50) // after 50..
            { 
                yVelocity -= 0.18f;
                transform.position += new Vector3(0, yVelocity, 0); // change y
                // change z rotate
                Vector3 rot = transform.rotation.eulerAngles;
                rot = new Vector3(rot.x, rot.y, rot.z - wonDelay/50f);
                transform.rotation = Quaternion.Euler(rot);

                // move down GameplayButtons
                GameplayButtons.transform.position -= new Vector3(0, 1, 0);
            }
        }
    }
}
