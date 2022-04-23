using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShipSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] public Vector2 cell;
    [SerializeField] private GameObject slotManagerObject;
    [SerializeField] private Sprite missedCellSprite;
    [SerializeField] private Sprite damagedCellSprite;
    [SerializeField] private Sprite paintedCellSprite;
    [SerializeField] private Sprite[] shootSpriteList;

    public GameObject[][] cells;
    private RectTransform rectTransform;
    private Image image;

    private bool disabled = false;
    private bool availableShow = true;

    private float shootImageTime = -1.0f;
    private float shootImageTimePr = -0.1f;
    private int shootImageStep = 0;
    public bool damaged = false;
    public bool missed = false;
    public bool painted = false;

    private GameObject shootObject;
    private GameObject cellObject;
    private Image shootImage;
    private Image cellImage;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void Start()
    {
        cells = slotManagerObject.GetComponent<SlotManager>().cells; // Must be later than Awake of SlotManager

        shootObject = new GameObject();
        shootImage = shootObject.AddComponent<Image>();
        shootImage.sprite = null;
        shootImage.enabled = false;
        shootObject.GetComponent<RectTransform>().SetParent(this.transform);
        shootObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        shootObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        shootImage.rectTransform.sizeDelta = new Vector2(90, 90);
        shootObject.SetActive(true);

        cellObject = new GameObject();
        cellImage = cellObject.AddComponent<Image>();
        cellImage.sprite = null;
        cellImage.enabled = false;
        cellObject.GetComponent<RectTransform>().SetParent(this.transform);
        cellObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        cellObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        cellImage.rectTransform.sizeDelta = new Vector2(90, 90);
        cellObject.SetActive(true);
    }

    public void Update()
    {
        UpdateShootSprite();
    }

    public void MakeDamaged()
    {
        ShowShootSprite();
        cellImage.enabled = true;
        cellImage.sprite = damagedCellSprite;
        damaged = true;
    }

    public void MakeMissed()
    {
        ShowShootSprite();
        cellImage.enabled = true;
        cellImage.sprite = missedCellSprite;
        missed = true;
    }

    public void MakePainted()
    {
        ShowShootSprite();
        cellImage.enabled = true;
        cellImage.sprite = paintedCellSprite;
        painted = true;
    }

    private void ShowShootSprite()
    {
        shootImageTime = 0.0f; // Animate
        shootImageTimePr = -0.1f;
        shootImageStep = 0;

        shootImage.enabled = true;
        shootImage.sprite = shootSpriteList[0];
    }

    private void UpdateShootSprite()
    {
        if (shootImageTime > -1.0f)
        {
            if (shootImageTime < shootSpriteList.Length * 0.1)
            {
                if (shootImageTime - shootImageTimePr > 0.1 && shootImageStep < shootSpriteList.Length)
                {
                    shootImage.sprite = shootSpriteList[shootImageStep];
                    shootImageStep++;
                    shootImageTimePr += 0.1f;
                }
                shootImageTime += Time.deltaTime;
            }
            else
            {
                shootImageTime = -1.0f;
                shootImageTimePr = -0.1f;
                shootImageStep = 0;

                shootImage.sprite = null;
                shootImage.enabled = false;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (disabled) return;
        //Debug.Log("OnDrop (ItemSlot)");
        if (eventData.pointerDrag != null)
        {
            if (CheckCanBeDropped(eventData.pointerDrag))
            {
                eventData.pointerDrag.GetComponent<ShipDragDrop>().Pin(rectTransform.position, this);
            }
        }
    }

    public bool CheckCanBeDropped(GameObject shipObject)
    {
        if (disabled) return false;
        bool canBeDropped = true;
        ShipDragDrop itemDragDrop = shipObject.GetComponent<ShipDragDrop>();
        switch (itemDragDrop.size)
        {
            case 1:
                // [p]
                //canBeDropped = true;
                break;
            case 2:
                // [p][ ]
                //
                // [p]
                // [ ]
                if (itemDragDrop.horizontally)
                {
                    if ((int)cell.x + 1 < 10)
                    {
                        canBeDropped = canBeDropped && !cells[(int)cell.y][(int)cell.x + 1].GetComponent<ShipSlot>().disabled;
                    }
                    else
                    { canBeDropped = false; break; }
                }
                else
                {
                    if ((int)cell.y + 1 < 10)
                    {
                        canBeDropped = canBeDropped && !cells[(int)cell.y + 1][(int)cell.x].GetComponent<ShipSlot>().disabled;
                    }
                    else
                    { canBeDropped = false; break; }
                }
                break;
            case 3:
                // [ ][p][ ]
                //
                // [ ]
                // [p]
                // [ ]
                if (itemDragDrop.horizontally)
                {
                    if ((int)cell.x + 1 < 10 && (int)cell.x - 1 > -1)
                    {
                        canBeDropped = canBeDropped && !cells[(int)cell.y][(int)cell.x + 1].GetComponent<ShipSlot>().disabled;
                        canBeDropped = canBeDropped && !cells[(int)cell.y][(int)cell.x - 1].GetComponent<ShipSlot>().disabled;
                    }
                    else
                    { canBeDropped = false; break; }
                }
                else
                {
                    if ((int)cell.y + 1 < 10 && (int)cell.y - 1 > -1)
                    {
                        canBeDropped = canBeDropped && !cells[(int)cell.y + 1][(int)cell.x].GetComponent<ShipSlot>().disabled;
                        canBeDropped = canBeDropped && !cells[(int)cell.y - 1][(int)cell.x].GetComponent<ShipSlot>().disabled;
                    }
                    else
                    { canBeDropped = false; break; }
                }
                break;
            case 4:
                // [ ][p][ ][ ]
                //
                // [ ]
                // [p]
                // [ ]
                // [ ]
                if (itemDragDrop.horizontally)
                {
                    if ((int)cell.x + 2 < 10 && (int)cell.x - 1 > -1)
                    {
                        canBeDropped = canBeDropped && !cells[(int)cell.y][(int)cell.x + 1].GetComponent<ShipSlot>().disabled;
                        canBeDropped = canBeDropped && !cells[(int)cell.y][(int)cell.x + 2].GetComponent<ShipSlot>().disabled;
                        canBeDropped = canBeDropped && !cells[(int)cell.y][(int)cell.x - 1].GetComponent<ShipSlot>().disabled;
                    }
                    else
                    { canBeDropped = false; break; }
                }
                else
                {
                    if ((int)cell.y + 2 < 10 && (int)cell.y - 1 > -1)
                    {
                        canBeDropped = canBeDropped && !cells[(int)cell.y + 1][(int)cell.x].GetComponent<ShipSlot>().disabled;
                        canBeDropped = canBeDropped && !cells[(int)cell.y + 2][(int)cell.x].GetComponent<ShipSlot>().disabled;
                        canBeDropped = canBeDropped && !cells[(int)cell.y - 1][(int)cell.x].GetComponent<ShipSlot>().disabled;
                    }
                    else
                    { canBeDropped = false; break; }
                }
                break;
            default:
                canBeDropped = false;
                break;
        }
        return canBeDropped;
    }

    public void SetState(bool en)
    {
        disabled = !en;
        if (en || !availableShow)
        {
            image.color = Color.white;
        }
        else
        {
            image.color = new Color(0.96f, 0.96f, 1.0f);
        }
    }

    public bool CheckEnabled()
    {
        return !disabled;
    }

    public void SetAvailableShow(bool en)
    {
        availableShow = en;
        SetState(!disabled); // Update color
    }
}
