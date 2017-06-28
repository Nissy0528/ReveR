using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMove : MonoBehaviour {

    public Vector3 TargetPosition = Vector3.zero;//目標ポジション
    public float speed;//移動速度
    public float StintDistance = 0.1f;


    private Vector3 Velocity;
    private float Distance;
    private bool IsGetDistance = false;
    void Start () {
        speed = speed / 10;
    }
	
	// Update is called once per frame
	void Update () {
        Move();
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
            Destroy(this);
        }

    }

    public void SetTargetPos(Vector3 Pos)
    {
        TargetPosition = Pos;
    }


}