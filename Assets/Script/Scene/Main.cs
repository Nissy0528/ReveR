using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public float stopTime;

    private GameObject[] enemys;
    private bool isStop;
    private float s_Time;

    // Use this for initialization
    void Start()
    {
        isStop = false;
    }

    // Update is called once per frame
    void Update()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemys.Length == 0.0f)
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("GameClear");
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
}
