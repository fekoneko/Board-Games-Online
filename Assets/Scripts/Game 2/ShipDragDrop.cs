using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipDragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject shipManagerObject;
    [SerializeField] public int size;
    [SerializeField] public bool horizontally;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private GameObject[] ships;

    private bool disabled = false;
    private bool pinned = false;
    private bool dragged = false;
    private Vector2 basePosition;
    private Vector2 pinPosition;
    private ShipSlot pinShipSlot;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); 
        canvasGroup = GetComponent<CanvasGroup>();
        ships = shipManagerObject.GetComponent<ShipManager>().ships;
    }

    public void Start()
    {
        basePosition = rectTransform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (disabled) return;
        //Debug.Log("OnPointerDown");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (disabled) return;
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (disabled) return;
        //Debug.Log("OnBeginDrag");
        canvasGroup.blocksRaycasts = false;
        dragged = true;
        if (pinned)
        {
            pinned = false;
            SetSlotsState(true);
        }
        rectTransform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (disabled) return;
        //Debug.Log("OnEndDrag")
        canvasGroup.blocksRaycasts = true;
        dragged = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (disabled) return;
        //Debug.Log("OnDrop");
    }

    public void Pin(Vector2 pinPos, ShipSlot itemSlot)
    {
        pinPosition = pinPos;
        pinShipSlot = itemSlot;
        pinned = true;
        SetSlotsState(false);
    }

    public void SetSlotsState(bool en)
    {
        int px = (int)pinShipSlot.cell.x;
        int py = (int)pinShipSlot.cell.y;
        GameObject[][] cells = pinShipSlot.cells;

        pinShipSlot.SetState(en);
        switch (size)
        {
            case 1:
                // [p]
                if (px - 1 > -1)
                    cells[py][px - 1].GetComponent<ShipSlot>().SetState(en);
                if (px + 1 < 10)
                    cells[py][px + 1].GetComponent<ShipSlot>().SetState(en);
                if (py + 1 < 10)
                {
                    cells[py + 1][px].GetComponent<ShipSlot>().SetState(en);
                    if (px - 1 > -1)
                        cells[py + 1][px - 1].GetComponent<ShipSlot>().SetState(en);
                    if (px + 1 < 10)
                        cells[py + 1][px + 1].GetComponent<ShipSlot>().SetState(en);
                }
                if (py - 1 > -1)
                {
                    cells[py - 1][px].GetComponent<ShipSlot>().SetState(en);
                    if (px - 1 > -1)
                        cells[py - 1][px - 1].GetComponent<ShipSlot>().SetState(en);
                    if (px + 1 < 10)
                        cells[py - 1][px + 1].GetComponent<ShipSlot>().SetState(en);
                }
                break;
            case 2:
                //  *  *  *  *
                //  * [p][ ] *
                //  *  *  *  *
                //
                //  *  *  *
                //  * [p] *
                //  * [ ] *
                //  *  *  *
                if (horizontally)
                {
                    if (px - 1 > -1)
                        cells[py][px - 1].GetComponent<ShipSlot>().SetState(en);
                    if (px + 1 < 10)
                        cells[py][px + 1].GetComponent<ShipSlot>().SetState(en);
                    if (px + 2 < 10)
                        cells[py][px + 2].GetComponent<ShipSlot>().SetState(en);
                    if (py + 1 < 10)
                    {
                        cells[py + 1][px].GetComponent<ShipSlot>().SetState(en);
                        if (px - 1 > -1)
                            cells[py + 1][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (px + 1 < 10)
                            cells[py + 1][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (px + 2 < 10)
                            cells[py + 1][px + 2].GetComponent<ShipSlot>().SetState(en);
                    }
                    if (py - 1 > -1)
                    {
                        cells[py - 1][px].GetComponent<ShipSlot>().SetState(en);
                        if (px - 1 > -1)
                            cells[py - 1][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (px + 1 < 10)
                            cells[py - 1][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (px + 2 < 10)
                            cells[py - 1][px + 2].GetComponent<ShipSlot>().SetState(en);
                    }
                }
                else
                {
                    if (py - 1 > -1)
                        cells[py - 1][px].GetComponent<ShipSlot>().SetState(en);
                    if (py + 1 < 10)
                        cells[py + 1][px].GetComponent<ShipSlot>().SetState(en);
                    if (py + 2 < 10)
                        cells[py + 2][px].GetComponent<ShipSlot>().SetState(en);
                    if (px + 1 < 10)
                    {
                        cells[py][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (py - 1 > -1)
                            cells[py - 1][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 1 < 10)
                            cells[py + 1][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 2 < 10)
                            cells[py + 2][px + 1].GetComponent<ShipSlot>().SetState(en);
                    }
                    if (px - 1 > -1)
                    {
                        cells[py][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (py - 1 > -1)
                            cells[py - 1][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 1 < 10)
                            cells[py + 1][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 2 < 10)
                            cells[py + 2][px - 1].GetComponent<ShipSlot>().SetState(en);
                    }
                }
                break;
            case 3:
                //  *  *  *  *  *
                //  * [ ][p][ ] *
                //  *  *  *  *  *
                //
                //  *  *  *
                //  * [ ] *
                //  * [p] *
                //  * [ ] *
                //  *  *  *
                if (horizontally)
                {
                    if (px - 1 > -1)
                        cells[py][px - 1].GetComponent<ShipSlot>().SetState(en);
                    if (px - 2 > -1)
                        cells[py][px - 2].GetComponent<ShipSlot>().SetState(en);
                    if (px + 1 < 10)
                        cells[py][px + 1].GetComponent<ShipSlot>().SetState(en);
                    if (px + 2 < 10)
                        cells[py][px + 2].GetComponent<ShipSlot>().SetState(en);
                    if (py + 1 < 10)
                    {
                        cells[py + 1][px].GetComponent<ShipSlot>().SetState(en);
                        if (px - 1 > -1)
                            cells[py + 1][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (px - 2 > -1)
                            cells[py + 1][px - 2].GetComponent<ShipSlot>().SetState(en);
                        if (px + 1 < 10)
                            cells[py + 1][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (px + 2 < 10)
                            cells[py + 1][px + 2].GetComponent<ShipSlot>().SetState(en);
                    }
                    if (py - 1 > -1)
                    {
                        cells[py - 1][px].GetComponent<ShipSlot>().SetState(en);
                        if (px - 1 > -1)
                            cells[py - 1][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (px - 2 > -1)
                            cells[py - 1][px - 2].GetComponent<ShipSlot>().SetState(en);
                        if (px + 1 < 10)
                            cells[py - 1][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (px + 2 < 10)
                            cells[py - 1][px + 2].GetComponent<ShipSlot>().SetState(en);
                    }
                }
                else
                {
                    if (py - 1 > -1)
                        cells[py - 1][px].GetComponent<ShipSlot>().SetState(en);
                    if (py - 2 > -1)
                        cells[py - 2][px].GetComponent<ShipSlot>().SetState(en);
                    if (py + 1 < 10)
                        cells[py + 1][px].GetComponent<ShipSlot>().SetState(en);
                    if (py + 2 < 10)
                        cells[py + 2][px].GetComponent<ShipSlot>().SetState(en);
                    if (px + 1 < 10)
                    {
                        cells[py][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 1 > -1)
                            cells[py - 1][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (py - 2 > -1)
                            cells[py - 2][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 1 < 10)
                            cells[py + 1][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 2 < 10)
                            cells[py + 2][px + 1].GetComponent<ShipSlot>().SetState(en);
                    }
                    if (px - 1 > -1)
                    {
                        cells[py][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (py - 1 > -1)
                            cells[py - 1][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (py - 2 > -1)
                            cells[py - 2][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 1 < 10)
                            cells[py + 1][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 2 < 10)
                            cells[py + 2][px - 1].GetComponent<ShipSlot>().SetState(en);
                    }
                }
                break;
            case 4:
                //  *  *  *  *  *  *
                //  * [ ][p][ ][ ] *
                //  *  *  *  *  *  *
                //
                //  *  *  *
                //  * [ ] *
                //  * [p] *
                //  * [ ] *
                //  * [ ] *
                //  *  *  *
                if (horizontally)
                {
                    if (px - 1 > -1)
                        cells[py][px - 1].GetComponent<ShipSlot>().SetState(en);
                    if (px - 2 > -1)
                        cells[py][px - 2].GetComponent<ShipSlot>().SetState(en);
                    if (px + 1 < 10)
                        cells[py][px + 1].GetComponent<ShipSlot>().SetState(en);
                    if (px + 2 < 10)
                        cells[py][px + 2].GetComponent<ShipSlot>().SetState(en);
                    if (px + 3 < 10)
                        cells[py][px + 3].GetComponent<ShipSlot>().SetState(en);
                    if (py + 1 < 10)
                    {
                        cells[py + 1][px].GetComponent<ShipSlot>().SetState(en);
                        if (px - 1 > -1)
                            cells[py + 1][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (px - 2 > -1)
                            cells[py + 1][px - 2].GetComponent<ShipSlot>().SetState(en);
                        if (px + 1 < 10)
                            cells[py + 1][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (px + 2 < 10)
                            cells[py + 1][px + 2].GetComponent<ShipSlot>().SetState(en);
                        if (px + 3 < 10)
                            cells[py + 1][px + 3].GetComponent<ShipSlot>().SetState(en);
                    }
                    if (py - 1 > -1)
                    {
                        cells[py - 1][px].GetComponent<ShipSlot>().SetState(en);
                        if (px - 1 > -1)
                            cells[py - 1][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (px - 2 > -1)
                            cells[py - 1][px - 2].GetComponent<ShipSlot>().SetState(en);
                        if (px + 1 < 10)
                            cells[py - 1][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (px + 2 < 10)
                            cells[py - 1][px + 2].GetComponent<ShipSlot>().SetState(en);
                        if (px + 3 < 10)
                            cells[py - 1][px + 3].GetComponent<ShipSlot>().SetState(en);
                    }
                }
                else
                {
                    if (py - 1 > -1)
                        cells[py - 1][px].GetComponent<ShipSlot>().SetState(en);
                    if (py - 2 > -1)
                        cells[py - 2][px].GetComponent<ShipSlot>().SetState(en);
                    if (py + 1 < 10)
                        cells[py + 1][px].GetComponent<ShipSlot>().SetState(en);
                    if (py + 2 < 10)
                        cells[py + 2][px].GetComponent<ShipSlot>().SetState(en);
                    if (py + 3 < 10)
                        cells[py + 3][px].GetComponent<ShipSlot>().SetState(en);
                    if (px + 1 < 10)
                    {
                        cells[py][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 1 > -1)
                            cells[py - 1][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (py - 2 > -1)
                            cells[py - 2][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 1 < 10)
                            cells[py + 1][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 2 < 10)
                            cells[py + 2][px + 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 3 < 10)
                            cells[py + 3][px + 1].GetComponent<ShipSlot>().SetState(en);
                    }
                    if (px - 1 > -1)
                    {
                        cells[py][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (py - 1 > -1)
                            cells[py - 1][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (py - 2 > -1)
                            cells[py - 2][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 1 < 10)
                            cells[py + 1][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 2 < 10)
                            cells[py + 2][px - 1].GetComponent<ShipSlot>().SetState(en);
                        if (py + 3 < 10)
                            cells[py + 3][px - 1].GetComponent<ShipSlot>().SetState(en);
                    }
                }
                break;
        }
        if (en)
        {
            foreach (GameObject i in ships)
            {
                if (i.GetComponent<ShipDragDrop>().pinned)
                {
                    i.GetComponent<ShipDragDrop>().SetSlotsState(false); // Recalculate disabled slots
                }
            }
        }
    }

    public void Enable() { disabled = false; }

    public void Disable() { disabled = true; }

    public bool CheckEnabled() { return !disabled; }

    public void Update()
    {
        if (!dragged)
        {
            if (pinned) // Jump back in slot
            {
                if (rectTransform.anchoredPosition != pinPosition)
                {
                    rectTransform.position = Vector2.MoveTowards(rectTransform.position, pinPosition, 10000*Time.deltaTime);
                }
            }
            else // Jump back to base
            {
                if (rectTransform.anchoredPosition != basePosition)
                {
                    rectTransform.position = Vector2.MoveTowards(rectTransform.position, basePosition, 10000 * Time.deltaTime);
                }
            }
        }
    }
}
