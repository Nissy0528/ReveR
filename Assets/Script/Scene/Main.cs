using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public float stopTime;
    public Player player;

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
    }

    // Update is called once per frame
    void Update()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemyDead = GameObject.Find("Boom_effct");

        if (enemys.Length == 0.0f && enemyDead == null && isClear)
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

    public void SetIsClear(bool isClear)
    {
        this.isClear = isClear;
    }
}
