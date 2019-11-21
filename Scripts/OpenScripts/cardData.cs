using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class cardData : MonoBehaviour
{
    public bool reveal;
    public float scale;
    public Transform transform;

    //----------------------------
    public GameObject content;
    public GameObject arrowTab;
    public GameObject currencyTab;
    public GameObject bonusTab;

    // arrow tab
    public Image arrowImage;
    public GameObject newTab;
    public GameObject oldTab;
    public TextMeshPro refundValue;

    // currency tab
    public SpriteRenderer bigIcon;
    public TextMeshPro actualValue;

    //--------------------------------------------
    public int colorIndex;
    public bool isArrow;

        // arrows
    public int pickedArrowIndex;
    public bool isNew;
    // not new
    public int actualRefundValue;

        // currency
    public bool isBonus;
    // not bonus
    public bool isStar;
    public int actualCurrencyValue;
    //--------------------------------------

    public Sprite[] arrowSkins;
    public Color[] arrowColors;
    public Sprite hintImage;
    public Sprite starImage;

    private void Start()
    {
        // content.SetActive(true); // remove this

        if (isBonus)
        {
            bonusTab.SetActive(true);
        }
        else if (isArrow)
        {
            arrowTab.SetActive(true); // then set arrow image
            arrowImage.sprite = arrowSkins[pickedArrowIndex];
            arrowImage.color = arrowColors[pickedArrowIndex];

            // new or owned?
            refundValue.text = actualRefundValue + "";
            if (isNew) { newTab.SetActive(true); }
            else { oldTab.SetActive(true); }
        }
        else
        {
            currencyTab.SetActive(true);

            actualValue.text = actualCurrencyValue + "";
            if (isStar)
            {
                bigIcon.sprite = starImage;
            }
            else
            {
                bigIcon.sprite = hintImage;
                bigIcon.color = new Color(1, 1, 1);
                actualValue.color = new Color(1, 1, 0);
            }
        }
    }

    private void Update()
    {
        if (scale < 7f)
        {
            scale += 0.2f;
            transform.localScale = new Vector3(scale, scale, 1);
        }
        else if (scale < 10f && reveal)
        {
            scale += 0.2f;
            transform.localScale = new Vector3(scale, scale, 1);

            if (scale >= 10f)
            {
                content.SetActive(true);
            }
        }
        
    }
}
