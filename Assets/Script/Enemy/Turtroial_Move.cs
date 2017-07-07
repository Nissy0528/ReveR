using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtroial_Move : MonoBehaviour
{
    public GameObject startupEffect;
    public Vector3 TargetPosition = Vector3.zero;//目標ポジション
    public float speed;//移動速度
    public Sprite normal;
    public float StintDistance = 0.1f;

    private Vector3 Velocity;
    private float Distance;
    private bool IsGetDistance = false;

    void Start()
    {
        speed = speed / 10;
    }
    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /// <summary>
    /// アクティブ化
    /// </summary>
    void GetMoveType()
    {

        if (transform.parent.tag != "Boss")
        {
            GetComponent<SpriteRenderer>().sprite = normal;//色のついた画像にする
                                                           //起動エフェクト生成
            GameObject s_effect = Instantiate(startupEffect, transform.position, transform.rotation, transform);
            s_effect.GetComponent<SpriteRenderer>().sprite = normal;
        }
        else
        {
            //transform.GetChild(0).GetComponent<LineRenderer>().enabled = true;
        }

        if (GetComponent<BoxCollider2D>() != null)
        {
            GetComponent<BoxCollider2D>().enabled = true;//あたり判定有効に

        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        if (TargetPosition != Vector3.zero)
            Distance = Vector3.Distance(TargetPosition, transform.position);
        Velocity = TargetPosition - transform.position;//移動量
        transform.position += Velocity * speed * Time.timeScale;//移動させる

        if (Distance <= StintDistance)
        {
            GetMoveType();//アクティブ化
            Destroy(this);
        }

    }

  
}
