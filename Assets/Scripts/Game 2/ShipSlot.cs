using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShipSlot : MonoBehaviour, IDropHandler
{
    public Vector2 cell;

    private RectTransform rectTransform;
    private Image image;

    private bool disabled = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (disabled) return;
        //Debug.Log("OnDrop (ItemSlot)");
        if (eventData.pointerDrag != null)
        {
            ShipDragDrop itemDragDrop = eventData.pointerDrag.GetComponent<ShipDragDrop>();
            itemDragDrop.Pin(rectTransform.anchoredPosition, this);
        }
    }

    public void Enable()
    {
        disabled = false;
        image.color = Color.white;
    }

    public void Disable()
    {
        disabled = true;
        image.color = new Color(0.7f, 0.7f, 0.7f);
    }

    public bool CheckEnabled() { return !disabled; }
}
