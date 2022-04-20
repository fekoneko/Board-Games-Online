using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipDragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject shipManagerObject;
    [SerializeField] private GameObject readyButtonObject;
    [SerializeField] private GameObject[] shipPartList;
    [SerializeField] public int size;
    [SerializeField] public bool horizontally;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private GameObject[] ships;

    private bool disabled = false;
    private bool pinned = false;
    private bool dragged = false;
    private bool moving = false;
    public Vector2 basePosition;
    public Vector2 pinPosition;
    private ShipSlot pinShipSlot;
    public float rotationAngle = 0;
    public float rotationAngleCur = 0;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Start()
    {
        basePosition = rectTransform.position;
        ships = shipManagerObject.GetComponent<ShipManager>().ships;
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
        if (horizontally)
            rotationAngle = 0.01f * eventData.delta.y / (canvas.scaleFactor * Time.deltaTime);
        else
            rotationAngle = 0.01f * eventData.delta.x / (canvas.scaleFactor * Time.deltaTime) + 90;
        transform.SetSiblingIndex(-1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (disabled) return;
        //Debug.Log("OnBeginDrag");
        canvasGroup.blocksRaycasts = false;
        dragged = true;
        if (pinned)
        {
            Unpin();
        }
        rectTransform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);


        if (shipManagerObject.GetComponent<ShipManager>().rotateShips)
        {
            horizontally = false;
            rotationAngle = 90.0f;
        }
        else
        {
            horizontally = true;
            rotationAngle = 0.0f;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (disabled) return;
        //Debug.Log("OnEndDrag")
        canvasGroup.blocksRaycasts = true;
        dragged = false;
        transform.SetSiblingIndex(0);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (disabled) return;
        //Debug.Log("OnDrop");
    }

    public ShipSlot GetPinShipSlot()
    {
        return pinShipSlot;
    }

    public void Pin(Vector2 pinPos, ShipSlot itemSlot)
    {
        pinPosition = pinPos;
        pinShipSlot = itemSlot;
        pinned = true;
        SetSlotsState(false);

        foreach (GameObject i in shipPartList)
        {
            i.GetComponent<ShipPartManager>().ShowEquipment();
        }

        // Make ReadyButton active (if needed)
        ReadyButton readyButton = readyButtonObject.GetComponent<ReadyButton>();
        if (readyButton.checkShips())
        {
            readyButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else
        {
            readyButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
    }

    public void Unpin()
    {
        pinned = false;
        SetSlotsState(true);

        foreach (GameObject i in shipPartList)
        {
            i.GetComponent<ShipPartManager>().HideEquipment();
        }
    }

    public void SetSlotsState(bool en)
    {
        if (pinShipSlot == null) return;
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

    public void SetState(bool en) { disabled = !en; }

    public bool CheckEnabled() { return !disabled; }

    public bool CheckPinned() { return pinned; }

    public bool isMoving() { return moving; }

    public void Update()
    {
        if (!disabled)
        {
            if (!dragged)
            {
                if (pinned) // Jump back in slot
                {
                    if (rectTransform.anchoredPosition != pinPosition)
                    {
                        rectTransform.position = Vector2.MoveTowards(rectTransform.position, pinPosition, 10000 * Time.deltaTime);
                    }
                }
                else // Jump back to base
                {
                    if (rectTransform.anchoredPosition != basePosition)
                    {
                        rectTransform.position = Vector2.MoveTowards(rectTransform.position, basePosition, 10000 * Time.deltaTime);
                        moving = true;
                    }
                }
            }

            if (rotationAngleCur != rotationAngle)
            {
                float step;
                if (!dragged || Mathf.Abs(rotationAngle - rotationAngleCur) > 20)
                    step = 500f * Time.deltaTime;
                else
                    step = 65f * Time.deltaTime;
                if (rotationAngle - rotationAngleCur > step)
                {
                    rotationAngleCur += step;
                }
                else if (rotationAngleCur - rotationAngle > step)
                {
                    rotationAngleCur -= step;
                }
                else
                {
                    rotationAngleCur = rotationAngle;
                }
                rectTransform.rotation = Quaternion.AngleAxis(rotationAngleCur, Vector3.back);

                if (horizontally)
                    rotationAngle = 0.0f;
                else
                    rotationAngle = 90.0f;
            }

            if (!dragged && !pinned)
            {
                horizontally = true;
                rotationAngle = 0.0f;
            }
        }
    }
}
