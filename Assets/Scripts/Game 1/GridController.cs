using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private int _size = 3;
    [SerializeField] private Cell _cellPrefab;

    private Cell[,] _grid;
    private int playerId;
    public void Awake()
    {
        playerId = Random.Range(0, 2);
        initializeGrid();
    }
    private void initializeGrid()
    {
        _grid = new Cell[_size, _size];
        for (int i = 0; i < _size; i++)
            for (int j = 0; j < _size; j++)
            {
                _grid[i, j] = Instantiate(_cellPrefab, transform);
                _grid[i, j].onCellPressed += OnCellPressed;
            }
    }

    private void OnCellPressed(Cell cell)
    {
        if (cell.playerId != -1)
            return;

        cell.SetId(playerId);
        CheckWinner();
        SwapPlayer();
    }

    private void CheckWinner()
    {
        int[] rows = new int[_size];
        int[] cols = new int[_size];
        int mainDiagonal = 0;
        int secondDiagonal = 0;

        for (int i = 0; i < _size; i++)
            for (int j = 0; j < _size; j++)
            {
                int operation = _grid[i, j].playerId == 1 ? 1 : _grid[i, j].playerId == 0 ? -1 : 0;
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

    private void SwapPlayer()
    {
        playerId = playerId == 0 ? 1 : 0;
    }
}
