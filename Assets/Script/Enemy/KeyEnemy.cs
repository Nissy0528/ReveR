using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEnemy : MonoBehaviour {

   
    //団体敵
    public GameObject Top;　　　　
    public GameObject RightUp;
    public GameObject RightDown;
    public GameObject LeftUp;
    public GameObject LefDown;

    //キーエネミー
    public GameObject Keyenemy;

    public float PentagonRadius = 1;　　　//展開後,エネミー間の距離
    public float CircleRadius = 1.9f;　　 //キーエネミー円運動の半径
    public float speed = 9f;　　　　　　　//キーエネミー円運動の速度

    private Vector3 Center;　　　　　　　//キーエネミー円運動の円の中心
    private float radian;　　　　　　　　//キーエネミー円運動の角度

    private BoxCollider2D[] BoxC; 　　　　//子オブジェクトのboxColliderを取る用の配列
    private Turtroial_Move[] TMove;　　　 //団体敵の展開時移動用のComponentを取る用の配列

	void Start () {
        //初期化

        Center = transform.position;
        radian = 0;
        BoxC = GetComponentsInChildren<BoxCollider2D>();
        TMove = GetComponentsInChildren<Turtroial_Move>();
    }
	
	void Update () {
        KeyEnemyMove();
        Expansion();
	}

    /// <summary>
    /// 団体敵展開
    /// </summary>
    void Expansion()
    {
        if (IsKeyEnemyDead()){
            foreach (var x in TMove) if (x != null) x.enabled = true;
            foreach (var x in BoxC) if (x != null) x.enabled = true;
            GetExPosition();
            Destroy(this);　　//展開後もう使わないので、このcompentを消す
        }
    }

    /// <summary>
    /// 展開後のポジションを取る
    /// </summary>
    void GetExPosition()
    {
        Top.GetComponent<Turtroial_Move>().TargetPosition = new Vector3(
           Top.transform.position.x,
           Top.transform.position.y + PentagonRadius-Top.transform.localScale.y/2);

        RightDown.GetComponent<Turtroial_Move>().TargetPosition = new Vector3(
            RightDown.transform.position.x + PentagonRadius / 2 + RightDown.transform.localScale.x / 2 * PentagonRadius,
            RightDown.transform.position.y - Mathf.Sqrt(3) * PentagonRadius);

        RightUp.GetComponent<Turtroial_Move>().TargetPosition = new Vector3(
            RightUp.transform.position.x + Mathf.Sqrt(3) * PentagonRadius,
            RightUp.transform.position.y + PentagonRadius / 2-RightUp.transform.localScale.y/2 * PentagonRadius);

        LefDown.GetComponent<Turtroial_Move>().TargetPosition = new Vector3(
            LefDown.transform.position.x - PentagonRadius / 2-LefDown.transform.localScale.x/2 * PentagonRadius,
            LefDown.transform.position.y - Mathf.Sqrt(3) * PentagonRadius);

        LeftUp.GetComponent<Turtroial_Move>().TargetPosition = new Vector3(
            LeftUp.transform.position.x - Mathf.Sqrt(3) * PentagonRadius,
            LeftUp.transform.position.y + PentagonRadius / 2-LeftUp.transform.localScale.y/2 * PentagonRadius);
    }

    /// <summary>
    /// キーエネミーの移動
    /// </summary>
    void KeyEnemyMove()
    {
        if (!IsKeyEnemyDead()){
            Center = transform.position;
            radian += 0.01f * speed;
            var x = Mathf.Sin(radian) * CircleRadius;
            var y = Mathf.Cos(radian) * CircleRadius;
            Keyenemy.transform.position = new Vector3(x + Center.x, y + Center.y);
        }
    }
    
    /// <summary>
    /// キーエネミーが消滅したかの判定
    /// </summary>
    /// <returns></returns>
    bool IsKeyEnemyDead()
    {
        if (Keyenemy == null) return true;
        else return false;
    }
   
}
