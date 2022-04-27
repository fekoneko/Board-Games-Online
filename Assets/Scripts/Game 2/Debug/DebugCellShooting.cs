using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCellShooting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GetComponent<OpponentCell>().MakeDamaged();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GetComponent<OpponentCell>().MakeMissed();
        }
         
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GetComponent<OpponentCell>().MakePainted();
        }
    }
}
