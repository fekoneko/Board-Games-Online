using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NavButton : MonoBehaviour
{
    [SerializeField] private string sceneName = "";
    [SerializeField] private bool breakConnection = false;
    
    // Start is called before the first frame update
    public void Start()
    {
        Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    private void TaskOnClick()
    {
        SceneManager.LoadScene(sceneName);

        if (breakConnection)
        {
            GameObject[] MainServerControllers = GameObject.FindGameObjectsWithTag("mainServerController");
            foreach (GameObject i in MainServerControllers)
            {
                i.SetActive(false);
                MainServerController1 msc1;
                MainServerController2 msc2;
                i.TryGetComponent(out msc1);
                i.TryGetComponent(out msc2);
                if (msc1 != null)
                {
                    msc1.gameExited = true;
                }
                if (msc2 != null)
                {
                    msc2.gameExited = true;
                }
                Destroy(i);
            }
        }
    }
}
