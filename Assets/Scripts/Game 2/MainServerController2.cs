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

    public void OnEnable()
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
            System.IO.File.WriteAllText(path, "http://localhost:5000/shipbattle");
            serverUrl = "http://localhost:5000/shipbattle";
        }

        if (ConnectToServer())
        {
            hubConnection.Closed += HubConnection_Closed;
        }
    }

    public void Start()
    {
        gameObject.tag = "mainServerController";
        DontDestroyOnLoad(gameObject);
    }

    private System.Threading.Tasks.Task HubConnection_Closed(System.Exception arg)
    {
        throw new System.NotImplementedException("Connection closed");
    }

    public void OnDisable()
    {
        hubConnection.StopAsync();
        hubConnection.DisposeAsync();
    }

    public void Update()
    {
        //if (gameObject.scene.name == "DontDestroyOnLoad")
        //{
        //    SceneManager.MergeScenes(gameObject.scene, SceneManager.GetActiveScene());
        //}
    }

    private void HandleMessage(string message)
    {
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
                        isMyTurn = false;
                    }
                    StartGame(isMyTurn);
                    break;

                case "success":
                    serverControllerObject = GameObject.FindGameObjectWithTag("serverController");
                    if (serverControllerObject != null)
                    {
                        serverController = serverControllerObject.GetComponent<ServerController>();
                        serverController.ServerHandle_StartBattle();
                        serverController.ChangeTurn(gameStartIsMyTurn);
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
                            shootX = -1;
                            shootY = -1;
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
                            shootX = -1;
                            shootY = -1;
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
                            shootX = -1;
                            shootY = -1;
                        }
                        if (serverControllerObject != null)
                        {
                            serverController = serverControllerObject.GetComponent<ServerController>();
                            serverController.ServerHandle_Shoot(shootX, shootY, "damaged");
                        }
                    }
                    break;

                case "win":
                    serverControllerObject = GameObject.FindGameObjectWithTag("serverController");
                    if (serverControllerObject != null)
                    {
                        serverController = serverControllerObject.GetComponent<ServerController>();
                        serverController.ServerHandle_EndBattle(true);
                    }
                    break;

                case "lose":
                    serverControllerObject = GameObject.FindGameObjectWithTag("serverController");
                    if (serverControllerObject != null)
                    {
                        serverController = serverControllerObject.GetComponent<ServerController>();
                        serverController.ServerHandle_EndBattle(false);
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


    public void SendReady(int[,] shipPos)
    {
        hubConnection.InvokeAsync<System.Threading.Tasks.Task>("SetShips", shipPos);
    }

    public void SendShoot(int shootX, int shootY)
    {
        hubConnection.InvokeAsync<System.Threading.Tasks.Task>("Shoot", shootX, shootY);
    }

    public void ServerHandle_Error(int errorCode)
    {
        // Something here
    }
}
