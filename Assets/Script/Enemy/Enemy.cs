using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // Use this for initialization
    private bool isRWHit;
    private bool isLWHit;
    private bool isDead;
    private int x;
    private int y;

    void Start()
    {
        isRWHit = false;
        isLWHit = false;
        x = 0;
        y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (x != 0)
        {
            x--;
        }
        if (y != 0)
        {
            y--;
        }

        if (transform.parent.name != "Enemys" && transform.parent.GetChild(0).tag == "Untagged")
        {
            GameObject.Find("MainManager").GetComponent<Main>().Stop();
            Destroy(gameObject);
        }

    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "L_Joint" && x == 0)
        {
            isLWHit = false;

        }
        if (col.gameObject.tag == "R_Joint" && y == 0)
        {
            isRWHit = false;


        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "L_Joint")
        {
            isLWHit = true;
            x = 60;
            transform.parent = col.transform.parent;
            isDead = true;
        }
        if (col.gameObject.tag == "R_Joint")
        {
            isRWHit = true;
            y = 60;
            transform.parent = col.transform.parent;
            isDead = true;
        }

        if (isLWHit == true && isRWHit == true)
        {
            GameObject.Find("MainManager").GetComponent<Main>().Stop();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 死亡判定取得
    /// </summary>
    /// <returns>死亡判定</returns>
    public bool IsDead()
    {
        return isDead;
    }

}
