using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectCtrl : MonoBehaviour {

    int num;
    public GameObject[] stage = {};
    Vector2 position;

    private int delay;
    public int delayTime;

	// Use this for initialization
	void Start ()
    {
        position = stage[num].transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        float x = Input.GetAxis("Horizontal");

        if (x >= 0.5f)//右
        {
            delay += 1;
            if (delay > delayTime)
            {
                if (num < stage.Length)
                {
                    num -= 1;
                }
                if (num < 0)
                {
                    num = 2;
                }
                delay = 0;
            }
        }

        if (x <= -0.5f)//左
        {
            delay += 1;
            if (delay > delayTime)
            {
                if (num < stage.Length)
                {
                    num += 1;
                }
                if (num >= stage.Length) 
                {
                    num = 0;
                }
                delay = 0;
            }

        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            delay += 1;
            if (delay > delayTime)
            {
                if (num < stage.Length)
                {
                    num += 1;
                }
                if (num >= stage.Length)
                {
                    num = 0;
                }
                delay = 0;
            }
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            delay += 1;
            if (delay > delayTime)
            {
                if (num < stage.Length)
                {
                    num -= 1;
                }
                if (num < 0)
                {
                    num = 2;
                }
                delay = 0;
            }
        }


        position = stage[num].transform.position;
        transform.position = position;

        if (num == 1)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                SceneManager.LoadScene("Main");
            }
        }
	}
}
