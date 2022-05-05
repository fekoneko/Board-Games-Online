using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Sprite[] circle1Sprites; // Only 4 elements!
    [SerializeField] private Sprite[] circle2Sprites;
    [SerializeField] private Sprite[] circle3Sprites;
    [SerializeField] private Sprite[] cross1Sprites;
    [SerializeField] private Sprite[] cross2Sprites;
    [SerializeField] private Sprite[] cross3Sprites;

    private Image cellFigure;
    public System.Action<Cell> onCellPressed;
    public bool isCircle = true;
    public bool active = true;
    public bool isMyTurn = false;
    public int x = 0;
    public int y = 0;

    private bool isAnimating = false;
    private float prAnimationStepTime = 0.0f;

    public void Start()
    {
        GameObject cellFigureObject = new GameObject();
        cellFigure = cellFigureObject.AddComponent<Image>();
        cellFigure.sprite = null;
        cellFigure.enabled = false;
        cellFigureObject.GetComponent<RectTransform>().SetParent(this.transform);
        cellFigureObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        cellFigureObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        cellFigure.rectTransform.sizeDelta = new Vector2(100, 100);
        cellFigureObject.SetActive(true);
    }

    public void SetId(bool id)
    {
        isMyTurn = id;
        //UpdateBackgroundSprite(); // When is correct
    }

    public void OnCellPressed()
    {
        onCellPressed?.Invoke(this);
    }

    public bool UpdateBackgroundSprite(bool isMy) // Returns true if circle was placed
    {
        int randomNum = Random.Range(0, 3);

        isAnimating = true;
        prAnimationStepTime = Time.realtimeSinceStartup;
        if ((isMy && isCircle) || (!isMy && !isCircle))
        {
            switch (randomNum)
            {
                case 0:
                    if (circle1Sprites.Length >= 1 && circle1Sprites[0] != null) { cellFigure.enabled = true; cellFigure.sprite = circle1Sprites[0]; }
                    break;
                case 1:
                    if (circle2Sprites.Length >= 1 && circle2Sprites[0] != null) { cellFigure.enabled = true; cellFigure.sprite = circle2Sprites[0]; }
                    break;
                case 2:
                    if (circle3Sprites.Length >= 1 && circle3Sprites[0] != null) { cellFigure.enabled = true; cellFigure.sprite = circle3Sprites[0]; }
                    break;
            }
        }
        else
        {
            switch (randomNum)
            {
                case 0:
                    if (cross1Sprites.Length >= 1 && cross1Sprites[0] != null) { cellFigure.enabled = true; cellFigure.sprite = cross1Sprites[0]; }
                    break;
                case 1:
                    if (cross2Sprites.Length >= 1 && cross2Sprites[0] != null) { cellFigure.enabled = true; cellFigure.sprite = cross2Sprites[0]; }
                    break;
                case 2:
                    if (cross3Sprites.Length >= 1 && cross3Sprites[0] != null) { cellFigure.enabled = true; cellFigure.sprite = cross3Sprites[0]; }
                    break;
            }
        }

        GetComponent<Button>().interactable = false;
        active = false;

        if (isMy) return isCircle;
        else return !isCircle;
    }

    public void Update()
    {
        if (isAnimating)
        {
            if (Time.realtimeSinceStartup - prAnimationStepTime > 0.06f)
            {
                if (cellFigure.sprite == circle1Sprites[0]) { if (circle1Sprites.Length >= 2 && circle1Sprites[1] != null) cellFigure.sprite = circle1Sprites[1]; }
                else if (cellFigure.sprite == circle1Sprites[1]) { if (circle1Sprites.Length >= 3 && circle1Sprites[2] != null) cellFigure.sprite = circle1Sprites[2]; }
                else if (cellFigure.sprite == circle1Sprites[2]) { if (circle1Sprites.Length >= 4 && circle1Sprites[3] != null) cellFigure.sprite = circle1Sprites[3]; }

                else if (cellFigure.sprite == circle2Sprites[0]) { if (circle1Sprites.Length >= 2 && circle2Sprites[1] != null) cellFigure.sprite = circle2Sprites[1]; }
                else if (cellFigure.sprite == circle2Sprites[1]) { if (circle1Sprites.Length >= 3 && circle2Sprites[2] != null) cellFigure.sprite = circle2Sprites[2]; }
                else if (cellFigure.sprite == circle2Sprites[2]) { if (circle1Sprites.Length >= 4 && circle2Sprites[3] != null) cellFigure.sprite = circle2Sprites[3]; }

                else if (cellFigure.sprite == circle3Sprites[0]) { if (circle1Sprites.Length >= 2 && circle3Sprites[1] != null) cellFigure.sprite = circle3Sprites[1]; }
                else if (cellFigure.sprite == circle3Sprites[1]) { if (circle1Sprites.Length >= 3 && circle3Sprites[2] != null) cellFigure.sprite = circle3Sprites[2]; }
                else if (cellFigure.sprite == circle3Sprites[2]) { if (circle1Sprites.Length >= 4 && circle3Sprites[3] != null) cellFigure.sprite = circle3Sprites[3]; }


                else if (cellFigure.sprite == cross1Sprites[0]) { if (cross1Sprites.Length >= 2 && cross1Sprites[1] != null) cellFigure.sprite = cross1Sprites[1]; }
                else if (cellFigure.sprite == cross1Sprites[1]) { if (cross1Sprites.Length >= 3 && cross1Sprites[2] != null) cellFigure.sprite = cross1Sprites[2]; }
                else if (cellFigure.sprite == cross1Sprites[2]) { if (cross1Sprites.Length >= 4 && cross1Sprites[3] != null) cellFigure.sprite = cross1Sprites[3]; }

                else if (cellFigure.sprite == cross2Sprites[0]) { if (cross1Sprites.Length >= 2 && cross2Sprites[1] != null) cellFigure.sprite = cross2Sprites[1]; }
                else if (cellFigure.sprite == cross2Sprites[1]) { if (cross1Sprites.Length >= 3 && cross2Sprites[2] != null) cellFigure.sprite = cross2Sprites[2]; }
                else if (cellFigure.sprite == cross2Sprites[2]) { if (cross1Sprites.Length >= 4 && cross2Sprites[3] != null) cellFigure.sprite = cross2Sprites[3]; }

                else if (cellFigure.sprite == cross3Sprites[0]) { if (cross1Sprites.Length >= 2 && cross3Sprites[1] != null) cellFigure.sprite = cross3Sprites[1]; }
                else if (cellFigure.sprite == cross3Sprites[1]) { if (cross1Sprites.Length >= 3 && cross3Sprites[2] != null) cellFigure.sprite = cross3Sprites[2]; }
                else if (cellFigure.sprite == cross3Sprites[2]) { if (cross1Sprites.Length >= 4 && cross3Sprites[3] != null) cellFigure.sprite = cross3Sprites[3]; }


                else isAnimating = false;

                prAnimationStepTime = Time.realtimeSinceStartup;
            }
        }
    }
}