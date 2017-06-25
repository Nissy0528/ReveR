using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float angle;
    public GameObject shootObj;
    public GameObject reverceObj;

    private Vector3 targetPos;
    private Vector3 startPos;
    private Vector3 force;
    private GameObject target;
    private GameObject obj;
    private Rigidbody2D rigid;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        target = GameObject.Find("LifeMeter");
        //ライフタイムの座標をワールド座標に変換
        targetPos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(target.transform.position);
        targetPos.x -= 2f;
        Shoot(targetPos);
    }

    // Update is called once per frame
    void Update()
    {
        startPos = transform.position;
        //targetPos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(target.transform.position);
        //reverceObj.transform.position = new Vector3(obj.transform.position.x, -obj.transform.position.y, obj.transform.position.z);
        Shoot(targetPos);
    }

    /// <summary>
    /// 斜方投射
    /// </summary>
    /// <param name="i_targetPos">到達座標</param>
    private void Shoot(Vector3 i_targetPos)
    {
        ShootAngle(i_targetPos, angle);
    }

    /// <summary>
    /// 斜方投射時の速度ベクトルを決めて発射
    /// </summary>
    /// <param name="i_targetPos">到達座標</param>
    /// <param name="i_angle">投射時の角度</param>
    private void ShootAngle(Vector3 i_targetPos, float i_angle)
    {
        //速度ベクトル設定
        //速度ベクトルが0以下なら何もしない
        float speedVec = SpeedVec(i_targetPos, i_angle);
        if (speedVec <= 0.0f)
        {
            return;
        }
        Vector3 vec = Vec(speedVec, i_angle, i_targetPos);//投射時の方向設定
        SpawnShootObj(vec);//投射
    }

    /// <summary>
    /// 速度ベクトル設定
    /// </summary>
    /// <param name="i_targetPos">到達座標</param>
    /// <param name="i_angle">投射時の角度</param>
    /// <returns></returns>
    private float SpeedVec(Vector3 i_targetPos, float i_angle)
    {
        float distance = Mathf.Abs(i_targetPos.x - transform.position.x);//到達座標との距離

        float x = distance;
        float g = Physics2D.gravity.y;//重力
        float y0 = startPos.y;//投射時のY座標
        float y = i_targetPos.y;//到達Y座標

        float rad = i_angle * Mathf.Deg2Rad;//角度をラジアン変換
        float cos = Mathf.Cos(rad);//コサイン
        float tan = Mathf.Tan(rad);//タンジェント

        //速度ベクトル計算
        //速度ベクトルが0以下なら何もしない
        float v0Square = g * x * x / (2 * cos * cos * (y - y0 - x * tan));
        if (v0Square <= 0.0f)
        {
            return 0.0f;
        }

        float v0 = Mathf.Sqrt(v0Square);//計算した速度ベクトルを平方根に
        return v0;
    }

    /// <summary>
    /// 投射時の方向設定
    /// </summary>
    /// <param name="i_v0">速度ベクトル</param>
    /// <param name="i_angle">角度</param>
    /// <param name="i_targetPos">到達座標</param>
    /// <returns></returns>
    private Vector3 Vec(float i_v0, float i_angle, Vector3 i_targetPos)
    {
        Vector3 startPos = this.startPos;//投射時の座標
        Vector3 targetPos = i_targetPos;//到達座標
        startPos.y = 0.0f;
        targetPos.y = 0.0f;

        //到達座標の方向を設定
        Vector3 dir = (targetPos - startPos).normalized;
        Quaternion yawRot = Quaternion.FromToRotation(Vector3.right, dir);
        Vector3 vec = i_v0 * Vector3.right;//設定した方向に速度ベクトルをかける

        //到達座標に行くように方向と速度を設定
        vec = yawRot * Quaternion.AngleAxis(i_angle, Vector3.forward) * vec;

        return vec;
    }

    /// <summary>
    /// 投射
    /// </summary>
    /// <param name="i_shootVector">到達座標に着く速度</param>
    private void SpawnShootObj(Vector3 i_shootVector)
    {
        GameObject obj = Instantiate(shootObj, startPos, Quaternion.identity);
        Rigidbody2D rigid = obj.AddComponent<Rigidbody2D>();
        rigid.gravityScale *= -1;

        force = i_shootVector * rigid.mass;
        rigid.AddForce(force, ForceMode2D.Impulse);
    }
}
