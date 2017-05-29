using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{

    public GameObject backGround;
    public float speed;
    private GameObject check;
    private float hori;
    private float vert;

    // Use this for initialization
    void Start()
    {
        check = GameObject.Find("PlayerPositionCheck");
    }

    // Update is called once per frame
    void Update()
    {

        hori = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");

        if (hori <= 0.5f || hori >= 0.5f ||//P右BG左 & P左BG右
            vert >= 0.5f || vert <= 0.5f)  //P上BG下 & P下BG上
        {
            backGround.transform.position =
               new Vector3(check.transform.position.x * -speed,
                           check.transform.position.y * -speed,
                           0);
        }
        else if (hori <= -0.5f && vert >= 0.5f || //P右斜め上
                 hori <= -0.5f && vert <= -0.5f || //P右斜め下
                 hori >= 0.5f && vert >= 0.5f || //P左斜め上
                 hori >= 0.5f && vert <= -0.5f)    //P左斜め下
        {
            backGround.transform.position =
               new Vector3(check.transform.position.x * -speed,
                           check.transform.position.y * -speed,
                           0);
        }
    }
}
