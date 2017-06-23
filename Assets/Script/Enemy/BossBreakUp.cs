﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBreakUp : MonoBehaviour
{
    public GameObject core2;
    public GameObject boss;

    public float upspeed;
    public float time;

    private Vector3 savePosition;
    private float minRangeX;
    private float maxRangeX;
    private float minRangeY;
    private float maxRangeY;
    public float range;

    private bool shake;
    private GameObject core2Obj;
    // Use this for initialization
    void Start()
    {
        savePosition = transform.position;
        minRangeX = savePosition.x - range;
        maxRangeX = savePosition.x + range;
        minRangeY = savePosition.y - range;
        maxRangeY = savePosition.y + range;
    }

    // Update is called once per frame
    void Update()
    {
        bossFrom();
        Shake();
    }

    void bossFrom()
    {
        if (core2Obj == null&&boss == null)
        {
            shake = true;
            
            core2Obj = Instantiate(core2,transform);
            core2Obj.GetComponent<BossBreakUp>().enabled = false;
            
            
        }

        if (core2Obj != null)
        {
            bossBreakUp();
        }

    }

    private void Shake()
    {
        if (shake == true)
        {
            if (time <= 0.0f)
            {

                return;
            }

            time -= Time.deltaTime;
            if (Time.timeScale > 0.0f)
            {
                float x_val = Random.Range(minRangeX, maxRangeX);
                float y_val = Random.Range(minRangeY, maxRangeY);
                transform.position = new Vector3(x_val, y_val, transform.position.z);
            }
        }
    }

    void bossBreakUp()
    {
        Shake();
        if (time <= 0)
        {

            core2Obj.transform.position = new Vector3(core2Obj.transform.position.x,
                                                  core2Obj.transform.position.y + upspeed,
                                                  core2Obj.transform.position.z);
        }
    } 
}
