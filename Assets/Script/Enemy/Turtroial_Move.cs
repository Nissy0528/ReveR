using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtroial_Move : MonoBehaviour
{
    public Vector3 TargetPosition;//目標ポジション
    public float speed;//移動速度
    public bool isKeyEnemy;
    public Sprite normal;

    private Vector3 Velocity;
    private Vector3 startPosition;

    void Start()
    {
        speed = speed / 10;
        startPosition = transform.position;

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
        if (isKeyEnemy)
        {
            GetComponent<SpriteRenderer>().sprite = normal;//色のついた画像にする
            GetComponent<BoxCollider2D>().enabled = true;//あたり判定有効に
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        Velocity = TargetPosition - transform.position;//移動量

        transform.position += Velocity * speed * Time.timeScale;//移動させる

        if (Velocity == Vector3.zero)
        {
            GetMoveType();//アクティブ化
            Destroy(this);
        }

    }
}
