using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Microsoft.AspNetCore.SignalR.Client;

public class MainServerController1 : MonoBehaviour
{
    [SerializeField] private string serverUrlFileName;
    private string serverUrl;
    private HubConnection hubConnection;

    public bool gameStartIsMyTurn = false;
    private bool gameStarted = false;
    private bool disconnect = false;
    private bool gameEnded = false;
    public bool gameExited = false;
    private float gameEndTime = 0.0f;

    public int gameResult = 0;

    public void OnEnable()
    {
        if (!gameStarted) // Just in case
        {
            // Read server url
            string path = Application.persistentDataPath + "/" + serverUrlFileName;
            if (System.IO.File.Exists(path))
            {
                serverUrl = System.IO.File.ReadAllText(path);
            }
            else
            {
                //System.IO.File.CreateText(path);
                serverUrl = "http://89.108.114.113/ticktacktoe";
                System.IO.File.WriteAllText(path, serverUrl);
            }

            if (ConnectToServer())
            {
                hubConnection.Closed += HubConnection_Closed;
            }
        }
    }

    public void Start()
    {
        gameObject.tag = "mainServerController";
        DontDestroyOnLoad(gameObject);
    }

    private System.Threading.Tasks.Task HubConnection_Closed(System.Exception arg)
    {
        disconnect = true;
        throw new System.NotImplementedException();
    }

    public void OnDisable()
    {
        if (hubConnection != null)
        {
            hubConnection.StopAsync();
            hubConnection.DisposeAsync();
        }
    }

    public void Update()
    {
        if (disconnect && !gameEnded) // Just a simple disconnect
        {
            if (!gameExited)
            {
                ServerHandle_Error(0); // Server error
            }
            SceneManager.LoadScene("Main Menu");
            Destroy(gameObject);
        }
        if (gameEnded && Time.realtimeSinceStartup - gameEndTime > 1.5f)
        {
            SceneManager.LoadScene("Game End Screen");
            // Delete gameObject later
        }
    }

    private void HandleMessage(string message)
    {
        Debug.Log(message);

        string[] sMessage = message.Split(";");

        GameObject gridControllerObject;
        GridController gridController;
        if (sMessage.Length > 0)
        {
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
                    ServerHandle_Error(errorCode);
                    break;

                case "selected":
                    bool isMyTurn;
                    if (sMessage.Length >= 2) // Just in case
                    {
                        if (sMessage[1] == "True") isMyTurn = true;
                        else isMyTurn = false;
                    }
                    else
                    {
                        Destroy(gameObject);
                        break;
                    }
                    StartGame(isMyTurn);
                    gameStarted = true;
                    gridControllerObject = GameObject.Find("Grid");
                    if (gridControllerObject != null)
                    {
                        gridController = gridControllerObject.GetComponent<GridController>();
                        gridController.SetTurn(isMyTurn);
                        gridController.isCircle = !isMyTurn;
                    }
                    break;

                case "ok":
                    gridControllerObject = GameObject.Find("Grid");
                    if (gridControllerObject != null)
                    {
                        gridController = gridControllerObject.GetComponent<GridController>();
                        gridController.ServerHandle_Ok();
                    }
                    break;

                case "opponent":
                    int shootX;
                    int shootY;
                    gridControllerObject = GameObject.Find("Grid");
                    if (sMessage.Length >= 3) // Just in case
                    {
                        shootX = System.Convert.ToInt32(sMessage[1]);
                        shootY = System.Convert.ToInt32(sMessage[2]);
                    }
                    else
                    {
                        break;
                    }
                    if (gridControllerObject != null)
                    {
                        gridController = gridControllerObject.GetComponent<GridController>();
                        gridController.ServerHandle_Shoot(shootX, shootY);
                        gridController.SetTurn(true);
                    }
                    break;

                case "win":
                    if (hubConnection.ConnectionId != null)
                    {
                        hubConnection.StopAsync();
                        hubConnection.DisposeAsync();
                        hubConnection = null;
                    }
                    ServerHandle_EndGame(1);
                    gameEnded = true;
                    break;

                case "lose":
                    if (hubConnection.ConnectionId != null)
                    {
                        hubConnection.StopAsync();
                        hubConnection.DisposeAsync();
                        hubConnection = null;
                    }
                    ServerHandle_EndGame(0);
                    gameEnded = true;
                    break;

                case "tie":
                    if (hubConnection.ConnectionId != null)
                    {
                        hubConnection.StopAsync();
                        hubConnection.DisposeAsync();
                        hubConnection = null;
                    }
                    ServerHandle_EndGame(2);
                    gameEnded = true;
                    break;
            }
        }
    }

    private bool ConnectToServer()
    {
        if (serverUrl != null)
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(new System.Uri(serverUrl))
                .WithAutomaticReconnect()
                .Build();

            hubConnection.On<string>("Notify", message =>
            {
                HandleMessage(message);
            });
            hubConnection.StartAsync();
            return true;
        }
        else return false;
    }




    private void StartGame(bool isMyTurn)
    {
        gameStartIsMyTurn = isMyTurn;
        //DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Game 1");
    }


    private void ServerHandle_EndGame(int win)
    {
        gameResult = win;
        gameEndTime = Time.realtimeSinceStartup;

        GameObject gridControllerObject = GameObject.Find("Grid");
        if (gridControllerObject != null)
        {
            GridController gridController = gridControllerObject.GetComponent<GridController>();
            if (win != 2) gridController.ShowWinStripe((win == 1) ? true : false);

            gridController.turnTimer.HideTimer();
            gridController.opponentTurnTimer.HideTimer();
        }
    }

    private void ServerHandle_Error(int errorCode)
    {
        switch (errorCode)
        {
            case 1: // Incorrect turn
                GameObject gridControllerObject = GameObject.Find("Grid");
                if (gridControllerObject != null)
                {
                    gridControllerObject.GetComponent<GridController>().SetTurn(true); // Another try to make a turn
                }
                break;
        }
    }
    public void Server_SendShoot(int shootX, int shootY)
    {
        if (hubConnection == null) return;
        if (hubConnection.ConnectionId == null) return;
        hubConnection.InvokeAsync("Move", shootX, shootY);
    }
}
