using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_LC : MonoBehaviour {

    // Use this for initialization
    public float radius = 1f;
    public float speed = 0.1f;
    public float MoveChange_time;

    private float angle;
    private Vector3 Center;
    private bool IsCircleMove;

	void Start () {
        MoveChange_time = 60;
        IsCircleMove = false;
    }
    void Update()
    {
        MoveChange_time--;

        if (MoveChange_time > 0)
            GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
       
        if (MoveChange_time == 0){
            GetCenter();
            IsCircleMove = true;
        }

        if (IsCircleMove && MoveChange_time <= 0) CircleMove();
    }
    void GetCenter()
    {
        float CenterX = transform.position.x - radius;
        float CenterY = transform.position.y;
        Center = new Vector3(CenterX, CenterY);

        if (transform.position.x > 0)
        {
            angle = Mathf.Atan2(0, transform.position.x);
        }
        else
        {
            angle = Mathf.Atan2(0, -transform.position.x);
        }

    }
    void CircleMove()
    {
        var radio = angle * (180 / Mathf.PI);
        angle += speed*0.5f;

        var x = Mathf.Cos(angle) * radius + Center.x;
        var y = Mathf.Sin(angle) * radius + Center.y;
        transform.position = new Vector3(x, y, 0);

        if (radio>=angle+360) 
        {
            IsCircleMove = false;
            MoveChange_time = 180;
        }
    }
}
