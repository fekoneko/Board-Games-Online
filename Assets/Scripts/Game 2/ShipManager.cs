using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [SerializeField] public GameObject[] ships;
    public bool rotateShips = false;

    public void SetShipDragging(bool en)
    {
        foreach (GameObject i in ships)
        {
            i.GetComponent<ShipDragDrop>().SetState(en);
        }
    }
}
