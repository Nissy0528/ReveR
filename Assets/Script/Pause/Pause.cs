using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    // Use this for initialization
    public GameObject PauseMenu;
    public GameObject arrowmove;

    private bool IsPause;

    private Behaviour[] Stop_Object_Behaviour;

    [SerializeField]
    public List<GameObject> Stop_Object;
    void Start()
    {
        IsPause = false;

        Stop_Object.Add(GameObject.FindWithTag("Enemy"));
        for (int i = 0; i < Stop_Object.Count; i++)
        {
            Stop_Object_Behaviour = Array.FindAll(Stop_Object[i].GetComponentsInChildren<Behaviour>(),
           (obj) => { return obj.enabled; });


        }

    }
    // Update is called once per frame
    void Update()
    {
        Pauseing();
    }
    void Pauseing()
    {
        Pauseing_Meun();
        Pauseing_MoveArrow();

    }
    void Pauseing_Meun()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (IsPause) { Get_Obj_Pause_No(); }
            else { Get_Obj_Pause_Yes(); }
        }
    }
    void Pauseing_MoveArrow()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && IsPause)
        {
            if (arrowmove.GetComponent<ArrowMove>().GetIsSelect_1())
            {
                SceneManager.LoadScene("Title");
                Get_Obj_Pause_No();
            }
            if (arrowmove.GetComponent<ArrowMove>().GetIsSelect_2())
            {
                SceneManager.LoadScene("StageSelect");
                Get_Obj_Pause_No();
            }
            if (arrowmove.GetComponent<ArrowMove>().GetIsSelect_3())
            {
                SceneManager.LoadScene("Main");
                Get_Obj_Pause_No();
            }
        }
    }
    void Get_Obj_Pause_Yes()
    {
        try
        {
            foreach (var com in Stop_Object_Behaviour) com.enabled = false;

            Time.timeScale = 0;
            IsPause = true;
            PauseMenu.SetActive(true);
        }
        catch
        {
            SceneManager.LoadScene("Main");
        }
    }
    void Get_Obj_Pause_No()
    {
        foreach (var com in Stop_Object_Behaviour) com.enabled = true;

        Time.timeScale = 1;
        IsPause = false;
        PauseMenu.SetActive(false);
    }

}
