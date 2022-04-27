using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndText : MonoBehaviour
{
    private int gameID = 0; // No game
    private GameObject mainServerControllerObject;
    public bool gameResult = false;

    void Start()
    {
        mainServerControllerObject = GameObject.Find("Main Server Controller 1"); // Tic-Tac-Toe
        if (mainServerControllerObject != null) gameID = 1;
        else
        {
            mainServerControllerObject = GameObject.Find("Main Server Controller 2"); // Ship Battle
            if (mainServerControllerObject != null) gameID = 2;
        }

        switch (gameID)
        {
            case 0:
                // Nothing here
                break;
            case 1:
                gameResult = mainServerControllerObject.GetComponent<MainServerController1>().gameResult;
                break;
            case 2:
                gameResult = mainServerControllerObject.GetComponent<MainServerController2>().gameResult;
                break;
        }

        if (gameResult) // Won
        {
            GetComponent<Text>().text = "Вы победили!";
        }
        else // Lost
        {
            GetComponent<Text>().text = "Вы проиграли!";
        }

        if (gameID != 0)
        {
            Destroy(mainServerControllerObject);
        }
    }

    void Update()
    {
        
    }
}
