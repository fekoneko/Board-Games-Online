using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RotateButton : MonoBehaviour
{
    [SerializeField] private GameObject shipManagerObject;

    private ShipManager shipManager;
    private Image image;

    // Start is called before the first frame update
    public void Start()
    {
        shipManager = shipManagerObject.GetComponent<ShipManager>();
        image = GetComponent<Image>();
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    public void Update()
    {
        if (shipManager.rotateShips)
        {
            image.color = new Color(0.7f, 0.7f, 0.7f);
        }
        else
        {
            image.color = Color.white;
        }
    }

    private void TaskOnClick()
    {
        shipManager.rotateShips = !shipManager.rotateShips;
    }
}
