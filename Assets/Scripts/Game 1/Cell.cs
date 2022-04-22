using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public System.Action<Cell> onCellPressed;
    public int playerId { get; private set; } = -1;

    public void SetId(int id)
    {
        playerId = id;
        UpdateBackgroundColor();
    }

    public void OnCellPressed()
    {
        onCellPressed?.Invoke(this);
    }

    [SerializeField] private Sprite _teamAColor;
    [SerializeField] private Sprite _teamBColor;
    [SerializeField] private Image _cellBackground;
    private void UpdateBackgroundColor()
    {
        _cellBackground.sprite = playerId == 0 ? _teamAColor : _teamBColor;
    }
}