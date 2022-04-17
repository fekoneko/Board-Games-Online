using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerController : MonoBehaviour
{
    [SerializeField] GameObject shipManagerObject;

    private ShipManager shipManager;


    void Start()
    {
        shipManager = shipManagerObject.GetComponent<ShipManager>();
    }

    void Update()
    {
        
    }

    private Vector2[,] GetShipPositions()
    {
        Vector2[,] shipPositions = new Vector2[10, 2];
        int i = 0;
        foreach (GameObject iship in shipManager.ships)
        {
            ShipDragDrop shipCur = iship.GetComponent<ShipDragDrop>();
            Vector2 mainPartPos = shipCur.GetPinShipSlot().cell;
            switch (shipCur.size)
            {
                case 1:
                    shipPositions[i, 0] = mainPartPos;
                    shipPositions[i, 1] = mainPartPos;
                    break;
                case 2:
                    if (shipCur.horizontally)
                    {
                        shipPositions[i, 0] = mainPartPos;
                        shipPositions[i, 1] = new Vector2(mainPartPos.x + 1, mainPartPos.y);
                    }
                    else
                    {
                        shipPositions[i, 0] = mainPartPos;
                        shipPositions[i, 1] = new Vector2(mainPartPos.x, mainPartPos.y + 1);
                    }
                    break;
                case 3:
                    if (shipCur.horizontally)
                    {
                        shipPositions[i, 0] = new Vector2(mainPartPos.x - 1, mainPartPos.y);
                        shipPositions[i, 1] = new Vector2(mainPartPos.x + 1, mainPartPos.y);
                    }
                    else
                    {
                        shipPositions[i, 0] = new Vector2(mainPartPos.x, mainPartPos.y - 1);
                        shipPositions[i, 1] = new Vector2(mainPartPos.x, mainPartPos.y + 1);
                    }
                    break;
                case 4:
                    if (shipCur.horizontally)
                    {
                        shipPositions[i, 0] = new Vector2(mainPartPos.x - 1, mainPartPos.y);
                        shipPositions[i, 1] = new Vector2(mainPartPos.x + 2, mainPartPos.y);
                    }
                    else
                    {
                        shipPositions[i, 0] = new Vector2(mainPartPos.x, mainPartPos.y - 1);
                        shipPositions[i, 1] = new Vector2(mainPartPos.x, mainPartPos.y + 2);
                    }
                    break;
                default:
                    shipPositions[i, 0] = Vector2.zero;
                    shipPositions[i, 1] = Vector2.zero;
                    break;
            }
            i++;
        }
        return shipPositions;
    }





    public void SendReady()
    {
        Vector2[,] shipPos = GetShipPositions();
    }
}
