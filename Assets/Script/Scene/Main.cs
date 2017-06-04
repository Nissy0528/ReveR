using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public static float time;
    public Player player;
    public GameObject timeText;
    public int stopTime;

    private GameObject[] enemys;
    private GameObject enemyDead;
    private bool isStop;
    private bool isClear;
    private int stopCnt;

    // Use this for initialization
    void Start()
    {
        isStop = false;
        isClear = false;
        time = 0.0f;
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
    /// ゲーム停止
    /// </summary>
    private void Stop()
    {
        if (!isStop) return;

        Time.timeScale = 0.0f;
        stopCnt += 1;
        if(stopCnt>=stopTime)
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
}
