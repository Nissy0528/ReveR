﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour {
    
	void Update () {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
