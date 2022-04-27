using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomButton : MonoBehaviour
{
    [SerializeField] private GameObject shipManagerObject;

    private ShipManager shipManager;

    // Start is called before the first frame update
    public void Start()
    {
        shipManager = shipManagerObject.GetComponent<ShipManager>();
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    private void TaskOnClick()
    {
        shipManager.SetRandomPositions();
    }
}
