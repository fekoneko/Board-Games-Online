using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipPartManager : MonoBehaviour
{
    [SerializeField] private Sprite[] equipmentList;

    private GameObject shipPartEquipment;
    private Image shipPartEquipmentImage;

    public void Start()
    {
        shipPartEquipment = new GameObject();
        shipPartEquipmentImage = shipPartEquipment.AddComponent<Image>();
        shipPartEquipmentImage.sprite = null;
        shipPartEquipmentImage.enabled = false;
        shipPartEquipment.GetComponent<RectTransform>().SetParent(this.transform);
        shipPartEquipment.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        shipPartEquipment.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        shipPartEquipmentImage.rectTransform.sizeDelta = new Vector2(90, 90);
        shipPartEquipment.GetComponent<RectTransform>().Rotate(Vector3.back, -90f);
        shipPartEquipment.SetActive(true);
    }

    public void ShowEquipment()
    {
        int randomNum = UnityEngine.Random.Range(0, equipmentList.Length);
        shipPartEquipmentImage.sprite = equipmentList[randomNum];
        shipPartEquipmentImage.enabled = true;
    }
    
    public void HideEquipment()
    {
        shipPartEquipmentImage.sprite = null;
        shipPartEquipmentImage.enabled = false;
    }
}
