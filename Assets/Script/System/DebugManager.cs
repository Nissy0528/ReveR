using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    public GameObject player;//プレイヤー
    public GameObject l_Joint;//左ジョイント
    public GameObject r_Joint;//右ジョイント
    public GameObject tutorial;//チュートリアル
    public GameObject inputManager;//スティック入力値
    public AudioClip[] enemyDeadSE;//敵の消滅効果音
    public int seNum;//効果音番号

    //デバッグ用判定
    public bool isAddSoeed;//プレイヤーに加速度を付けるか
    public bool isForce;//プレイヤーに慣性を付けるか
    public bool isReturnReset;//プレイヤーの切り返しの慣性をなくすか
    public bool isReturnBoost;//プレイヤーの切り返しにブーストを付けるか
    public bool isJointMass;//プレイヤーのジョイントに重さを付けるか
    public bool isReturnFroce;//プレイヤーの切り返しに慣性を付けるか
    public bool isInput;//スティック入力値を視覚化するか
    public bool isDetaClear;//保存したデータを削除するか
    public bool isTutorialSkip;//チュートリアルをスキップするか
    public float drag;//摩擦力（プレイヤーの慣性をオンにしたとき使用）

    private PlayerOld p_Class;//プレイヤークラス
    private Joint lj_Class;//左ジョイントクラス
    private Joint rj_Class;//右ジョイントクラス
    private bool isReset;//加速度初期化判定

    // Use this for initialization
    void Start()
    {
        p_Class = player.GetComponent<PlayerOld>();//プレイヤークラス取得
        lj_Class = l_Joint.GetComponent<Joint>();//左ジョイントクラス取得
        rj_Class = r_Joint.GetComponent<Joint>();//右ジョイントクラス取得
        player.GetComponent<AudioSource>().clip = enemyDeadSE[seNum];
    }

    // Update is called once per frame
    void Update()
    {
        if (p_Class != null)
        {
            P_AddSpeed();//加速度切り替え
            Force();//慣性切り替え
            Return();//反転時の加速度初期化切り替え
            ReturnBoost();//反転時に加速するかの切り替え
            ReturnForce();//切り替えし慣性切り替え
        }
        JointMass();//ジョイントの重さ切り替え
        EnemySE();//効果音（敵を倒した時）
        InputManager();//スティック入力差の表示設定
        DataClear();//保存したデータを削除
        TutorialSkip();//チュートリアルスキップ
    }

    /// <summary>
    /// 加速度切り替え
    /// </summary>
    private void P_AddSpeed()
    {
        //加速度がオフなら
        if (!isAddSoeed)
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
        p_Class.SetForce(isForce);//プレイヤーの慣性オンに
    }

    /// <summary>
    /// 反転時の加速度初期化切り替え
    /// </summary>
    private void Return()
    {
        p_Class.SetReturn(isReturnReset);//プレイヤー加速度初期化判定設定
    }

    /// <summary>
    /// 反転時に加速するかの切り替え
    /// </summary>
    private void ReturnBoost()
    {
        p_Class.SetRBoost(isReturnBoost);//プレイヤー反転時加速判定設定
    }

    /// <summary>
    /// ジョイントの重さ切り替え
    /// </summary>
    private void JointMass()
    {
        //左右のジョイントの重さ判定設定
        lj_Class.SetJMass(isJointMass);
        rj_Class.SetJMass(isJointMass);
    }

    /// <summary>
    /// 切り替えし慣性切り替え
    /// </summary>
    private void ReturnForce()
    {
        p_Class.SetRForce(isReturnFroce);
    }

    /// <summary>
    /// 効果音（敵を倒した時）
    /// </summary>
    private void EnemySE()
    {
        if (player == null) return;
        player.GetComponent<AudioSource>().clip = enemyDeadSE[seNum];
    }

    /// <summary>
    /// スティック入力差のデバッグ表示設定
    /// </summary>
    private void InputManager()
    {
        inputManager.GetComponent<InputManager>().SetDebug(isInput);

        inputManager.GetComponent<SpriteRenderer>().enabled = isInput;
        for(int i = 0; i < inputManager.transform.childCount; i++)
        {
            inputManager.transform.GetChild(i).gameObject.SetActive(isInput);
        }
    }

    /// <summary>
    /// 保存したデータを削除
    /// </summary>
    private void DataClear()
    {
        if (!isDetaClear) return;

        PlayerPrefs.DeleteAll();
    }

    /// <summary>
    /// チュートリアルスキップ
    /// </summary>
    private void TutorialSkip()
    {
        if (!isTutorialSkip) return;

        if (tutorial.GetComponent<TutoUISpawner>().GetTutoNum() < 4)
        {
            tutorial.GetComponent<TutoUISpawner>().SetTutoNum(4);
        }
    }
}
