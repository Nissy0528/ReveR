using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public GameObject cursor;

    void Update()
    {
        Cursor.visible = false;

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (cursor.GetComponent<ArrowMove>().GetIsSelect(0))
            {
                SceneManager.LoadScene("Main");
            }
            if(cursor.GetComponent<ArrowMove>().GetIsSelect(1))
            {
                Application.Quit();
            }
        }

        else if(Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            SceneManager.LoadScene("Credit");
        }
    }
}
