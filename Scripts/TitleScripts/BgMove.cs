using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMove : MonoBehaviour
{
    public Transform Transfrom;
    public float slowFactor;
    void Update()
    {
        float yPos = Mathf.Cos(Time.frameCount / slowFactor) * 10;
        Transfrom.position = new Vector3(0, yPos, 10);
    }
}
