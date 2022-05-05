using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreeDotsAnimator : MonoBehaviour
{
    private float prTime = -1.0f;
    private int textStep = 0;
    private Text text;
    private string mainString;

    public void Start()
    {
        text = GetComponent<Text>();
        mainString = text.text;
        prTime = Time.realtimeSinceStartup - 1.1f;
    }

    public void Update()
    {
        if (Time.realtimeSinceStartup - prTime > 1.0f)
        {
            string textToWrite = mainString;
            switch (textStep)
            {
                case 0:
                    textToWrite = mainString + ".";
                    break;
                case 1:
                    textToWrite = mainString + "..";
                    break;
                case 2:
                    textToWrite = mainString + "...";
                    break;
            }
            text.text = textToWrite;

            textStep++;
            textStep %= 3;
            prTime += 1.0f;
        }
    }
}
