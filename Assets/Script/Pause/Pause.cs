using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {

    // Use this for initialization
    public GameObject PauseMenu;
    private bool IsPause;
    public GameObject arrowmove;
	void Start () {
        IsPause = false;
        
	}
	
	// Update is called once per frame
	void Update () {
        Pauseing();
        
        
    }
    void Pauseing()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (IsPause) { Pause_No(); }
            else { Pause_Yes(); }
        }
        if (Input.GetKeyDown(KeyCode.J)&&IsPause)
        {
            if (arrowmove.GetComponent<ArrowMove>().GetIsSelect_1())
            {
                SceneManager.LoadScene("Title");
                Pause_No();
            }
            if (arrowmove.GetComponent<ArrowMove>().GetIsSelect_2())
            {
                SceneManager.LoadScene("StageSelect");
                Pause_No();
            }
            if (arrowmove.GetComponent<ArrowMove>().GetIsSelect_3())
            {
                SceneManager.LoadScene("Main");
                Pause_No();
            }
        }
    }
    void Pause_Yes()
    {
        Time.timeScale = 0;
        IsPause = true;
        PauseMenu.SetActive(true);
    }
    void Pause_No()
    {
        Time.timeScale = 1;
        IsPause = false;
        PauseMenu.SetActive(false);
    }


}
