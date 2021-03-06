using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    [SerializeField] private GameObject[] cells0;
    [SerializeField] private GameObject[] cells1;
    [SerializeField] private GameObject[] cells2;
    [SerializeField] private GameObject[] cells3;
    [SerializeField] private GameObject[] cells4;
    [SerializeField] private GameObject[] cells5;
    [SerializeField] private GameObject[] cells6;
    [SerializeField] private GameObject[] cells7;
    [SerializeField] private GameObject[] cells8;
    [SerializeField] private GameObject[] cells9;

    [SerializeField] public TurnTimer turnTimer;
    [SerializeField] private bool areOpponentCells = false;

    public GameObject[][] cells;

    public int lastShootX = -1; // For opponent cells
    public int lastShootY = -1;

    private void Awake()
    {
        cells = new GameObject[][] { null, null, null, null, null, null, null, null, null, null };
        cells[0] = new GameObject[] { null, null, null, null, null, null, null, null, null, null };
        cells[1] = new GameObject[] { null, null, null, null, null, null, null, null, null, null };
        cells[2] = new GameObject[] { null, null, null, null, null, null, null, null, null, null };
        cells[3] = new GameObject[] { null, null, null, null, null, null, null, null, null, null };
        cells[4] = new GameObject[] { null, null, null, null, null, null, null, null, null, null };
        cells[5] = new GameObject[] { null, null, null, null, null, null, null, null, null, null };
        cells[6] = new GameObject[] { null, null, null, null, null, null, null, null, null, null };
        cells[7] = new GameObject[] { null, null, null, null, null, null, null, null, null, null };
        cells[8] = new GameObject[] { null, null, null, null, null, null, null, null, null, null };
        cells[9] = new GameObject[] { null, null, null, null, null, null, null, null, null, null };
        Array.Copy(cells0, cells[0], 10);
        Array.Copy(cells1, cells[1], 10);
        Array.Copy(cells2, cells[2], 10);
        Array.Copy(cells3, cells[3], 10);
        Array.Copy(cells4, cells[4], 10);
        Array.Copy(cells5, cells[5], 10);
        Array.Copy(cells6, cells[6], 10);
        Array.Copy(cells7, cells[7], 10);
        Array.Copy(cells8, cells[8], 10);
        Array.Copy(cells9, cells[9], 10);
    }

    public void Start()
    {
        if (!areOpponentCells)
        {
            turnTimer.ShowTimer(90.0f);
        }
    }


    // For DragDrop

    public void SetAvailableShow(bool en)
    {
        if (areOpponentCells) return;
        foreach (GameObject[] j in cells)
        {
            foreach (GameObject i in j)
            {
                if (i.GetComponent<ShipSlot>() != null)
                {
                    i.GetComponent<ShipSlot>().SetAvailableShow(en);
                }
            }
        }
    }


    // For OpponentCell

    public void SetButtonState(bool en)
    {
        if (!areOpponentCells) return;
        foreach (GameObject[] j in cells)
        {
            foreach (GameObject i in j)
            {
                if (i.GetComponent<OpponentCell>() != null)
                {
                    i.GetComponent<UnityEngine.UI.Button>().interactable = en;
                }
            }
        }
    }
}
