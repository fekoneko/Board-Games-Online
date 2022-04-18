using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerController : MonoBehaviour
{
    [SerializeField] GameObject shipManagerObject;
    [SerializeField] GameObject readyButtonObject;

    private ShipManager shipManager;
    private ReadyButton readyButton;

    private bool readyWaitForAnswer = false;
    private bool shootWaitForAnswer = false;
    private int gameTurn = 0;

    private struct Ship
    {
        public Vector2Int start;
        public Vector2Int end;
    }


    void Start()
    {
        shipManager = shipManagerObject.GetComponent<ShipManager>();
        readyButton = readyButtonObject.GetComponent<ReadyButton>();
    }

    void Update()
    {
        
    }


    // Server send

    // Tell server that the player is ready to start and send ship positions
    public void ServerSendReady() // Call is in ReadyButton
    {
        Ship[] shipPos = GetShipPositions();
        string json = "Ready\n";
        foreach (Ship i in shipPos)
        {
            json += JsonUtility.ToJson(i) + "\n";
        }
        ServerSend(json);

        readyWaitForAnswer = true;
    }

    // Send position which player chose
    public void ServerSendShoot(Vector2Int shootPos)
    {
        string json = "Shoot\n";
        json += JsonUtility.ToJson(shootPos) + "\n";
        ServerSend(json);

        shootWaitForAnswer = true;
    }

    private void ServerSend(string message)
    {
        // Something here

        Debug.Log("Sent to server:\n\n" + message);
    }


    // Server recieve

    public void GameStart()
    {
        if (readyWaitForAnswer)
        {
            readyButton.startGame();

            readyWaitForAnswer = false;
        }
        // Else server did something wrong, game cannot be started
    }

    public void ShootCallback(int callback)
    {
        if (shootWaitForAnswer)
        {
            // Something here

            shootWaitForAnswer = false;
        }
        // Else server did something wrong, there was not any shoot
    }

    public void Shoot(Vector2Int shootPos)
    {
        // Something here
    }

    public void NextTurn(int turn)
    {
        // Something here

        gameTurn = turn;
    }

    public void GameEnd(int win)
    {
        // Something here
    }




    public bool IsMyTurn()
    {
        if (gameTurn == 0) return false;
        else return true;
    }




    private Ship[] GetShipPositions()
    {
        Ship[] shipPositions = new Ship[10];
        int i = 0;
        foreach (GameObject iship in shipManager.ships)
        {
            ShipDragDrop shipCur = iship.GetComponent<ShipDragDrop>();
            Vector2Int mainPartPos = new Vector2Int((int)shipCur.GetPinShipSlot().cell.x, (int)shipCur.GetPinShipSlot().cell.y);
            switch (shipCur.size)
            {
                case 1:
                    shipPositions[i].start = mainPartPos;
                    shipPositions[i].end = mainPartPos;
                    break;
                case 2:
                    if (shipCur.horizontally)
                    {
                        shipPositions[i].start = mainPartPos;
                        shipPositions[i].end = new Vector2Int(mainPartPos.x + 1, mainPartPos.y);
                    }
                    else
                    {
                        shipPositions[i].start = mainPartPos;
                        shipPositions[i].end = new Vector2Int(mainPartPos.x, mainPartPos.y + 1);
                    }
                    break;
                case 3:
                    if (shipCur.horizontally)
                    {
                        shipPositions[i].start = new Vector2Int(mainPartPos.x - 1, mainPartPos.y);
                        shipPositions[i].end = new Vector2Int(mainPartPos.x + 1, mainPartPos.y);
                    }
                    else
                    {
                        shipPositions[i].start = new Vector2Int(mainPartPos.x, mainPartPos.y - 1);
                        shipPositions[i].end = new Vector2Int(mainPartPos.x, mainPartPos.y + 1);
                    }
                    break;
                case 4:
                    if (shipCur.horizontally)
                    {
                        shipPositions[i].start = new Vector2Int(mainPartPos.x - 1, mainPartPos.y);
                        shipPositions[i].end = new Vector2Int(mainPartPos.x + 2, mainPartPos.y);
                    }
                    else
                    {
                        shipPositions[i].start = new Vector2Int(mainPartPos.x, mainPartPos.y - 1);
                        shipPositions[i].end = new Vector2Int(mainPartPos.x, mainPartPos.y + 2);
                    }
                    break;
                default:
                    shipPositions[i].start = Vector2Int.zero;
                    shipPositions[i].end = Vector2Int.zero;
                    break;
            }
            i++;
        }
        return shipPositions;
    }
}
