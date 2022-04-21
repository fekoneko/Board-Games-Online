using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerController : MonoBehaviour
{
    [SerializeField] private GameObject shipManagerObject;
    [SerializeField] private GameObject opponentSlotManagerObject;
    [SerializeField] private GameObject readyButtonObject;

    private ShipManager shipManager;
    private SlotManager opponentSlotManager;
    private ReadyButton readyButton;
    private MainServerController2 mainServerController;

    private bool isMyTurn = false;

    public void Start()
    {
        gameObject.tag = "serverController";

        shipManager = shipManagerObject.GetComponent<ShipManager>();
        opponentSlotManager = opponentSlotManagerObject.GetComponent<SlotManager>();
        readyButton = readyButtonObject.GetComponent<ReadyButton>();

        GameObject mainServerControllerObject = GameObject.FindGameObjectWithTag("mainServerController");
        if (mainServerControllerObject != null)
        {
            mainServerController = mainServerControllerObject.GetComponent<MainServerController2>();
        }
        else
        {
            mainServerController = null;
        }
    }

    public void Update()
    {
        
    }


    public void ServerHandle_StartBattle()
    {
        readyButton.startGame();
    }

    public void ServerHandle_ShootCallback(string callback)
    {
        // Something here
        ChangeTurn(false);
    }

    public void ServerHandle_Shoot(int shootX, int shootY, string callback)
    {
        // Something here
        ChangeTurn(true);
    }

    public void ServerHandle_EndBattle(bool win)
    {
        // Something here
    }


    public void ServerSend_Ready()
    {
        if (mainServerController != null)
        {
            int[][] shipPos = GetShipPositions();
            mainServerController.SendReady(shipPos);
        }
    }

    public void ServerSend_Shoot(int shootX, int shootY)
    {
        if (mainServerController != null)
        {
            mainServerController.SendShoot(shootX, shootY);
        }
    }



    public bool IsMyTurn()
    {
        return isMyTurn;
    }

    public void ChangeTurn(bool turn)
    {
        isMyTurn = turn;
        opponentSlotManager.SetButtonState(isMyTurn);
    }



    private int[][] GetShipPositions()
    {
        int[][] shipPositions = new int[10][];
        shipPositions[0] = new int[4];
        shipPositions[1] = new int[4];
        shipPositions[2] = new int[4];
        shipPositions[3] = new int[4];
        shipPositions[4] = new int[4];
        shipPositions[5] = new int[4];
        shipPositions[6] = new int[4];
        shipPositions[7] = new int[4];
        shipPositions[8] = new int[4];
        shipPositions[9] = new int[4];
        int i = 0;
        foreach (GameObject iship in shipManager.ships)
        {
            ShipDragDrop shipCur = iship.GetComponent<ShipDragDrop>();
            Vector2Int mainPartPos = new Vector2Int((int)shipCur.GetPinShipSlot().cell.x, (int)shipCur.GetPinShipSlot().cell.y);
            switch (shipCur.size)
            {
                case 1:
                    shipPositions[i][0] = mainPartPos.x;
                    shipPositions[i][1] = mainPartPos.y;
                    shipPositions[i][2] = mainPartPos.x;
                    shipPositions[i][3] = mainPartPos.y;
                    break;
                case 2:
                    if (shipCur.horizontally)
                    {
                        shipPositions[i][0] = mainPartPos.x;
                        shipPositions[i][1] = mainPartPos.y;
                        shipPositions[i][2] = mainPartPos.x + 1;
                        shipPositions[i][3] = mainPartPos.y;
                    }
                    else
                    {
                        shipPositions[i][0] = mainPartPos.x;
                        shipPositions[i][1] = mainPartPos.y;
                        shipPositions[i][2] = mainPartPos.x;
                        shipPositions[i][3] = mainPartPos.y + 1;
                    }
                    break;
                case 3:
                    if (shipCur.horizontally)
                    {
                        shipPositions[i][0] = mainPartPos.x - 1;
                        shipPositions[i][1] = mainPartPos.y;
                        shipPositions[i][2] = mainPartPos.x + 1;
                        shipPositions[i][3] = mainPartPos.y;
                    }
                    else
                    {
                        shipPositions[i][0] = mainPartPos.x;
                        shipPositions[i][1] = mainPartPos.y - 1;
                        shipPositions[i][2] = mainPartPos.x;
                        shipPositions[i][3] = mainPartPos.y + 1;
                    }
                    break;
                case 4:
                    if (shipCur.horizontally)
                    {
                        shipPositions[i][0] = mainPartPos.x - 1;
                        shipPositions[i][1] = mainPartPos.y;
                        shipPositions[i][2] = mainPartPos.x + 2;
                        shipPositions[i][3] = mainPartPos.y;
                    }
                    else
                    {
                        shipPositions[i][0] = mainPartPos.x;
                        shipPositions[i][1] = mainPartPos.y - 1;
                        shipPositions[i][2] = mainPartPos.x;
                        shipPositions[i][3] = mainPartPos.y + 2;
                    }
                    break;
                default:
                    shipPositions[i][0] = -1;
                    shipPositions[i][1] = -1;
                    shipPositions[i][2] = -1;
                    shipPositions[i][3] = -1;
                    break;
            }
            i++;
        }
        return shipPositions;
    }
}
