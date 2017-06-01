using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public static float time;
    public float stopTime;
    public Player player;
    public GameObject timeText;

    private GameObject[] enemys;
    private GameObject enemyDead;
    private bool isStop;
    private bool isClear;
    private float s_Time;

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

        if (isStop)
        {
            s_Time += 1.0f;
            if (s_Time >= stopTime)
            {
                Time.timeScale = 1.0f;
                s_Time = 0.0f;
                isStop = false;
            }
        }

        TimeCount();//経過時間処理
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
    /// 停止
    /// </summary>
    public void Stop()
    {
        if (!isStop)
        {
            Time.timeScale = 0.0f;
            isStop = true;
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
}
