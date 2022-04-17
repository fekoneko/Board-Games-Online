using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [SerializeField] public GameObject[] ships;
    [SerializeField] public GameObject slotManagerObject;
    public bool rotateShips = false;

    private SlotManager slotManager;

    public void Start()
    {
        slotManager = slotManagerObject.GetComponent<SlotManager>();
    }

    public void SetShipDragging(bool en)
    {
        foreach (GameObject i in ships)
        {
            i.GetComponent<ShipDragDrop>().SetState(en);
        }
    }

    public void SetRandomPositions()
    {
        foreach (GameObject i in ships)
        {
            i.GetComponent<ShipDragDrop>().Unpin();
        }
        // 'ships' array should start from the longest ship
        foreach (GameObject i in ships)
        {
            bool[,] remainPositions = new bool[20, 10] {
                { true, true, true, true, true, true, true, true, true, true }, // Rotate 0 deg
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },

                { true, true, true, true, true, true, true, true, true, true }, // Rotate 90 deg
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true },
            };
            while (true) // Break is in the loop body
            {
                int rx;
                int ry;
                while (true)
                {
                    rx = Random.Range(0, 20);
                    ry = Random.Range(0, 10);
                    bool correct = false;
                    for (int ii = 0; ii < remainPositions.GetLength(0); ii++)
                    {
                        for (int jj = 0; jj < remainPositions.GetLength(1); jj++)
                        {
                            if (remainPositions[ii, jj])
                            {
                                correct = true;
                                break;
                            }
                        }
                        if (correct) break;
                    }
                    if (correct) break;
                }
                bool hor;
                bool horPr;
                if (rx > 9)
                {
                    hor = false;
                    rx -= 10;
                }
                else
                {
                    hor = true;
                }
                ShipSlot itemSlot = slotManager.cells[ry][rx].GetComponent<ShipSlot>();
                ShipDragDrop shipCur = i.GetComponent<ShipDragDrop>();
                horPr = shipCur.horizontally;
                shipCur.horizontally = hor;
                if (itemSlot.CheckCanBeDropped(i))
                {
                    shipCur.Pin(itemSlot.transform.position, itemSlot);
                    if (!hor)
                    {
                        shipCur.transform.rotation = Quaternion.AngleAxis(90f, Vector3.back);
                    }
                    else
                    {
                        shipCur.transform.rotation = Quaternion.AngleAxis(0f, Vector3.back);
                    }
                    break;
                }
                else
                {
                    if (!hor)
                    {
                        rx += 10;
                    }
                    remainPositions[rx, ry] = false;
                    shipCur.horizontally = horPr;
                }
            }
        }
    }
}
