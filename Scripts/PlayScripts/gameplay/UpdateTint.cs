using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// update the tint of this item, incease if no activated
// also apply shrink animation when just activated
public class UpdateTint : MonoBehaviour
{
    public float tintScale; // white by default
    private bool activated;
    private Renderer rend;

    private CreatePuzzle CreatePuzzle; // to get startAnimateValue & cell size
    public ArrowData ArrowData;
    private float cellSize;
    private float shrinkFactor = 0.6f; // 1 is normal, 0.6 when pressed

    void Start()
    {
        rend = GetComponent<Renderer>();
        tintScale = 1f;

        // set ref of CreatePuzzle
        CreatePuzzle = GameObject.Find("_LevelManager").GetComponent<CreatePuzzle>();
        cellSize = CreatePuzzle.getCellSize(); // get cell size
    }

    void Update()
    {
        int startAnimateValue = CreatePuzzle.startAnimateValue;

        // update tint
        activated = ArrowData.activated;
        if (activated)
        {tintScale += 0.05f;}
        else if (startAnimateValue <= 0)
        {tintScale -= 0.05f;}
        tintScale = Mathf.Clamp(tintScale, 0.25f, 1f); //         Change the tint limit here
        rend.material.color = new Color(tintScale, tintScale, tintScale, 1);

        // update size
        if (shrinkFactor < 1 && startAnimateValue <= 0)
        {
            shrinkFactor += 0.05f;
            if (shrinkFactor > 1) { shrinkFactor = 1f; } // make sure it's 1

            transform.localScale = new Vector3(cellSize * shrinkFactor, cellSize * shrinkFactor, 0);
        }
    }

    public void setShrink( float s)
    {
        shrinkFactor = s;
    }
}
