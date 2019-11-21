using TMPro;
using UnityEngine;

public class LevelTextScript : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("level") + "";
    }
}
