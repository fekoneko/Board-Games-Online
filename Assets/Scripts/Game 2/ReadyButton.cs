using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButton : MonoBehaviour
{
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject shipManagerObject;
    [SerializeField] private GameObject slotManagerObject;
    [SerializeField] private GameObject serverControllerObject;
    [SerializeField] private GameObject rotateButtonObject;
    [SerializeField] private GameObject randomButtonObject;
    [SerializeField] private GameObject slotsObject;
    [SerializeField] private GameObject opponentSlotsObject;
    [SerializeField] private Sprite[] waitingBannerImageList;
    [SerializeField] private string[] waitingBannerTextList;

    private Button button;
    private Text text;
    private ShipManager shipManager;
    private SlotManager slotManager;
    private ServerController serverController;
    private bool waiting = false;
    private float waitingTime = 0.0f;
    private float waitingTimePr = -1.0f;
    private int textStep = 0;
    private bool waitingBannerShowed = false;
    private bool waitingBannerStopped = false;
    private bool gameStarted = false;

    private GameObject waitingBannerImageObject;
    private RectTransform waitingBannerImageRectTransform;
    private Image waitingBannerImage;
    private GameObject waitingBannerTextObject;
    private RectTransform waitingBannerTextRectTransform;
    private Text waitingBannerText;

    // Start is called before the first frame update
    public void Start()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        text = textObject.GetComponent<Text>();
        shipManager = shipManagerObject.GetComponent<ShipManager>();
        slotManager = slotManagerObject.GetComponent<SlotManager>();
        serverController = serverControllerObject.GetComponent<ServerController>();


        waitingBannerImageObject = new GameObject();
        waitingBannerImage = waitingBannerImageObject.AddComponent<Image>();
        waitingBannerImage.sprite = null;
        waitingBannerImage.enabled = false;

        waitingBannerImageRectTransform = waitingBannerImageObject.GetComponent<RectTransform>();
        waitingBannerImageRectTransform.SetParent(this.transform);
        waitingBannerImageRectTransform.anchoredPosition = new Vector2(-1500, 400);
        waitingBannerImageRectTransform.localScale = new Vector3(1, 1, 1);

        waitingBannerImage.rectTransform.sizeDelta = new Vector2(500, 500);
        waitingBannerImageObject.SetActive(true);


        waitingBannerTextObject = new GameObject();
        waitingBannerText = waitingBannerTextObject.AddComponent<Text>();
        waitingBannerText.text = "";
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        waitingBannerText.font = ArialFont;
        waitingBannerText.material = ArialFont.material;
        waitingBannerText.fontSize = 50;
        waitingBannerText.fontStyle = FontStyle.Bold;
        waitingBannerText.alignment = TextAnchor.MiddleCenter;
        waitingBannerText.color = new Color(0.396f, 0.396f, 0.459f);
        waitingBannerText.enabled = false;

        waitingBannerTextRectTransform = waitingBannerTextObject.GetComponent<RectTransform>();
        waitingBannerTextRectTransform.SetParent(this.transform);
        waitingBannerTextRectTransform.anchoredPosition = new Vector2(-1500, 155);
        waitingBannerTextRectTransform.localScale = new Vector3(1, 1, 1);

        waitingBannerText.rectTransform.sizeDelta = new Vector2(800, 80);
        waitingBannerTextObject.SetActive(true);
    }

    // Update is called once per frame
    public void Update()
    {
        if (waiting && !gameStarted)
        {
            if (waitingTime - waitingTimePr > 1.0f)
            {
                string textToWrite = "";
                switch (textStep)
                {
                    case 0:
                        textToWrite = "∆дЄм противника.";
                        break;
                    case 1:
                        textToWrite = "∆дЄм противника..";
                        break;
                    case 2:
                        textToWrite = "∆дЄм противника...";
                        break;
                }
                text.text = textToWrite;

                textStep++;
                textStep %= 3;
                waitingTimePr = waitingTime;
            }
            if (waitingTime > 5.0f)
            {
                showWaitingBanner();
                waitingBannerShowed = true;
            }

            waitingTime += Time.deltaTime;
        }
        if (waitingBannerShowed && !waitingBannerStopped)
        {
            float nx = Vector2.MoveTowards(waitingBannerImageRectTransform.anchoredPosition, Vector2.zero, 2000 * Time.deltaTime).x; // X is the same
            waitingBannerImageRectTransform.anchoredPosition = new Vector2(nx, waitingBannerImageRectTransform.anchoredPosition.y);
            waitingBannerTextRectTransform.anchoredPosition = new Vector2(nx, waitingBannerTextRectTransform.anchoredPosition.y);
            if (nx == 0) waitingBannerStopped = true;
        }
        if (gameStarted)
        {
            //float ny = Vector2.MoveTowards(new Vector2(1000, transform.localPosition.y), new Vector2(0, 100), 2000 * Time.deltaTime).y; // Ease
            //transform.localPosition = new Vector2(transform.localPosition.x, ny);
            float ns = Time.deltaTime;
            transform.localScale -= new Vector3(ns, ns, ns);

            RectTransform slotsRectTransform = slotsObject.GetComponent<RectTransform>();
            slotsRectTransform.anchoredPosition = Vector2.MoveTowards(slotsRectTransform.anchoredPosition, new Vector2(slotsRectTransform.anchoredPosition.x, 430), 500 * Time.deltaTime);
            RectTransform shipManagerRectTransform = shipManagerObject.GetComponent<RectTransform>();
            shipManagerRectTransform.anchoredPosition = Vector2.MoveTowards(shipManagerRectTransform.anchoredPosition, new Vector2(shipManagerRectTransform.anchoredPosition.x, -290), 500 * Time.deltaTime);
            RectTransform opponentSlotsRectTransform = opponentSlotsObject.GetComponent<RectTransform>();
            opponentSlotsRectTransform.anchoredPosition = Vector2.MoveTowards(opponentSlotsRectTransform.anchoredPosition, new Vector2(opponentSlotsRectTransform.anchoredPosition.x, -430), 2000 * Time.deltaTime);

            if (opponentSlotsRectTransform.anchoredPosition.y == -430) // Last action
            {
                if (waitingBannerShowed)
                {
                    Destroy(waitingBannerImageObject);
                    Destroy(waitingBannerTextObject);
                }
                Destroy(this);
            }
        }
    }

    private void TaskOnClick()
    {
        if (checkShips())
        {
            shipManager.SetShipDragging(false);
            slotManager.SetAvailableShow(false);

            //serverController.ServerSendReady();

            Destroy(rotateButtonObject);
            Destroy(randomButtonObject);

            button.interactable = false;
            waiting = true;
        }
    }

    private bool checkShips()
    {
        bool allArePinned = true;
        foreach(GameObject i in shipManager.ships)
        {
            allArePinned = allArePinned && i.GetComponent<ShipDragDrop>().CheckPinned();
            if (!allArePinned) break;
        }
        return allArePinned;
    }

    private void showWaitingBanner()
    {
        int randomNum = UnityEngine.Random.Range(0, waitingBannerImageList.Length);
        waitingBannerImage.sprite = waitingBannerImageList[randomNum];
        waitingBannerText.text = waitingBannerTextList[randomNum];
        waitingBannerImage.enabled = true;
        waitingBannerText.enabled = true;
    }

    public void startGame()
    {
        gameStarted = true; // Moving up is in Update()
    }
}
