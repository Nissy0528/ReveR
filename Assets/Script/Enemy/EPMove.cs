using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EPMove : MonoBehaviour {

    public float speed;
    //public float width;
    //private float MinWidth;

    private int x;
    
    private Vector3 player;
    //private Vector3 playerMove;
    private Vector3 Target;

    private bool IsGoal;
    private void Start()
    {
        
        transform.position = new Vector3(3, 3,0);
        
        IsGoal = false;
    }

    private void Update()
    {
        
        if (IsGoal==false)
        {
            player = GameObject.FindWithTag("Player").transform.position;
            Target = player - transform.position;
            IsGoal = true;
            x = 60;
        }
        else
        {
            //transform.position += new Vector3(Target.x * 0.01f * speed, Target.y * 0.01f * speed, 0);
            GetComponent<Rigidbody2D>().velocity=new Vector2(Target.x  * speed, Target.y  * speed);
           // GetComponent<Rigidbody2D>().MovePosition(player*speed*0.01f);


            //playerMove = GameObject.FindWithTag("Player").transform.position;
            if (player == transform.position)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                x--;
                if (x == 0)
                {
                    IsGoal = false;
                }

            }
        }

    }

}
