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
    public float speed=1;
    
    private bool IsPause;
    private bool IsClose_Menu;

    private Behaviour[] chirdrens;
    private Behaviour[] myComponents;
    private List<Behaviour[]> List_chirdrens = new List<Behaviour[]>();
    private List<Behaviour[]> List_myComponents = new List<Behaviour[]>();


    private int time;
    private Vector3 MenuMove;

    [SerializeField]
    public List<GameObject> Stop_Object;
    void Start()
    {
        IsPause = false;
        IsClose_Menu = false;
        time = 200;
        
        MenuMove = new Vector3(0, 0, 0);


        
        for (int i = 0; i < Stop_Object.Count; i++)
        {
            //Debug.Log(Stop_Object.Count);
            //子供
            chirdrens = Stop_Object[i].GetComponentsInChildren<Behaviour>();
            List_chirdrens.Add(chirdrens);
            //自分
            myComponents = Stop_Object[i].GetComponents<Behaviour>();
            List_myComponents.Add(myComponents);
        }
    }
    // Update is called once per frame
    void Update()
    {
        time--;
        Menu_Move();
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
            if (IsPause)
            {
                IsClose_Menu = true;
                time = (int)(220/speed);
                MenuMove = new Vector3(-1, 0, 0);
            }
            else
            {
                OnPause();
                time = (int)(220/speed);
                MenuMove = new Vector3(1, 0, 0);
            }
        }
        if (IsClose_Menu == true&& time <0)
        {
            OnResume();
            IsClose_Menu = false;
        }
      

    }
    void Menu_Move()
    {
        if (time > 0 && time <= 220/speed)
        {
            PauseMenu.GetComponent<RectTransform>().position += MenuMove*speed;
        }
        
    }
    void Pauseing_MoveArrow()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && IsPause)
        {
            if (arrowmove.GetComponent<ArrowMove>().GetIsSelect_1())
            {
                SceneManager.LoadScene("Title");
                OnResume();
            }
            if (arrowmove.GetComponent<ArrowMove>().GetIsSelect_2())
            {
                SceneManager.LoadScene("StageSelect");
                OnResume();
            }
            if (arrowmove.GetComponent<ArrowMove>().GetIsSelect_3())
            {
                SceneManager.LoadScene("Main");
                OnResume();
            }
        }
    }
    void OnPause()
    {
       
        foreach (var x in List_chirdrens)
        {
           foreach(var z in x)
            {
                z.enabled = false;
            }
        }
        foreach (var x in List_myComponents)
        {
            foreach (var z in x)
            {
                z.enabled = false;
            }
        }
        Time.timeScale = 0;
        IsPause = true;
        PauseMenu.SetActive(true);
    }
    void OnResume()
    {
        foreach (var x in List_chirdrens)
        {
            foreach (var z in x)
            {
                z.enabled = true;
            }
        }
        foreach (var x in List_myComponents)
        {
            foreach (var z in x)
            {
                z.enabled = true;
            }
        }



        Time.timeScale = 1;
        IsPause = false;
        PauseMenu.SetActive(false);


    }
    
}
