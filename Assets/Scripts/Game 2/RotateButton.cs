using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateButton : MonoBehaviour
{
    [SerializeField] private GameObject shipManagerObject;
    [SerializeField] private GameObject arrow;

    private ShipManager shipManager;
    private Image image;
    private float arrowAngle = 90.0f;

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
        float step = 550f * Time.deltaTime;
        if (shipManager.rotateShips)
        {
            if (arrowAngle > step)
                arrowAngle -= step;
            else
            {
                if (arrowAngle == 0f) return; /////////////////// !
                arrowAngle = 0f;
            }
        }
        else
        {
            if (90f - arrowAngle > step)
                arrowAngle += step;
            else
            {
                if (arrowAngle == 90f) return; /////////////////// !
                arrowAngle = 90f;
            }
        }
        arrow.GetComponent<RectTransform>().rotation = Quaternion.AngleAxis(arrowAngle, Vector3.back);
    }

    private void TaskOnClick()
    {
        shipManager.rotateShips = !shipManager.rotateShips;
    }
}
