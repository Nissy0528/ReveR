using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public static float time;
    public Player player;
    public int stopTime;
    public float battleTime;

    private GameObject[] enemys;
    private GameObject enemyDead;
    private GameObject timeText;
    private GameObject battleTimeText;
    private bool isStop;
    private bool isClear = true;
    private bool isBattleTime;
    private int stopCnt;

    // Use this for initialization
    void Start()
    {
        isStop = false;
        if (GameObject.Find("Tutorial") != null)
        {
            isClear = false;
        }
        isBattleTime = false;
        time = 0.0f;
        timeText = GameObject.Find("Time");
        battleTimeText = GameObject.Find("LifeTime");
        timeText.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemyDead = GameObject.Find("Boom_effct");

        if (enemys.Length == 0 && enemyDead == null && isClear)
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("GameClear");
        }

        if (player.IsStop())
        {
            SceneManager.LoadScene("GameOver");
        }

        TimeCount();//経過時間処理
        LifeTimeCount();//生存時間処理
        Stop();//停止
    }

    /// <summary>
    /// 経過時間処理
    /// </summary>
    private void TimeCount()
    {
        if (enemys.Length == 0) return;

        time += Time.deltaTime;
        time = Mathf.Round(time * 100) / 100;
        timeText.GetComponent<Text>().text = time.ToString();
    }

    /// <summary>
    /// 生存時間処理
    /// </summary>
    private void LifeTimeCount()
    {
        if (enemys.Length == 0) return;

        if (isBattleTime)
        {
            battleTime = Mathf.Max(battleTime - Time.deltaTime, 0.0f);
        }
        battleTime = Mathf.Round(battleTime * 100) / 100;
        battleTimeText.GetComponent<Text>().text = battleTime.ToString();
    }

    /// <summary>
    /// ゲーム停止
    /// </summary>
    private void Stop()
    {
        if (!isStop) return;

        Time.timeScale = 0.0f;
        stopCnt += 1;
        if (stopCnt >= stopTime)
        {
            Time.timeScale = 1.0f;
            isStop = false;
        }
    }

    /// <summary>
    /// クリア判定設定
    /// </summary>
    /// <param name="isClear">クリア判定（trueならクリアシーンに移行）</param>
    public void SetIsClear(bool isClear)
    {
        this.isClear = isClear;
    }

    /// <summary>
    /// 停止判定設定
    /// </summary>
    /// <param name="isStop">停止判定</param>
    public void SetStop()
    {
        stopCnt = 0;
        isStop = true;
    }

    /// <summary>
    /// 停止判定取得
    /// </summary>
    /// <returns>停止判定</returns>
    public bool IsStop()
    {
        return isStop;
    }

    /// <summary>
    /// 生存時間設定
    /// </summary>
    /// <param name="battleTime">生存時間</param>
    public void SetBattleTime(float battleTime)
    {
        this.battleTime += battleTime;
        isBattleTime = false;
    }

    /// <summary>
    /// 生存時間経過判定設定
    /// </summary>
    /// <param name="isBattleTime"></param>
    public void SetIsBattleTime(bool isBattleTime)
    {
        this.isBattleTime = isBattleTime;
    }
}
