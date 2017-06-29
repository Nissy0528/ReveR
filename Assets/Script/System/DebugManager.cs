using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{
    public GameObject player;//プレイヤー
    public GameObject l_Joint;//左ジョイント
    public GameObject r_Joint;//右ジョイント
    public GameObject inputManager;//スティック入力値
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
    public bool isPlayerMaxSpeed;//プレイヤーの速度を常に最大にするか
    public bool isScrollUp;//スクロールを早送りにするか
    public float drag;//摩擦力（プレイヤーの慣性をオンにしたとき使用）

    private PlayerOld p_OldClass;//プレイヤークラス（旧）
    private Player p_Class;//プレイヤークラス
    private Joint lj_Class;//左ジョイントクラス
    private Joint rj_Class;//右ジョイントクラス
    private bool isReset;//加速度初期化判定
    private float iniScrollSpeed;//スクロール速度

    // Use this for initialization
    void Start()
    {
        p_OldClass = player.GetComponent<PlayerOld>();//プレイヤークラス取得（旧）
        p_Class = player.GetComponent<Player>();//プレイヤークラス取得
        lj_Class = l_Joint.GetComponent<Joint>();//左ジョイントクラス取得
        rj_Class = r_Joint.GetComponent<Joint>();//右ジョイントクラス取得
        iniScrollSpeed = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        if (p_OldClass != null)
        {
            P_AddSpeed();//加速度切り替え
            Force();//慣性切り替え
            Return();//反転時の加速度初期化切り替え
            ReturnBoost();//反転時に加速するかの切り替え
            ReturnForce();//切り替えし慣性切り替え
        }
        PlayerMaxSpeed();//プレイヤーの速度を常に最大に
        JointMass();//ジョイントの重さ切り替え
        InputManager();//スティック入力差の表示設定
        DataClear();//保存したデータを削除
        TutorialSkip();//チュートリアルスキップ
        ScrollUp();//スクロール早送り
        Clear();
    }

    /// <summary>
    /// 加速度切り替え
    /// </summary>
    private void P_AddSpeed()
    {
        //加速度がオフなら
        if (!isAddSoeed)
        {
            p_OldClass.SetAddSpeed(1.0f);//加速度無効
            isReset = false;//加速度初期化判定false
        }
        //加速度オンなら
        else
        {
            //加速度初期化判定がtrueなら
            if (isReset)
            {
                p_OldClass.SetAddSpeed(0.0f);//加速度初期化
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
        p_OldClass.SetForce(isForce);//プレイヤーの慣性オンに
    }

    /// <summary>
    /// 反転時の加速度初期化切り替え
    /// </summary>
    private void Return()
    {
        p_OldClass.SetReturn(isReturnReset);//プレイヤー加速度初期化判定設定
    }

    /// <summary>
    /// 反転時に加速するかの切り替え
    /// </summary>
    private void ReturnBoost()
    {
        p_OldClass.SetRBoost(isReturnBoost);//プレイヤー反転時加速判定設定
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
        p_OldClass.SetRForce(isReturnFroce);
    }

    /// <summary>
    /// スティック入力差のデバッグ表示設定
    /// </summary>
    private void InputManager()
    {
        inputManager.GetComponent<InputManager>().SetDebug(isInput);

        inputManager.GetComponent<SpriteRenderer>().enabled = isInput;
        for (int i = 0; i < inputManager.transform.childCount; i++)
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
        GameObject tutorial = GameObject.Find("Tutorial");//チュートリアル

        if (!isTutorialSkip || tutorial == null) return;

        if (tutorial.GetComponent<TutoUISpawner>().GetTutoNum() < 4)
        {
            tutorial.GetComponent<TutoUISpawner>().SetTutoNum(4);
        }
    }

    /// <summary>
    /// プレイヤーの速度を常に最大に
    /// </summary>
    private void PlayerMaxSpeed()
    {
        if (!isPlayerMaxSpeed) return;

        p_Class.speed = p_Class.speedLimit;
    }

    /// <summary>
    /// スクロール早送り
    /// </summary>
    private void ScrollUp()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("EnemyManager");
        GameObject[] keyEnemys = GameObject.FindGameObjectsWithTag("KeyEnemy");

        if (isScrollUp)
        {
            if (enemys != null)
            {
                foreach (var e in enemys)
                {
                    e.GetComponent<EnemyManager>().scrollSpeed = iniScrollSpeed * 10.0f;
                }
            }
            if (keyEnemys != null)
            {
                foreach (var k in keyEnemys)
                {
                    k.GetComponent<EnemyManager>().scrollSpeed = iniScrollSpeed * 10.0f;
                }
            }
        }
        else
        {
            if (enemys != null)
            {
                foreach (var e in enemys)
                {
                    e.GetComponent<EnemyManager>().scrollSpeed = iniScrollSpeed;
                }
            }
            if (keyEnemys != null)
            {
                foreach (var k in keyEnemys)
                {
                    k.GetComponent<EnemyManager>().scrollSpeed = iniScrollSpeed;
                }
            }
        }
    }

    /// <summary>
    /// 強制クリア
    /// </summary>
    private void Clear()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("GameClear");
        }
    }
}
