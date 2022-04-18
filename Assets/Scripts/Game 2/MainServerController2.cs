using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Microsoft.AspNetCore.SignalR.Client;

public class MainServerController2 : MonoBehaviour
{
    [SerializeField] private string serverUrl;
    private HubConnection hubConnection;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        ConnectToServer();
    }

    void Update()
    {
        
    }

    private void MessageHandler(string message)
    {
        string[] sMessage = message.Split(";");

        switch (sMessage[0])
        {
            case "error":
                int errorCode;
                if (sMessage.Length >= 2) // Just in case
                {
                    errorCode = System.Convert.ToInt32(sMessage[1]);
                }
                else
                {
                    errorCode = -1;
                }
                // Func
                break;

            case "started":
                bool isMyTurn;
                if (sMessage.Length >= 2) // Just in case
                {
                    if (sMessage[1] == "True") isMyTurn = true;
                    else isMyTurn = false;
                }
                else
                {
                    isMyTurn = false;
                }
                StartGame(isMyTurn);
                break;
            
            case "success":
                // Func
                break;
            
            case "killed":
                if (sMessage.Length == 1)
                {
                    // Callback on previous shoot
                    // Func
                }
                else
                {
                    // Opponent's shoot on you
                    int shootX;
                    int shootY;
                    if (sMessage.Length >= 3) // Just in case
                    {
                        shootX = System.Convert.ToInt32(sMessage[1]);
                        shootY = System.Convert.ToInt32(sMessage[2]);
                    }
                    else
                    {
                        shootX = -1;
                        shootY = -1;
                    }
                    // Func
                }
                break;
            
            case "injured":
                if (sMessage.Length == 1)
                {
                    // Callback on previous shoot
                    // Func
                }
                else
                {
                    // Opponent's shoot on you
                    int shootX;
                    int shootY;
                    if (sMessage.Length >= 3) // Just in case
                    {
                        shootX = System.Convert.ToInt32(sMessage[1]);
                        shootY = System.Convert.ToInt32(sMessage[2]);
                    }
                    else
                    {
                        shootX = -1;
                        shootY = -1;
                    }
                    // Func
                }
                break;
            
            case "missed":
                if (sMessage.Length == 1)
                {
                    // Callback on previous shoot
                    // Func
                }
                else
                {
                    // Opponent's shoot on you
                    int shootX;
                    int shootY;
                    if (sMessage.Length >= 3) // Just in case
                    {
                        shootX = System.Convert.ToInt32(sMessage[1]);
                        shootY = System.Convert.ToInt32(sMessage[2]);
                    }
                    else
                    {
                        shootX = -1;
                        shootY = -1;
                    }
                    // Func
                }
                break;
            
            case "win":
                // Func
                break;
            
            case "lose":
                // Func
                break;
        }
    }


    private void ConnectToServer()
    {
        hubConnection = new HubConnectionBuilder().WithUrl(serverUrl).Build();

        hubConnection.On<string>("Notify", message =>
        {
            MessageHandler(message);
        });
        hubConnection.StartAsync();
    }







    private void StartGame(bool isMyTurn)
    {
        SceneManager.LoadScene("Game 2");
    }
}
