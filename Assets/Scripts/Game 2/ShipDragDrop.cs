using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipDragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private bool disabled = false;
    private bool pinned = false;
    private bool dragged = false;
    private Vector2 basePosition;
    private Vector2 pinPosition;
    private ShipSlot pinItemSlot;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); 
        canvasGroup = GetComponent<CanvasGroup>();
        basePosition = rectTransform.anchoredPosition;
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
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (disabled) return;
        //Debug.Log("OnBeginDrag");
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        dragged = true;
        if (pinned)
        {
            pinned = false;
            pinItemSlot.Enable();
        }
        rectTransform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (disabled) return;
        //Debug.Log("OnEndDrag")
        canvasGroup.alpha = 1.0f;
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
        pinItemSlot = itemSlot;
        pinned = true;
        pinItemSlot.Disable();
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
                    rectTransform.position = Vector2.MoveTowards(rectTransform.anchoredPosition, pinPosition, 10);
                }
            }
            else // Jump back to base
            {
                if (rectTransform.anchoredPosition != basePosition)
                {
                    rectTransform.position = Vector2.MoveTowards(rectTransform.anchoredPosition, basePosition, 10);
                }
            }
        }
    }
}
