using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELMove : MonoBehaviour
{
    public float speed;
    public float width;

    private float WXmax;
    private float WXmin;
    private float WYmax;
    private float WYmin;

    private List<Vector2> velocity;
    private int z;

    private Rigidbody2D rigid;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        velocity = new List<Vector2>()
        {
            new Vector2(1,0),new Vector2(-1,0),new Vector2(0,1),new Vector2(0,-1),
        };

        z = Random.Range(0, 4);

        WXmax = GetComponent<Rigidbody2D>().position.x + width * 0.5f;
        WXmin = GetComponent<Rigidbody2D>().position.x - width * 0.5f;
        WYmax = GetComponent<Rigidbody2D>().position.y + width * 0.5f;
        WYmin = GetComponent<Rigidbody2D>().position.y - width * 0.5f;


        if (z == 0 || z == 1)
        {
            rigid.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
        if (z == 2 || z == 3)
        {
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
        }      
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody2D>().position.x < WXmin ||
            GetComponent<Rigidbody2D>().position.x > WXmax ||
            GetComponent<Rigidbody2D>().position.y < WYmin ||
            GetComponent<Rigidbody2D>().position.y > WYmax
            )
        {
            GetComponent<Rigidbody2D>().AddForce(-velocity[z] * speed);
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(velocity[z] * speed);
        }

        if (GetComponent<Enemy>().IsDead())
        {
            Destroy(this);
        }
    }

}
