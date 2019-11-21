using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleRunner : MonoBehaviour
{
    private int titleTimer = 100; // value to keep showing title, 0 is done
    public GameObject SceneManager;

    // Update is called once per frame
    void Update()
    {
        // running title scene
        if (titleTimer > 0)
        {
            titleTimer--;
            if (titleTimer == 0)
            {
                SceneManager.GetComponent<ChangeScene>().ChangeToScene("Menu");
            }
        }
    }
}
