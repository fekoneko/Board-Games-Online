using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public System.Action<Cell> onCellPressed;
    public bool isCircle = true;
    public bool active = true;
    public bool isMyTurn = false;
    public int x = 0;
    public int y = 0;

    public void SetId(bool id)
    {
        isMyTurn = id;
        //UpdateBackgroundSprite(); // When is correct
    }

    public void OnCellPressed()
    {
        onCellPressed?.Invoke(this);
    }

    [SerializeField] private Sprite circleSprite;
    [SerializeField] private Sprite crossSprite;
    [SerializeField] private Image cellBackground;
    public void UpdateBackgroundSprite()
    {
        cellBackground.sprite = isCircle ? circleSprite : crossSprite;
        GetComponent<Button>().interactable = false;
        active = false;
    }
}