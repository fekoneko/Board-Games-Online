using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [SerializeField] private int _size = 3;
    [SerializeField] private Cell _cellPrefab;

    public Cell[,] _grid;
    private MainServerController1 mainServerController;
    public bool isMyTurn = false;
    public bool isCircle = true;
    public int lastX = 0;
    public int lastY = 0;

    public void Start()
    {
        GameObject mainServerControllerObject = GameObject.FindGameObjectWithTag("mainServerController");
        if (mainServerControllerObject != null)
        {
            mainServerController = mainServerControllerObject.GetComponent<MainServerController1>();
        }
        else
        {
            mainServerController = null;
        }
        IinitializeGrid();
    }
    private void IinitializeGrid()
    {
        _grid = new Cell[_size, _size];
        for (int i = 0; i < _size; i++)
            for (int j = 0; j < _size; j++)
            {
                _grid[i, j] = Instantiate(_cellPrefab, transform);
                _grid[i, j].onCellPressed += OnCellPressed;
                _grid[i, j].isCircle = isCircle;
                _grid[i, j].x = i;
                _grid[i, j].y = j;
                _grid[i, j].isMyTurn = isMyTurn;
            }
    }

    private void OnCellPressed(Cell cell)
    {
        cell.SetId(isMyTurn);
        lastX = cell.x;
        lastY = cell.y;
        SetTurn(false);
        //CheckWinner();
    }

    /*
    private void CheckWinner()
    {
        int[] rows = new int[_size];
        int[] cols = new int[_size];
        int mainDiagonal = 0;
        int secondDiagonal = 0;

        for (int i = 0; i < _size; i++)
            for (int j = 0; j < _size; j++)
            {
                int operation = _grid[i, j].isMyTurn == 1 ? 1 : _grid[i, j].isMyTurn == 0 ? -1 : 0;
                rows[i] += operation;
                cols[j] += operation;
                if (i == j)
                    mainDiagonal += operation;

                if (i + j + 1 == _size)
                    secondDiagonal += operation;
            }

        if (mainDiagonal == _size || secondDiagonal == _size)
        {
            Debug.Log("First player is a winner");
            return;
        }

        if (mainDiagonal == -_size || secondDiagonal == -_size)
        {
            Debug.Log("Second player is a winner");
            return;
        }

        for (int i = 0; i < _size; i++)
        {
            if (rows[i] == _size || cols[i] == _size)
            {
                Debug.Log("First player is a winner");
                return;
            }

            if (rows[i] == -_size || cols[i] == -_size)
            {
                Debug.Log("Second player is a winner");
                return;
            }
        }
    }
    */

    public void SetTurn(bool turn)
    {
        isMyTurn = turn;
        
        foreach (Cell i in _grid)
        {
            i.isMyTurn = turn;
            if (i.active) i.GetComponent<Button>().interactable = turn;
        }
    }

    public void ServerHandle_Shoot(int shootX, int shootY)
    {
        
    }
}
