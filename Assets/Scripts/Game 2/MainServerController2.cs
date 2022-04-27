using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Microsoft.AspNetCore.SignalR.Client;

public class MainServerController2 : MonoBehaviour
{
    [SerializeField] private string serverUrlFileName;
    private string serverUrl;
    private HubConnection hubConnection;

    private bool gameStartIsMyTurn = false;
    private bool gameStarted = false;
    private bool disconnect = false;
    private bool gameEnded = false;
    private bool fightStarted = false;
    private float gameEndTime = 0.0f;

    public bool gameResult = false;

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
                serverUrl = "http://localhost:5000/shipbattle";
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

        if (sMessage.Length > 0)
        {
            GameObject serverControllerObject;
            ServerController serverController;
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

                case "started":
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
                    break;

                /*
                case "success":
                    break;
                */

                case "fight":
                    serverControllerObject = GameObject.FindGameObjectWithTag("serverController");
                    if (serverControllerObject != null)
                    {
                        serverController = serverControllerObject.GetComponent<ServerController>();
                        serverController.ServerHandle_StartBattle();
                        serverController.ChangeTurn(gameStartIsMyTurn);
                        fightStarted = true;
                    }
                    break;

                case "killed":
                    serverControllerObject = GameObject.FindGameObjectWithTag("serverController");
                    if (sMessage.Length == 1)
                    {
                        // Callback on previous shoot
                        if (serverControllerObject != null)
                        {
                            serverController = serverControllerObject.GetComponent<ServerController>();
                            serverController.ServerHandle_ShootCallback("killed");
                        }
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
                            break;
                        }
                        if (serverControllerObject != null)
                        {
                            serverController = serverControllerObject.GetComponent<ServerController>();
                            serverController.ServerHandle_Shoot(shootX, shootY, "killed");
                        }
                    }
                    break;

                case "injured":
                    serverControllerObject = GameObject.FindGameObjectWithTag("serverController");
                    if (sMessage.Length == 1)
                    {
                        // Callback on previous shoot
                        if (serverControllerObject != null)
                        {
                            serverController = serverControllerObject.GetComponent<ServerController>();
                            serverController.ServerHandle_ShootCallback("damaged");
                        }
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
                            break;
                        }
                        if (serverControllerObject != null)
                        {
                            serverController = serverControllerObject.GetComponent<ServerController>();
                            serverController.ServerHandle_Shoot(shootX, shootY, "damaged");
                        }
                    }
                    break;

                case "missed":
                    serverControllerObject = GameObject.FindGameObjectWithTag("serverController");
                    if (sMessage.Length == 1)
                    {
                        // Callback on previous shoot
                        if (serverControllerObject != null)
                        {
                            serverController = serverControllerObject.GetComponent<ServerController>();
                            serverController.ServerHandle_ShootCallback("missed");
                        }
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
                            break;
                        }
                        if (serverControllerObject != null)
                        {
                            serverController = serverControllerObject.GetComponent<ServerController>();
                            serverController.ServerHandle_Shoot(shootX, shootY, "missed");
                        }
                    }
                    break;

                case "win":
                    if (hubConnection.ConnectionId != null)
                    {
                        hubConnection.StopAsync();
                        hubConnection.DisposeAsync();
                        hubConnection = null;
                    }
                    if (fightStarted)
                    {
                        ServerHandle_EndBattle(true);
                        gameEnded = true;
                    }
                    break;

                case "lose":
                    if (hubConnection.ConnectionId != null)
                    {
                        hubConnection.StopAsync();
                        hubConnection.DisposeAsync();
                        hubConnection = null;
                    }
                    if (fightStarted)
                    {
                        ServerHandle_EndBattle(false);
                        gameEnded = true;
                    }
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
        SceneManager.LoadScene("Game 2");
    }


    public void SendReady(int[][] shipPos)
    {
        if (hubConnection == null) return;
        if (hubConnection.ConnectionId == null) return;
        hubConnection.InvokeAsync("SetShips", shipPos);
    }

    public void SendShoot(int shootX, int shootY)
    {
        if (hubConnection == null) return;
        if (hubConnection.ConnectionId == null) return;
        hubConnection.InvokeAsync("Shoot", shootX, shootY);
    }

    private void ServerHandle_EndBattle(bool win)
    {
        gameResult = win;
        gameEndTime = Time.realtimeSinceStartup;
    }

    private void ServerHandle_Error(int errorCode)
    {
        // Something here
    }
}
