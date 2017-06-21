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
    public float speed = 1;
    public static bool IsPause;
    private bool IsClose_Menu;

    private Behaviour[] chirdrens;
    private Behaviour[] myComponents;
    private List<Behaviour[]> List_chirdrens = new List<Behaviour[]>();
    private List<Behaviour[]> List_myComponents = new List<Behaviour[]>();


    private int time;
    private Vector3 MenuMove;
    private GameObject main;

    [SerializeField]
    public List<GameObject> Stop_Object;
    void Start()
    {
        main = GameObject.Find("MainManager");
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
            main.GetComponent<ControllerShake>().SetShake(0.0f, 0.0f, 0);

            if (IsPause)
            {
                IsClose_Menu = true;
                time = (int)(250 / speed);
                MenuMove = new Vector3(-1, 0, 0);
            }
            else
            {
                Stop_Object.RemoveAll(c => c == null);
                List_chirdrens.RemoveAll(c => c == null);
                List_myComponents.RemoveAll(c => c == null);
                PauseMenu.GetComponent<RectTransform>().localPosition = new Vector3(-743, 0, 0);
                OnPause();
                time = (int)(250 / speed);
                MenuMove = new Vector3(1, 0, 0);
            }
        }
        if (IsClose_Menu == true && time < 0)
        {
            OnResume();
            IsClose_Menu = false;
        }


    }
    void Menu_Move()
    {
        if (time > 0 && time <= 250 / speed)
        {
            PauseMenu.GetComponent<RectTransform>().position += MenuMove * speed;
        }

    }
    void Pauseing_MoveArrow()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && IsPause)
        {
            if (arrowmove.GetComponent<ArrowMove>().GetIsSelect(0))
            {
                SceneManager.LoadScene("Title");
                OnResume();
            }
            //if (arrowmove.GetComponent<ArrowMove>().GetIsSelect(1))
            //{
            //    SceneManager.LoadScene("StageSelect");
            //    OnResume();
            //}
            if (arrowmove.GetComponent<ArrowMove>().GetIsSelect(1))
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
            foreach (var z in x)
            {
                if (z != null)
                    z.enabled = false;
                if (z == null)
                    UnityEngine.Debug.Log(x.Length);
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
                if (z != null) z.enabled = true;
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
