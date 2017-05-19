using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    // Use this for initialization
    public GameObject PauseMenu;
    private bool IsPause;
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
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
            IsPause = true;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1;
            IsPause = false;
        }
    }
		
	
}
