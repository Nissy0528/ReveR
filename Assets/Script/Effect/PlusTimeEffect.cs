using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusTimeEffect : MonoBehaviour
{
    public float startSpeed;//初期速度
    public float changeDis;//向き切り替え距離
    public float deadDis;//消滅距離
    public float angle;//斜方投射時の角度
    public float speed;//斜方投射時の速度

    private Vector2 direc;//向き
    private Vector3 iniPos;//初期座標
    private Vector3 startPos;//斜方投射時の初期座標
    private Vector3 lifeUIPos;//ライフタイムUIの座標
    private Rigidbody2D rigid;//リジッドボディ
    private GameObject lifeUI;//ライフタイムUI
    private bool isChange;//向き切り替えフラグ

    // Use this for initialization
    void Start()
    {
        isChange = false;//向き切り替えフラグfalseに
        rigid = GetComponent<Rigidbody2D>();
        lifeUI = GameObject.Find("LifeMeter");
        //ライフタイムの座標をワールド座標に変換
        lifeUIPos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(lifeUI.transform.position);
        lifeUIPos.x -= 2f;
        iniPos = transform.position;
        StartDirec();//最初の向き設定
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDirec();//移動先変更
        Move();//移動
        DestroyObj();//消滅
    }

    /// <summary>
    /// 最初の向き設定
    /// </summary>
    private void StartDirec()
    {
        //最初の向きはランダムで設定
        float x = Random.Range(-1, 2);
        float y = Random.Range(-1, 2);

        direc = new Vector2(x, y);
        Rotate();
        rigid.AddForce(transform.up * startSpeed, ForceMode2D.Impulse);//瞬発移動
    }

    /// <summary>
    /// 回転
    /// </summary>
    private void Rotate()
    {
        float angle = (Mathf.Atan2(direc.y, direc.x) * Mathf.Rad2Deg) - 90.0f;//角度計算
        Quaternion newRota = Quaternion.Euler(0.0f, 0.0f, angle);//見る角度設定
        transform.rotation = newRota;//指定した方向に向く
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (!isChange) return;
        Shoot(lifeUIPos);
        isChange = false;
    }

    /// <summary>
    /// 向き切り替え
    /// </summary>
    private void ChangeDirec()
    {
        float dis = Vector3.Distance(iniPos, transform.position);
        if (rigid.velocity.magnitude <= changeDis && rigid.gravityScale == 0.0f)
        {
            isChange = true;
            startPos = transform.position;
            rigid.velocity = Vector2.zero;
            rigid.drag = 0.0f;
            rigid.gravityScale = 1.0f;
        }
    }

    /// <summary>
    /// オブジェクト消滅
    /// </summary>
    private void DestroyObj()
    {
        //ターゲットとの距離計算
        float dis = Vector2.Distance(lifeUIPos, transform.position);

        //ターゲットとの距離が指定した距離以下になったら消滅
        if (dis <= deadDis)
        {
            Destroy(gameObject);
        }
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
        float g = Physics2D.gravity.y * speed;//重力
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
        rigid.gravityScale *= speed;
        Vector3 force = i_shootVector * rigid.mass;//力（速度×重さ）
        rigid.AddForce(force, ForceMode2D.Impulse);//投射する
    }
}

