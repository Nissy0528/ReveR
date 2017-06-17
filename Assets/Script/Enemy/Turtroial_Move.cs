using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtroial_Move : MonoBehaviour
{


    public Vector3 TargetPosition;//目標ポジション
    public float speed;//移動速度
    public float Distant = 98;//目標ポジションにずれる距離

    private Vector3 Velocity;
    private Vector3 startPosition;
    void Start()
    {

        speed = speed / 10;
        Distant = Distant / 1000;
        startPosition = transform.position;

    }
    // Update is called once per frame
    void Update()
    {

        Move();

    }

    /// <summary>
    /// 切り替えたい移動のタイプ
    /// </summary>
    /// スクリプトを有効化
    /// スタートのポジションをとる
    void GetMoveType()
    {
        if (gameObject.GetComponent<ELMove>() != null)
        {
            gameObject.GetComponent<ELMove>().enabled = true;
            gameObject.GetComponent<ELMove>().GetStartPosition(transform.position);
        }

    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        Velocity = TargetPosition - transform.position;//移動量

        transform.position += Velocity * speed * Time.timeScale;//移動させる

        if (
            ((Velocity.x <= Distant && Velocity.x >= -Distant) && (startPosition.x >= Distant || startPosition.x <= -Distant))
            && ((Velocity.y <= Distant && Velocity.y >= -Distant) && (startPosition.y >= Distant || startPosition.y <= -Distant)))
        {

            GetMoveType();
            Destroy(this);
        }

    }
}
