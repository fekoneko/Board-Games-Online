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
            GameObject[] MainServerController = GameObject.FindGameObjectsWithTag("mainServerController");
            foreach (GameObject i in MainServerController)
            {
                //i.SetActive(false);
                Destroy(i);
            }
        }
    }
}
