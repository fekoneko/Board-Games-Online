using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpponentCell : MonoBehaviour
{
    [SerializeField] public Vector2 cell;
    [SerializeField] private GameObject slotManagerObject;
    [SerializeField] private Sprite missedCellSprite;
    [SerializeField] private Sprite damagedCellSprite;
    [SerializeField] private Sprite paintedCellSprite;
    [SerializeField] private Sprite[] shootSpriteList;
    [SerializeField] private GameObject serverControllerObject;

    public GameObject[][] cells;

    private RectTransform rectTransform;
    private Image image;
    private Button button;
    private GameObject cellObject;
    private Image cellImage;
    private GameObject shootObject;
    private Image shootImage;

    private SlotManager slotManager;
    private ServerController serverController;

    private float shootImageTime = -1.0f;
    private float shootImageTimePr = -0.1f;
    private int shootImageStep = 0;
    public bool damaged = false;
    public bool missed = false;
    public bool painted = false;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void Start()
    {
        cells = slotManagerObject.GetComponent<SlotManager>().cells;
        slotManager = slotManagerObject.GetComponent<SlotManager>();
        serverController = serverControllerObject.GetComponent<ServerController>();

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

        button.interactable = true;
    }

    public void Update()
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

    public void MakeDamaged()
    {
        cellImage.enabled = true;
        cellImage.sprite = damagedCellSprite;
        damaged = true;
        button.interactable = false;
    }

    public void MakeMissed()
    {
        cellImage.enabled = true;
        cellImage.sprite = missedCellSprite;
        missed = true;
        button.interactable = false;
    }

    public void MakePainted()
    {
        cellImage.enabled = true;
        cellImage.sprite = paintedCellSprite;
        painted = true;
        button.interactable = false;
    }

    private void TaskOnClick()
    {
        // Shoot
        serverController.ServerSend_Shoot((int)cell.x, (int)cell.y);
        slotManager.lastShootX = (int)cell.y; // Swapped
        slotManager.lastShootY = (int)cell.x;

        shootImageTime = 0.0f; // Animate
        shootImageTimePr = -0.1f;
        shootImageStep = 0;

        shootImage.enabled = true;
        shootImage.sprite = shootSpriteList[0];
    }
}
