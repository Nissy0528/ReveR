﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELMove : MonoBehaviour
{
    public enum MoveDirection
    {
        UP, DOWN, LEFT, RIGHT
    }
    public float speed;
    public float Move_Width;
    public MoveDirection MoveWay;
    public float x;
    public float y;

    private List<Vector2> Move_velocity;
    private bool IsOverMoveRange;
    private Rigidbody2D Rigi;
    private Vector3 Position;
    private float time;

    private float XRangeMax;
    private float XRangeMin;
    private float YRangeMax;
    private float YRangeMin;
    private int List_num;

    // Use this for initialization
    void Start()
    {
        Rigi = GetComponent<Rigidbody2D>();
        Position = new Vector3(x, y);
        IsOverMoveRange = false;

        Move_velocity = new List<Vector2>()
        {
            new Vector2(1,0),new Vector2(-1,0),new Vector2(0,1),new Vector2(0,-1),
        };

        YRangeMax = Position.y + Move_Width * 0.5f;
        YRangeMin = Position.y - Move_Width * 0.5f;
        XRangeMax = Position.x + Move_Width * 0.5f;
        XRangeMin = Position.x - Move_Width * 0.5f;

        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Rigi.velocity.magnitude);
        if (time >= 10)
        {
            time = 0;
        }
        else
        {
            time += Time.deltaTime;
        }
        
        Move();
        GetMoveRange();
       
    }
    void Move()
    {
       
        if (MoveWay == MoveDirection.UP)
            List_num = IsOverMoveRange ? 3 : 2;

        else if (MoveWay == MoveDirection.DOWN)
            List_num = IsOverMoveRange ? 2 : 3;

        else if (MoveWay == MoveDirection.LEFT)
            List_num = IsOverMoveRange ? 0 : 1;

        else if (MoveWay == MoveDirection.RIGHT)
            List_num = IsOverMoveRange ? 1 : 0;

        Rigi.velocity = Move_velocity[List_num] * speed ;


    }
    void GetMoveRange()
    {
       
        if (MoveWay == MoveDirection.UP || MoveWay == MoveDirection.DOWN)
        {
            
            if ((transform.position.y > YRangeMax && List_num == 2) || 
                (transform.position.y < YRangeMin && List_num == 3))
            {
                IsOverMoveRange = IsOverMoveRange ? false : true;

            }
           
            Rigi.constraints = RigidbodyConstraints2D.FreezePositionX;
            Rigi.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else if(MoveWay == MoveDirection.LEFT || MoveWay == MoveDirection.RIGHT)
        {
            
            if ((transform.position.x > XRangeMax && List_num==0 ) || 
                (transform.position.x < XRangeMin && List_num==1))
            {
                IsOverMoveRange = IsOverMoveRange ? false : true;
            }
           
            Rigi.constraints = RigidbodyConstraints2D.FreezePositionY;
            Rigi.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

    }

}
