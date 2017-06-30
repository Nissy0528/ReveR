using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{
    public GameObject player;//プレイヤー
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

    private Player p_Class;//プレイヤークラス
    private Joint lj_Class;//左ジョイントクラス
    private Joint rj_Class;//右ジョイントクラス
    private bool isReset;//加速度初期化判定
    private float iniScrollSpeed;//スクロール速度

    // Use this for initialization
    void Start()
    {
        p_Class = player.GetComponent<Player>();//プレイヤークラス取得
        iniScrollSpeed = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMaxSpeed();//プレイヤーの速度を常に最大に
        InputManager();//スティック入力差の表示設定
        DataClear();//保存したデータを削除
        ScrollUp();//スクロール早送り
        Clear();//強制クリア
        GameOver();//強制ゲームオーバー
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
    /// プレイヤーの速度を常に最大に
    /// </summary>
    private void PlayerMaxSpeed()
    {
        if (!isPlayerMaxSpeed) return;

        GameObject.Find("MainManager").GetComponent<Main>().lifeTime = 60;
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("GameClear");
        }
    }

    /// <summary>
    /// 強制ゲームオーバー
    /// </summary>
    private void GameOver()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameObject.Find("MainManager").GetComponent<Main>().lifeTime = 0.0f;
        }
    }
}
