using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerController : MonoBehaviour
{
    [SerializeField] private GameObject shipManagerObject;
    [SerializeField] private GameObject slotManagerObject;
    [SerializeField] private GameObject opponentSlotManagerObject;
    [SerializeField] private GameObject readyButtonObject;

    private ShipManager shipManager;
    public SlotManager slotManager;
    public SlotManager opponentSlotManager;
    private ReadyButton readyButton;
    private MainServerController2 mainServerController;

    private bool isMyTurn = false;

    public void Start()
    {
        gameObject.tag = "serverController";

        shipManager = shipManagerObject.GetComponent<ShipManager>();
        slotManager = slotManagerObject.GetComponent<SlotManager>();
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
        if (opponentSlotManager.lastShootX > -1 && opponentSlotManager.lastShootY > -1 &&
            opponentSlotManager.lastShootX < 10 && opponentSlotManager.lastShootY < 10)
        {
            switch (callback)
            {
                case "missed":
                    opponentSlotManager.cells[opponentSlotManager.lastShootX][opponentSlotManager.lastShootY].GetComponent<OpponentCell>().MakeMissed();
                    ChangeTurn(false);
                    break;
                case "damaged":
                    opponentSlotManager.cells[opponentSlotManager.lastShootX][opponentSlotManager.lastShootY].GetComponent<OpponentCell>().MakeDamaged();
                    ChangeTurn(true);
                    break;
                case "killed":
                    opponentSlotManager.cells[opponentSlotManager.lastShootX][opponentSlotManager.lastShootY].GetComponent<OpponentCell>().MakeDamaged();
                    DisplayKilledShip(opponentSlotManager.lastShootX, opponentSlotManager.lastShootY);
                    ChangeTurn(true);
                    break;
            }
        }
        else
        {
            ChangeTurn(false);
        }
    }

    public void ServerHandle_Shoot(int shootX, int shootY, string callback)
    {
        switch(callback)
        {
            case "missed":
                slotManager.cells[shootY][shootX].GetComponent<ShipSlot>().MakeMissed();
                ChangeTurn(true);
                break;
            case "damaged":
                slotManager.cells[shootY][shootX].GetComponent<ShipSlot>().MakeDamaged();
                ChangeTurn(false);
                break;
            case "killed":
                slotManager.cells[shootY][shootX].GetComponent<ShipSlot>().MakeDamaged();
                DisplayMyKilledShip(shootY, shootX);
                ChangeTurn(false);
                break;
        }
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
        opponentSlotManager.SetButtonState(turn);
        if (turn)
        {
            opponentSlotManager.turnTimer.ShowTimer(30.0f);
            slotManager.turnTimer.HideTimer();
        }
        else
        {
            slotManager.turnTimer.ShowTimer(30.0f);
            opponentSlotManager.turnTimer.HideTimer();
        }
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

    private void DisplayKilledShip(int x, int y)
    {
        if (x < 0 || x > 9 || y < 0 || y > 9) return;
        if (!opponentSlotManager.cells[x][y].GetComponent<OpponentCell>().damaged) return;

        int[][] shipParts = new int[][] { null, null, null, null };
        shipParts[0] = new int[] { x, y };

        int shift = 1;
        int shipPartNum = 1;
        while (true) // Check up
        {
            if (shift > 3 || shipPartNum > 3)
            {
                break;
            }
            if (y + shift < 10)
            {
                if (opponentSlotManager.cells[x][y + shift].GetComponent<OpponentCell>().damaged)
                {
                    shipParts[shipPartNum] = new int[] { x, y + shift };
                    shipPartNum++;
                }
                else break;
            }
            shift++;
        }
        shift = -1;
        while (true) // Check down
        {
            if (shift < -3 || shipPartNum > 3)
            {
                break;
            }
            if (y + shift > -1)
            {
                if (opponentSlotManager.cells[x][y + shift].GetComponent<OpponentCell>().damaged)
                {
                    shipParts[shipPartNum] = new int[] { x, y + shift };
                    shipPartNum++;
                }
                else break;
            }
            shift--;
        }
        shift = 1;
        while (true) // Check right
        {
            if (shift > 3 || shipPartNum > 3)
            {
                break;
            }
            if (x + shift < 10)
            {
                if (opponentSlotManager.cells[x + shift][y].GetComponent<OpponentCell>().damaged)
                {
                    shipParts[shipPartNum] = new int[] { x + shift, y };
                    shipPartNum++;
                }
                else break;
            }
            shift++;
        }
        shift = -1;
        while (true) // Check left
        {
            if (shift < -3 || shipPartNum > 3)
            {
                break;
            }
            if (x + shift > -1)
            {
                if (opponentSlotManager.cells[x + shift][y].GetComponent<OpponentCell>().damaged)
                {
                    shipParts[shipPartNum] = new int[] { x + shift, y };
                    shipPartNum++;
                }
                else break;
            }
            shift--;
        }

        for (int i = 0; i < shipParts.Length; i++)
        {
            if (shipParts[i] != null)
            {
                bool noU = false;
                bool noL = false;
                bool noB = false;
                bool noR = false;
                for (int j = 0; j < shipParts.Length; j++)
                {
                    if (shipParts[j] != null)
                    {
                        if (shipParts[i][1] + 1 == shipParts[j][1] && shipParts[i][0] == shipParts[j][0]) noU = true;
                        if (shipParts[i][0] - 1 == shipParts[j][0] && shipParts[i][1] == shipParts[j][1]) noL = true;
                        if (shipParts[i][1] - 1 == shipParts[j][1] && shipParts[i][0] == shipParts[j][0]) noB = true;
                        if (shipParts[i][0] + 1 == shipParts[j][0] && shipParts[i][1] == shipParts[j][1]) noR = true;
                    }
                }
                string spriteID = "";
                float rotation = 0f;
                if (!noU && !noL && !noB && !noR)
                {
                    spriteID = "ULBR";
                    rotation = 0f;
                }
                else if (!noU && !noL && noB && !noR)
                {
                    spriteID = "ULR";
                    rotation = 0f;
                }
                else if (!noU && noL && !noB && !noR)
                {
                    spriteID = "ULR";
                    rotation = 90f;
                }
                else if (noU && !noL && !noB && !noR)
                {
                    spriteID = "LBR";
                    rotation = 0f;
                }
                else if (!noU && !noL && !noB && noR)
                {
                    spriteID = "LBR";
                    rotation = 90f;
                }
                else if (noU && !noL && noB && !noR)
                {
                    spriteID = "LR";
                    rotation = 0f;
                }
                else if (!noU && noL && !noB && noR)
                {
                    spriteID = "LR";
                    rotation = 90f;
                }
                else spriteID = "ULBR";
                // All the derections are completely confused, do not look on 'em. I'm sorry ^_^

                opponentSlotManager.cells[shipParts[i][0]][shipParts[i][1]].GetComponent<OpponentCell>().SetShipSprite(spriteID, rotation + 90f);

                // Paint around
                /*
                int[] s0 = shipParts[0];
                int[] s1;
                if (shipParts[1] != null) s1 = shipParts[1];
                else s1 = new int[] { -1, -1 };
                int[] s2;
                if (shipParts[2] != null) s2 = shipParts[2];
                else s2 = new int[] { -1, -1 };
                int[] s3;
                if (shipParts[3] != null) s3 = shipParts[3];
                else s3 = new int[] { -1, -1 };

                if ((s0[0] != shipParts[i][0] - 1 || s0[1] != shipParts[i][1] - 1) &&
                    (s1[0] != shipParts[i][0] - 1 || s1[1] != shipParts[i][1] - 1) &&
                    (s2[0] != shipParts[i][0] - 1 || s2[1] != shipParts[i][1] - 1) &&
                    (s3[0] != shipParts[i][0] - 1 || s3[1] != shipParts[i][1] - 1))
                    if (shipParts[i][0] - 1 > -1 && shipParts[i][1] - 1 > -1)
                        opponentSlotManager.cells[shipParts[i][0] - 1][shipParts[i][1] - 1].GetComponent<OpponentCell>().MakePainted();
                if ((s0[0] != shipParts[i][0] - 1 || s0[1] != shipParts[i][1] + 0) &&
                    (s1[0] != shipParts[i][0] - 1 || s1[1] != shipParts[i][1] + 0) &&
                    (s2[0] != shipParts[i][0] - 1 || s2[1] != shipParts[i][1] + 0) &&
                    (s3[0] != shipParts[i][0] - 1 || s3[1] != shipParts[i][1] + 0))
                    if (shipParts[i][0] - 1 > -1)
                        opponentSlotManager.cells[shipParts[i][0] - 1][shipParts[i][1] + 0].GetComponent<OpponentCell>().MakePainted();
                if ((s0[0] != shipParts[i][0] - 1 || s0[1] != shipParts[i][1] + 1) &&
                    (s1[0] != shipParts[i][0] - 1 || s1[1] != shipParts[i][1] + 1) &&
                    (s2[0] != shipParts[i][0] - 1 || s2[1] != shipParts[i][1] + 1) &&
                    (s3[0] != shipParts[i][0] - 1 || s3[1] != shipParts[i][1] + 1))
                    if (shipParts[i][0] - 1 > -1 && shipParts[i][1] + 1 < 10)
                        opponentSlotManager.cells[shipParts[i][0] - 1][shipParts[i][1] + 1].GetComponent<OpponentCell>().MakePainted();

                if ((s0[0] != shipParts[i][0] + 1 || s0[1] != shipParts[i][1] - 1) &&
                    (s1[0] != shipParts[i][0] + 1 || s1[1] != shipParts[i][1] - 1) &&
                    (s2[0] != shipParts[i][0] + 1 || s2[1] != shipParts[i][1] - 1) &&
                    (s3[0] != shipParts[i][0] + 1 || s3[1] != shipParts[i][1] - 1))
                    if (shipParts[i][0] + 1 < 10 && shipParts[i][1] - 1 > -1)
                        opponentSlotManager.cells[shipParts[i][0] + 1][shipParts[i][1] - 1].GetComponent<OpponentCell>().MakePainted();
                if ((s0[0] != shipParts[i][0] + 1 || s0[1] != shipParts[i][1] + 0) &&
                    (s1[0] != shipParts[i][0] + 1 || s1[1] != shipParts[i][1] + 0) &&
                    (s2[0] != shipParts[i][0] + 1 || s2[1] != shipParts[i][1] + 0) &&
                    (s3[0] != shipParts[i][0] + 1 || s3[1] != shipParts[i][1] + 0))
                    if (shipParts[i][0] + 1 < 10)
                        opponentSlotManager.cells[shipParts[i][0] + 1][shipParts[i][1] + 0].GetComponent<OpponentCell>().MakePainted();
                if ((s0[0] != shipParts[i][0] + 1 || s0[1] != shipParts[i][1] + 1) &&
                    (s1[0] != shipParts[i][0] + 1 || s1[1] != shipParts[i][1] + 1) &&
                    (s2[0] != shipParts[i][0] + 1 || s2[1] != shipParts[i][1] + 1) &&
                    (s3[0] != shipParts[i][0] + 1 || s3[1] != shipParts[i][1] + 1))
                    if (shipParts[i][0] + 1 < 10 && shipParts[i][1] + 1 < 10)
                        opponentSlotManager.cells[shipParts[i][0] + 1][shipParts[i][1] +  1].GetComponent<OpponentCell>().MakePainted();

                if ((s0[0] != shipParts[i][0] + 0 || s0[1] != shipParts[i][1] - 1) &&
                    (s1[0] != shipParts[i][0] + 0 || s1[1] != shipParts[i][1] - 1) &&
                    (s2[0] != shipParts[i][0] + 0 || s2[1] != shipParts[i][1] - 1) &&
                    (s3[0] != shipParts[i][0] + 0 || s3[1] != shipParts[i][1] - 1))
                    if (shipParts[i][1] - 1 > -1)
                        opponentSlotManager.cells[shipParts[i][0] + 0][shipParts[i][1] - 1].GetComponent<OpponentCell>().MakePainted();
                if ((s0[0] != shipParts[i][0] + 0 || s0[1] != shipParts[i][1] + 1) &&
                    (s1[0] != shipParts[i][0] + 0 || s1[1] != shipParts[i][1] + 1) &&
                    (s2[0] != shipParts[i][0] + 0 || s2[1] != shipParts[i][1] + 1) &&
                    (s3[0] != shipParts[i][0] + 0 || s3[1] != shipParts[i][1] + 1))
                    if (shipParts[i][1] + 1 < 10)
                        opponentSlotManager.cells[shipParts[i][0] + 0][shipParts[i][1] + 1].GetComponent<OpponentCell>().MakePainted();
                */

                // Don't worry about killed cellss (they will not be painted). This is more beautiful
                if (shipParts[i][0] - 1 > -1 && shipParts[i][1] - 1 > -1)
                    opponentSlotManager.cells[shipParts[i][0] - 1][shipParts[i][1] - 1].GetComponent<OpponentCell>().MakePainted();
                if (shipParts[i][0] - 1 > -1)
                    opponentSlotManager.cells[shipParts[i][0] - 1][shipParts[i][1]].GetComponent<OpponentCell>().MakePainted();
                if (shipParts[i][0] - 1 > -1 && shipParts[i][1] + 1 < 10)
                    opponentSlotManager.cells[shipParts[i][0] - 1][shipParts[i][1] + 1].GetComponent<OpponentCell>().MakePainted();

                if (shipParts[i][0] + 1 < 10 && shipParts[i][1] - 1 > -1)
                    opponentSlotManager.cells[shipParts[i][0] + 1][shipParts[i][1] - 1].GetComponent<OpponentCell>().MakePainted();
                if (shipParts[i][0] + 1 < 10)
                    opponentSlotManager.cells[shipParts[i][0] + 1][shipParts[i][1]].GetComponent<OpponentCell>().MakePainted();
                if (shipParts[i][0] + 1 < 10 && shipParts[i][1] + 1 < 10)
                    opponentSlotManager.cells[shipParts[i][0] + 1][shipParts[i][1] + 1].GetComponent<OpponentCell>().MakePainted();

                if (shipParts[i][1] - 1 > -1)
                    opponentSlotManager.cells[shipParts[i][0]][shipParts[i][1] - 1].GetComponent<OpponentCell>().MakePainted();
                if (shipParts[i][1] + 1 < 10)
                    opponentSlotManager.cells[shipParts[i][0]][shipParts[i][1] + 1].GetComponent<OpponentCell>().MakePainted();
            }
        }
    }
    private void DisplayMyKilledShip(int x, int y)
    {
        if (x < 0 || x > 9 || y < 0 || y > 9) return;
        if (!slotManager.cells[x][y].GetComponent<ShipSlot>().damaged) return;

        int[][] shipParts = new int[][] { null, null, null, null };
        shipParts[0] = new int[] { x, y };

        int shift = 1;
        int shipPartNum = 1;
        while (true) // Check up
        {
            if (shift > 3 || shipPartNum > 3)
            {
                break;
            }
            if (y + shift < 10)
            {
                if (slotManager.cells[x][y + shift].GetComponent<ShipSlot>().damaged)
                {
                    shipParts[shipPartNum] = new int[] { x, y + shift };
                    shipPartNum++;
                }
                else break;
            }
            shift++;
        }
        shift = -1;
        while (true) // Check down
        {
            if (shift < -3 || shipPartNum > 3)
            {
                break;
            }
            if (y + shift > -1)
            {
                if (slotManager.cells[x][y + shift].GetComponent<ShipSlot>().damaged)
                {
                    shipParts[shipPartNum] = new int[] { x, y + shift };
                    shipPartNum++;
                }
                else break;
            }
            shift--;
        }
        shift = 1;
        while (true) // Check right
        {
            if (shift > 3 || shipPartNum > 3)
            {
                break;
            }
            if (x + shift < 10)
            {
                if (slotManager.cells[x + shift][y].GetComponent<ShipSlot>().damaged)
                {
                    shipParts[shipPartNum] = new int[] { x + shift, y };
                    shipPartNum++;
                }
                else break;
            }
            shift++;
        }
        shift = -1;
        while (true) // Check left
        {
            if (shift < -3 || shipPartNum > 3)
            {
                break;
            }
            if (x + shift > -1)
            {
                if (slotManager.cells[x + shift][y].GetComponent<ShipSlot>().damaged)
                {
                    shipParts[shipPartNum] = new int[] { x + shift, y };
                    shipPartNum++;
                }
                else break;
            }
            shift--;
        }

        for (int i = 0; i < shipParts.Length; i++)
        {
            if (shipParts[i] != null)
            {
                // Don't worry about killed cellss (they will not be painted). This is more beautiful
                if (shipParts[i][0] - 1 > -1 && shipParts[i][1] - 1 > -1)
                    slotManager.cells[shipParts[i][0] - 1][shipParts[i][1] - 1].GetComponent<ShipSlot>().MakePainted();
                if (shipParts[i][0] - 1 > -1)
                    slotManager.cells[shipParts[i][0] - 1][shipParts[i][1]].GetComponent<ShipSlot>().MakePainted();
                if (shipParts[i][0] - 1 > -1 && shipParts[i][1] + 1 < 10)
                    slotManager.cells[shipParts[i][0] - 1][shipParts[i][1] + 1].GetComponent<ShipSlot>().MakePainted();

                if (shipParts[i][0] + 1 < 10 && shipParts[i][1] - 1 > -1)
                    slotManager.cells[shipParts[i][0] + 1][shipParts[i][1] - 1].GetComponent<ShipSlot>().MakePainted();
                if (shipParts[i][0] + 1 < 10)
                    slotManager.cells[shipParts[i][0] + 1][shipParts[i][1]].GetComponent<ShipSlot>().MakePainted();
                if (shipParts[i][0] + 1 < 10 && shipParts[i][1] + 1 < 10)
                    slotManager.cells[shipParts[i][0] + 1][shipParts[i][1] + 1].GetComponent<ShipSlot>().MakePainted();

                if (shipParts[i][1] - 1 > -1)
                    slotManager.cells[shipParts[i][0]][shipParts[i][1] - 1].GetComponent<ShipSlot>().MakePainted();
                if (shipParts[i][1] + 1 < 10)
                    slotManager.cells[shipParts[i][0]][shipParts[i][1] + 1].GetComponent<ShipSlot>().MakePainted();
            }
        }
    }
}
