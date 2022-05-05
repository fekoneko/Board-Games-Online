using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Sprite[] stripeSprites; // 4
    [SerializeField] public TurnTimer turnTimer;
    [SerializeField] public TurnTimer opponentTurnTimer;

    private Cell[,] _grid;
    private bool[,] circleField;
    private bool[,] crossField;
    private MainServerController1 mainServerController;
    public bool isMyTurn = false;
    public bool isCircle = true;
    public int lastX = 0;
    public int lastY = 0;
    private Image[] stripeImage;

    private bool isAnimating = false;
    private float prAnimationStepTime = 0.0f;

    public void Start()
    {
        stripeImage = new Image[] { null, null, null, null, null, null, null, null };

        GameObject mainServerControllerObject = GameObject.FindGameObjectWithTag("mainServerController");
        if (mainServerControllerObject != null)
        {
            mainServerController = mainServerControllerObject.GetComponent<MainServerController1>();
            isMyTurn = mainServerController.gameStartIsMyTurn;
            isCircle = !mainServerController.gameStartIsMyTurn;
        }
        else
        {
            mainServerController = null;
        }
        IinitializeGrid();
        SetTurn(isMyTurn);

        circleField = new bool[,] { { false, false, false }, { false, false, false }, { false, false, false } };
        crossField = new bool[,] { { false, false, false }, { false, false, false }, { false, false, false } };
    }
    private void IinitializeGrid()
    {
        _grid = new Cell[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
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
        mainServerController.Server_SendShoot(cell.x, cell.y);
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

        if (turn)
        {
            turnTimer.ShowTimer(30.0f);
            opponentTurnTimer.HideTimer();
        }
        else
        {
            opponentTurnTimer.ShowTimer(30.0f);
            turnTimer.HideTimer();
        }
    }

    public void ServerHandle_Shoot(int shootX, int shootY)
    {
        bool isCirclePlaced = _grid[shootX, shootY].UpdateBackgroundSprite(false);
        if (isCirclePlaced)
        {
            circleField[shootX, shootY] = true;
        }
        else
        {
            crossField[shootX, shootY] = true;
        }
    }

    public void ServerHandle_Ok()
    {
        bool isCirclePlaced = _grid[lastX, lastY].UpdateBackgroundSprite(true);
        if (isCirclePlaced)
        {
            circleField[lastX, lastY] = true;
        }
        else
        {
            crossField[lastX, lastY] = true;
        }
    }

    public void ShowWinStripe(bool win)
    {
        Vector2Int[][] stripePostion = checkStripePosition(win ? isCircle : !isCircle);
        if (stripePostion != null)
        {
            if (stripePostion.Length > 0)
            {
                int iNum = 0;
                foreach (Vector2Int[] i in stripePostion)
                {
                    if (i == null) break;
                    GameObject stripeObject = new GameObject();
                    stripeImage[iNum] = stripeObject.AddComponent<Image>();
                    isAnimating = true;
                    prAnimationStepTime = Time.realtimeSinceStartup;
                    stripeImage[iNum].sprite = null;
                    stripeImage[iNum].enabled = false;
                    stripeObject.GetComponent<RectTransform>().SetParent(transform.parent); // Not in the grid!
                    stripeObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    Strech(stripeObject, _grid[i[0].x, i[0].y].transform.position, _grid[i[1].x, i[1].y].transform.position, false);
                    stripeObject.SetActive(true);
                    iNum++;
                }
            }
        }
    }

    private Vector2Int[][] checkStripePosition(bool circle) // { { start, end }, ..., null }
    {
        Vector2Int[][] returnArray = new Vector2Int[][] { null, null, null, null, null, null, null, null };
        bool[,] field;
        int stripeNum = 0; 
        if (circle) field = circleField;
        else field = crossField;
        if (field == null) return returnArray;

        // Check horisontal
        for (int i = 0; i < 3; i++)
        {
            if (field[0, i] && field[1, i] && field[2, i])
            {
                returnArray[stripeNum] = new Vector2Int[] { new Vector2Int(0, i), new Vector2Int(2, i) };
                stripeNum++;
            }
        }
        // Check vertical
        for (int i = 0; i < 3; i++)
        {
            if (field[i, 0] && field[i, 1] && field[i, 2])
            {
                returnArray[stripeNum] = new Vector2Int[] { new Vector2Int(i, 0), new Vector2Int(i, 2) };
                stripeNum++;
            }
        }
        // Check diagonal
        if (field[0, 0] && field[1, 1] && field[2, 2])
        {
            returnArray[stripeNum] = new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(2, 2) };
            stripeNum++;
        }
        if (field[2, 0] && field[1, 1] && field[0, 2])
        {
            returnArray[stripeNum] = new Vector2Int[] { new Vector2Int(2, 0), new Vector2Int(0, 2) };
            stripeNum++;
        }

        return returnArray;
    }

    private void Strech(GameObject _sprite, Vector3 _initialPosition, Vector3 _finalPosition, bool _mirrorZ)
    {
        //float width = _sprite.GetComponent<Image>().sprite.bounds.size.x;
        float width = _sprite.GetComponent<RectTransform>().rect.width;
        Vector3 centerPos = (_initialPosition + _finalPosition) / 2f;
        _sprite.transform.position = centerPos;
        Vector3 direction = _finalPosition - _initialPosition;
        direction = Vector3.Normalize(direction);
        _sprite.transform.right = direction;
        if (_mirrorZ) _sprite.transform.right *= -1f;
        Vector3 scale = new Vector3(1, 1, 1);
        scale.x = Vector3.Distance(_initialPosition, _finalPosition) / width * 1.5f; // A little bit wider
        _sprite.transform.localScale = scale;
    }

    public void Update()
    {
        if (isAnimating)
        {
            if (Time.realtimeSinceStartup - prAnimationStepTime > 0.045f)
            {
                bool atLeastOneStripe = false;
                int iNum = 0;
                foreach (Image i in stripeImage)
                {
                    if (i != null)
                    {
                        if (i.sprite == null) { if (stripeSprites.Length >= 1 && stripeSprites[0] != null) { i.sprite = stripeSprites[0]; i.enabled = true; } }
                        else if (i.sprite == stripeSprites[0]) { if (stripeSprites.Length >= 2 && stripeSprites[1] != null) i.sprite = stripeSprites[1]; }
                        else if (i.sprite == stripeSprites[1]) { if (stripeSprites.Length >= 3 && stripeSprites[2] != null) i.sprite = stripeSprites[2]; }
                        else if (i.sprite == stripeSprites[2]) { if (stripeSprites.Length >= 4 && stripeSprites[3] != null) i.sprite = stripeSprites[3]; }
                        else stripeImage[iNum] = null; // Remove from animating list
                        atLeastOneStripe = true;
                        break; // Draw one by one
                    }
                    iNum++;
                }
                if (!atLeastOneStripe) isAnimating = false;

                prAnimationStepTime = Time.realtimeSinceStartup;
            }
        }
    }
}
