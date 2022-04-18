using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerController : MonoBehaviour
{
    [SerializeField] GameObject shipManagerObject;
    [SerializeField] GameObject opponentSlotManagerObject;
    [SerializeField] GameObject readyButtonObject;

    private ShipManager shipManager;
    private SlotManager opponentSlotManager;
    private ReadyButton readyButton;

    private int gameTurn = 0;



    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        shipManager = shipManagerObject.GetComponent<ShipManager>();
        opponentSlotManager = opponentSlotManagerObject.GetComponent<SlotManager>();
        readyButton = readyButtonObject.GetComponent<ReadyButton>();
    }

    void Update()
    {
        
    }


    public void GameStart()
    {
        readyButton.startGame();
    }

    public void ShootCallback(int callback)
    {
        // Something here
    }

    public void Shoot(Vector2Int shootPos)
    {
        // Something here
    }

    public void ChangeTurn(int turn)
    {
        gameTurn = turn;
        opponentSlotManager.SetButtonState(IsMyTurn());
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



    private int[,] GetShipPositions()
    {
        int[,] shipPositions = new int[10,4];
        int i = 0;
        foreach (GameObject iship in shipManager.ships)
        {
            ShipDragDrop shipCur = iship.GetComponent<ShipDragDrop>();
            Vector2Int mainPartPos = new Vector2Int((int)shipCur.GetPinShipSlot().cell.x, (int)shipCur.GetPinShipSlot().cell.y);
            switch (shipCur.size)
            {
                case 1:
                    shipPositions[i,0] = mainPartPos.x;
                    shipPositions[i,1] = mainPartPos.y;
                    shipPositions[i,2] = mainPartPos.x;
                    shipPositions[i,3] = mainPartPos.y;
                    break;
                case 2:
                    if (shipCur.horizontally)
                    {
                        shipPositions[i,0] = mainPartPos.x;
                        shipPositions[i,1] = mainPartPos.y;
                        shipPositions[i,2] = mainPartPos.x + 1;
                        shipPositions[i,3] = mainPartPos.y;
                    }
                    else
                    {
                        shipPositions[i,0] = mainPartPos.x;
                        shipPositions[i,1] = mainPartPos.y;
                        shipPositions[i,2] = mainPartPos.x;
                        shipPositions[i,3] = mainPartPos.y + 1;
                    }
                    break;
                case 3:
                    if (shipCur.horizontally)
                    {
                        shipPositions[i,0] = mainPartPos.x - 1;
                        shipPositions[i,1] = mainPartPos.y;
                        shipPositions[i,2] = mainPartPos.x + 1;
                        shipPositions[i,3] = mainPartPos.y;
                    }
                    else
                    {
                        shipPositions[i,0] = mainPartPos.x;
                        shipPositions[i,1] = mainPartPos.y - 1;
                        shipPositions[i,2] = mainPartPos.x;
                        shipPositions[i,3] = mainPartPos.y + 1;
                    }
                    break;
                case 4:
                    if (shipCur.horizontally)
                    {
                        shipPositions[i,0] = mainPartPos.x - 1;
                        shipPositions[i,1] = mainPartPos.y;
                        shipPositions[i,2] = mainPartPos.x + 2;
                        shipPositions[i,3] = mainPartPos.y;
                    }
                    else
                    {
                        shipPositions[i,0] = mainPartPos.x;
                        shipPositions[i,1] = mainPartPos.y - 1;
                        shipPositions[i,2] = mainPartPos.x;
                        shipPositions[i,3] = mainPartPos.y + 2;
                    }
                    break;
                default:
                    shipPositions[i,0] = -1;
                    shipPositions[i,1] = -1;
                    shipPositions[i,2] = -1;
                    shipPositions[i,3] = -1;
                    break;
            }
            i++;
        }
        return shipPositions;
    }
}
