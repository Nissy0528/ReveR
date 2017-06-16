using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            SceneManager.LoadScene("Main");
        }

        else if(Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            SceneManager.LoadScene("Credit");
        }
    }
}
