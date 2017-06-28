﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Main : MonoBehaviour
{
    public static float time;
    public static List<string> Evaluation;//ランクのデータを集まる用のリスト

    public GameObject[] enemyWave;
    public GameObject warning;
    public int stopTime;
    public float lifeTime;
    public float lifeTimeMax;
    public float bossMoveDelay;
    public GameObject GameOver;
   

    private GameObject[] enemys;
    private GameObject[] lifeTimeText;
    private GameObject enemyDead;
    private GameObject timeText;
    private GameObject waveText;
    private GameObject warningObj;
    private bool isStop;
    private bool isClear;
    private bool isLifeTime;
    private bool isAdd;
    private bool isSub;
    private int stopCnt;
    private int waveNum;
    private float bossMoveCnt;
    private float addCurrentTime;
    private float addLifeTime;
    private float subCurrentTime;
    private float subLifeTime;


    //lifetime とmeter のalpha
    public static float lifeAlpha;


    //GameOver
    public static bool IsGameOver = false;

    // Use this for initialization
    void Start()
    {
        Evaluation = new List<string>();
        isStop = false;
        isClear = false;
        isLifeTime = false;
        isAdd = false;
        time = 0.0f;
        timeText = GameObject.Find("Time");
        lifeTimeText = GameObject.FindGameObjectsWithTag("LifeTime");
        waveText = GameObject.Find("Wave");
        timeText.SetActive(true);
        waveNum = 0;
        bossMoveCnt = bossMoveDelay;
        addCurrentTime = lifeTime;
        subCurrentTime = lifeTime;
        
        lifeAlpha = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemyDead = GameObject.Find("Boom_effct");
        lifeTimeText = GameObject.FindGameObjectsWithTag("LifeTime");

        if (enemys.Length == 0 && enemyDead == null && isClear)
        {
            Time.timeScale = 1.0f;
            ControllerShake.Shake(0.0f, 0.0f);
            SceneManager.LoadScene("GameClear");
        }

        if (lifeTime <= 0.0f)
        {
            ControllerShake.Shake(0.0f, 0.0f);
            IsGameOver = true;
            GameOver.SetActive(true);
        }

        TimeCount();//経過時間処理
        LifeTimeCount();//生存時間処理
        LifeTime();//ライフタイム変動
        Stop();//停止
        E_WaveSpawn();//ウェイブ生成
        WaveNum();//現在のウェイブ表示
        BossWarning();//ボス登場エフェクト
        LifeTimeAlpha();//lifetimeのalpha 

        lifeTime = Mathf.Clamp(lifeTime, 0.0f, lifeTimeMax);


        Cursor.visible = false;
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
    /// ボスバトル時間処理
    /// </summary>
    private void LifeTimeCount()
    {
        if (enemys.Length == 0) return;

        if (isLifeTime)
        {
            //lifetimeとMETERのalpha
            lifeAlpha = 1f;

            
            lifeTime = Mathf.Max(lifeTime - Time.deltaTime, 0.0f);
        }
        lifeTime = Mathf.Round(lifeTime * 100) / 100;
        foreach (var b in lifeTimeText)
        {
            b.GetComponent<Text>().text = lifeTime.ToString();
        }
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
    /// ウェイブ生成
    /// </summary>
    private void E_WaveSpawn()
    {
        if (enemyWave[waveNum].transform.FindChild("Boss") != null) return;

        if (waveNum < enemyWave.Length - 1)
        {
            waveNum += 1;//ウェイブ番号加算
            enemyWave[waveNum].SetActive(true);//次のウェイブ生成
        }
        else
        {
            isClear = true;//最終ウェイブならクリア判定trueに
        }
    }

    /// <summary>
    /// 現在のウェイブ表示
    /// </summary>
    private void WaveNum()
    {
        waveText.GetComponent<Text>().text = "Wave " + (waveNum + 1) + "/" + enemyWave.Length;
    }

    /// <summary>
    ///  最終ウェブ
    /// </summary>
    /// <returns></returns>
    public bool LastWave()
    {
        return waveNum + 1 == enemyWave.Length;
    }

    /// <summary>
    /// ボス登場エフェクト
    /// </summary>
    private void BossWarning()
    {
        int childCnt = enemyWave[waveNum].transform.childCount;
        for (int i = 0; i < childCnt; i++)
        {
            if (enemyWave[waveNum].transform.GetChild(i).tag != "Boss" || isClear)
            {
                bossMoveCnt = bossMoveDelay;
                return;
            }
        }

        SpawnWarning();
    }

    /// <summary>
    /// ボス登場エフェクト生成
    /// </summary>
    private void SpawnWarning()
    {
        if (enemyDead != null) return;

        if (bossMoveCnt <= 0.0f && enemyWave[waveNum].transform.FindChild("Boss") != null)
        {
            DestroyWarning();
            enemyWave[waveNum].transform.FindChild("Boss").gameObject.SetActive(true);
            return;
        }

        if (warningObj == null)
        {
            warningObj = Instantiate(warning, GameObject.Find("Canvas").transform);
        }

        bossMoveCnt -= Time.deltaTime;
    }

    /// <summary>
    /// ボスエフェクト消滅
    /// </summary>
    private void DestroyWarning()
    {
        if (warningObj == null) return;

        //ボスエフェクトの子オブジェクトがなければ消滅
        if (warningObj.transform.childCount == 0)
        {
            if (waveNum >= enemyWave.Length - 1)
            {
                bossMoveCnt = bossMoveDelay;
            }
            else
            {
                Destroy(warningObj);
            }
        }
    }

    /// <summary>
    /// ライフタイム変動
    /// </summary>
    private void LifeTime()
    {
        if (isAdd)
        {
            lifeAlpha = 1f;
            lifeTime += Time.deltaTime * 6.0f;
            float addDif = Mathf.Abs(addCurrentTime - lifeTime);

            //Debug.Log("Add : " + addDif);

            if (addDif >= Mathf.Abs(addLifeTime))
            {
                isAdd = false;
            }
        }
        if (isSub)
        {
            lifeAlpha = 1f;
            lifeTime -= Time.deltaTime * 6.0f;
            float subDif = Mathf.Abs(subCurrentTime - lifeTime);

            //Debug.Log("Sub : " + subDif);

            if (subDif >= Mathf.Abs(subLifeTime))
            {
                isSub = false;
            }
        }
    }
    /// <summary>
    /// lifetimeのalphaを調整
    /// </summary>
    private void LifeTimeAlpha()
    {
        lifeTimeText[0].GetComponent<Text>().color = new Color(1, 1, 1, lifeAlpha);
        GameObject.FindGameObjectWithTag("LifeFrame").GetComponent<Image>().color=new Color(1, 1, 1, lifeAlpha);
        //GameObject.FindGameObjectWithTag("LifeGauge").GetComponent<Image>().color = new Color(1, 1, 1, lifeAlpha);

        if (!isLifeTime && !isAdd && !isSub && lifeAlpha > 0.3f)
            lifeAlpha -= 0.01f;
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
    /// <param name="addLifeTIme">生存時間</param>
    public void StartTime(float time, int type)
    {
        if (type == 0)
        {
            addLifeTime = time;
            addCurrentTime = lifeTime;
            isAdd = true;
        }
        if (type == 1)
        {
            subLifeTime = time;
            subCurrentTime = lifeTime;
            isSub = true;
        }
    }

    /// <summary>
    /// 生存時間経過判定設定
    /// </summary>
    /// <param name="islifeTime"></param>
    public void SetIsLifeTime(bool islifeTime)
    {
        this.isLifeTime = islifeTime;
    }
}
