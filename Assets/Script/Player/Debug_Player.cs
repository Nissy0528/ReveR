using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_Player : MonoBehaviour
{
    public GameObject player;//プレイヤー
    public GameObject l_Joint;//左ジョイント
    public GameObject r_Joint;//右ジョイント
    //デバッグ用判定
    public bool kasokudo, kansei, kasokudoRisetto, kirikaesiBoost, jointOmosa, kirikaesiKansei;
    public float drag;//摩擦力（プレイヤーの慣性をオンにしたとき使用）

    private Player p_Class;//プレイヤークラス
    private Joint lj_Class;//左ジョイントクラス
    private Joint rj_Class;//右ジョイントクラス
    private bool isReset;//加速度初期化判定

    // Use this for initialization
    void Start()
    {
        p_Class = player.GetComponent<Player>();//プレイヤークラス取得
        lj_Class = l_Joint.GetComponent<Joint>();//左ジョイントクラス取得
        rj_Class = r_Joint.GetComponent<Joint>();//右ジョイントクラス取得
    }

    // Update is called once per frame
    void Update()
    {
        P_AddSpeed();//加速度切り替え
        Force();//慣性切り替え
        Return();//反転時の加速度初期化切り替え
        //ReturnBoost();//反転時に加速するかの切り替え
        JointMass();//ジョイントの重さ切り替え
        ReturnForce();//切り替えし慣性切り替え
    }

    /// <summary>
    /// 加速度切り替え
    /// </summary>
    private void P_AddSpeed()
    {
        //加速度がオフなら
        if (!kasokudo)
        {
            p_Class.SetAddSpeed(1.0f);//加速度無効
            isReset = false;//加速度初期化判定false
        }
        //加速度オンなら
        else
        {
            //加速度初期化判定がtrueなら
            if (isReset)
            {
                p_Class.SetAddSpeed(0.0f);//加速度初期化
                isReset = false;//加速度初期化判定false
            }
        }
    }

    /// <summary>
    /// 慣性切り替え
    /// </summary>
    private void Force()
    {
        player.GetComponent<Rigidbody2D>().drag = drag;//摩擦設定
        p_Class.SetForce(kansei);//プレイヤーの慣性オンに
    }

    /// <summary>
    /// 反転時の加速度初期化切り替え
    /// </summary>
    private void Return()
    {
        p_Class.SetReturn(kasokudoRisetto);//プレイヤー加速度初期化判定設定
    }

    /// <summary>
    /// 反転時に加速するかの切り替え
    /// </summary>
    private void ReturnBoost()
    {
        p_Class.SetRBoost(kirikaesiBoost);//プレイヤー反転時加速判定設定
    }

    /// <summary>
    /// ジョイントの重さ切り替え
    /// </summary>
    private void JointMass()
    {
        //左右のジョイントの重さ判定設定
        lj_Class.SetJMass(jointOmosa);
        rj_Class.SetJMass(jointOmosa);
    }

    /// <summary>
    /// 切り替えし慣性切り替え
    /// </summary>
    private void ReturnForce()
    {
        p_Class.SetRForce(kirikaesiKansei);
    }
}
