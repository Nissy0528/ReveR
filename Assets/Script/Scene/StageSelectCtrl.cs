using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectCtrl : MonoBehaviour {

    int num;
    public GameObject[] stage = {};

    Vector2 position;

	// Use this for initialization
	void Start ()
    {
        position = stage[num].transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        float x = Input.GetAxis("Horizontal");

		if(x >= 0.5f)//右
        {
            num += 1;
        }
        
        if(x <= -0.5f)//左
        {
            num -= 1;
        }

        position = stage[num].transform.position;
        transform.position = position;

        if (num == 1)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene("Main");
            }
        }
	}
}
