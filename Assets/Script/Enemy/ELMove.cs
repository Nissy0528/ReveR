using System.Collections;
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

    private List<Vector2> Move_velocity;
    private bool IsOverMoveRange;
    private Rigidbody2D Rigi;

    private float XRangeMax;
    private float XRangeMin;
    private float YRangeMax;
    private float YRangeMin;

    // Use this for initialization
    void Start()
    {
        Rigi = GetComponent<Rigidbody2D>();
        IsOverMoveRange = false;

        Move_velocity = new List<Vector2>()
        {
            new Vector2(1,0),new Vector2(-1,0),new Vector2(0,1),new Vector2(0,-1),
        };

        YRangeMax = Rigi.position.y + Move_Width * 0.5f;
        YRangeMin = Rigi.position.y - Move_Width * 0.5f;
        XRangeMax = Rigi.position.x + Move_Width * 0.5f;
        XRangeMin = Rigi.position.x - Move_Width * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        GetMoveRange();
        Move();
    }
    void Move()
    {
        if (MoveWay == MoveDirection.UP)
            Rigi.AddForce(IsOverMoveRange ? -Move_velocity[2] * speed : Move_velocity[2] * speed);

        else if (MoveWay == MoveDirection.DOWN)
            Rigi.AddForce(IsOverMoveRange ? -Move_velocity[3] * speed : Move_velocity[3] * speed);

        else if (MoveWay == MoveDirection.LEFT)
            Rigi.AddForce(IsOverMoveRange ? -Move_velocity[0] * speed : Move_velocity[0] * speed);

        else if (MoveWay == MoveDirection.RIGHT)
            Rigi.AddForce(IsOverMoveRange ? -Move_velocity[1] * speed : Move_velocity[1] * speed);
  
    }
    void GetMoveRange()
    {
        
        if (MoveWay == MoveDirection.UP || MoveWay == MoveDirection.DOWN)
        {
            if (Rigi.position.y > YRangeMax || Rigi.position.y < YRangeMin)
            {
                IsOverMoveRange = true;
            }
            else
            {
                IsOverMoveRange = false;
            }
            Rigi.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
        else if(MoveWay == MoveDirection.LEFT || MoveWay == MoveDirection.RIGHT)
        {
            if (Rigi.position.x > XRangeMax ||Rigi.position.x < XRangeMin)
            {
                IsOverMoveRange = true;
            }
            else
            {
                IsOverMoveRange = false;
            }
            Rigi.constraints = RigidbodyConstraints2D.FreezePositionY;
        }

    }

}
