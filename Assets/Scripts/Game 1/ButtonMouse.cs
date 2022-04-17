using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMouse : MonoBehaviour
{
    [SerializeField] Sprite sprite_1;
    [SerializeField] Sprite sprite_2;

    private GameObject sign;
    private Image signImage;

    public void Start()
    {
        Button button = this.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        sign = new GameObject();
        signImage = sign.AddComponent<Image>();
        signImage.sprite = null;
        signImage.enabled = false;
        sign.GetComponent<RectTransform>().SetParent(this.transform);
        sign.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        sign.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        signImage.rectTransform.sizeDelta = new Vector2(150, 150);
        sign.SetActive(true);
    }

    public void Update()
    {

    }

    private void TaskOnClick()
    {
        signImage.sprite = sprite_1;
        signImage.sprite = sprite_2;
        signImage.enabled = true;
    }
}
