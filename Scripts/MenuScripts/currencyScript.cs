using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class currencyScript : MonoBehaviour
{
    private float xScale = 2f;

    public GameObject ValueObject;

    public bool isStar;
    public int value;
    private bool dontLoad;
    

    void Start()
    {
        if (!dontLoad) { LoadValue(isStar); } // LOAD REAL VALUE
    }

    void Update()
    {
        // not 1 and needs change
        if (xScale > 1) {
            xScale -= 0.1f; xScale = Mathf.Max(xScale, 1f);
            transform.localScale = new Vector3(xScale, 1, 1);
        }
        

    }

    public void LoadValue(bool thisIsStar) {
        if (thisIsStar)
        { value = PlayerPrefs.GetInt("stars"); }
        else { value = PlayerPrefs.GetInt("hints"); }

        setValueToText(value);
    }

    public void setValueToText(int val)
    {
        dontLoad = true;
        string valueAsText = val + "";
        if (val >= 1000) // 4+ figures?
        {
            valueAsText = valueAsText.Substring(0, valueAsText.Length - 3) + "," + valueAsText.Substring(valueAsText.Length - 3);
        }
        ValueObject.GetComponent<TextMeshPro>().text = valueAsText;
    }
}
