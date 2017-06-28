using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public static float time;
    public static List<string> Evaluation;//ランクのデータを集まる用のリスト

    public GameObject[] enemyWave;
    public AudioClip[] bgm;
    public GameObject warning;
    public GameObject GameOver;
    public GameObject nextWave;
    public int stopTime;
    public float lifeTime;
    public float lifeTimeMax;
    public float bossMoveDelay;

    private GameObject[] enemys;
    private GameObject lifeTimeText;
    private GameObject enemyDead;
    private GameObject waveText;
    private GameObject warningObj;
    private AudioSource audio;
    private bool isStop;
    private bool isClear;
    private bool isLifeTime;
    private int stopCnt;
    private int waveNum;
    private float bossMoveCnt;
    private float currentTime;
    private float lifeTimeValue;
    private float cntStopper = 1.0f;

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
        time = 0.0f;
        lifeTimeText = GameObject.FindGameObjectWithTag("LifeTime");
        waveText = GameObject.Find("Wave");
        waveNum = 0;
        bossMoveCnt = bossMoveDelay;
        currentTime = lifeTime;
        audio = GetComponent<AudioSource>();
        lifeAlpha = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        enemyDead = GameObject.Find("Boom_effct");
        lifeTimeText = GameObject.FindGameObjectWithTag("LifeTime");

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

        LifeTimeCount();//生存時間処理
        LifeTimeSetActive();//ライフタイムアクティブ設定
        Stop();//停止
        E_WaveSpawn();//ウェイブ生成
        WaveNum();//現在のウェイブ表示
        BossWarning();//ボス登場エフェクト
        LifeTimeAlpha();//lifetimeのalpha 

        lifeTime = Mathf.Clamp(lifeTime, 0.0f, lifeTimeMax);
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
            lifeTime -= Time.deltaTime;
        }
        lifeTime = Mathf.Round(lifeTime * 100) / 100;
        lifeTimeText.GetComponent<Text>().text = lifeTime.ToString();
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
            audio.clip = bgm[0];
            audio.Play();
            GameObject next = null;
            if (next == null)
            {
                next = Instantiate(nextWave, GameObject.Find("Canvas").transform);
                next.GetComponent<Text>().text = "Wave " + waveNum.ToString();
            }
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
        waveText.GetComponent<Text>().text = "Wave " + (waveNum) + "/" + (enemyWave.Length - 1);
    }

    /// <summary>
    /// 最終ウェイブ
    /// </summary>
    /// <returns></returns>
    public bool LastWave()
    {
        return waveNum == enemyWave.Length;
    }

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
        if (enemyDead != null || waveNum == 0) return;

        if (bossMoveCnt <= 0.0f && enemyWave[waveNum].transform.FindChild("Boss") != null)
        {
            DestroyWarning();
            GameObject boss = enemyWave[waveNum].transform.FindChild("Boss").gameObject;
            boss.SetActive(true);
            if (boss.GetComponent<EnemyManager>().IsStop())
            {
                audio.UnPause();
            }
            return;
        }

        if (warningObj == null)
        {
            audio.clip = bgm[1];
            audio.Play();
            audio.Pause();
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
            if (waveNum >= enemyWave.Length)
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
    /// ライフタイムのアクティブ設定
    /// </summary>
    private void LifeTimeSetActive()
    {
        GameObject lifeText = GameObject.FindGameObjectWithTag("L_CntEffect");

        if (lifeText == null)
        {
            lifeTimeText.GetComponent<Text>().enabled = true;
        }

        if (lifeText != null)
        {
            lifeTimeText.GetComponent<Text>().enabled = false;
        }
    }
    /// <summary>
    /// lifetimeのalphaを調整
    /// </summary>
    private void LifeTimeAlpha()
    {
        lifeTimeText.GetComponent<Text>().color = new Color(1, 1, 1, lifeAlpha);
        GameObject.FindGameObjectWithTag("LifeFrame").GetComponent<Image>().color = new Color(1, 1, 1, lifeAlpha);
        //GameObject.FindGameObjectWithTag("LifeGauge").GetComponent<Image>().color = new Color(1, 1, 1, lifeAlpha);

        if (!isLifeTime && cntStopper == 1.0f && lifeAlpha > 0.3f)
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
    public void StartTime(float time)
    {
        lifeTime += time;
        lifeAlpha = 1f;
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
