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
    public GameObject warning;
    public int stopTime;
    public float battleTime;
    public float b_TimeMax;
    public float bossMoveDelay;

    private GameObject[] enemys;
    private GameObject enemyDead;
    private GameObject timeText;
    private GameObject battleTimeText;
    private GameObject waveText;
    private GameObject warningObj;
    private bool isStop;
    private bool isClear;
    private bool isBattleTime;
    private int stopCnt;
    private int waveNum;
    private float bossMoveCnt;

    // Use this for initialization
    void Start()
    {
        Evaluation = new List<string>();


        isStop = false;
        isClear = false;
        isBattleTime = false;
        time = 0.0f;
        timeText = GameObject.Find("Time");
        battleTimeText = GameObject.Find("LifeTime");
        waveText = GameObject.Find("Wave");
        timeText.SetActive(true);

        waveNum = 0;
        bossMoveCnt = bossMoveDelay;
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

        if (battleTime <= 0.0f)
        {
            SceneManager.LoadScene("GameOver");
        }

        TimeCount();//経過時間処理
        BattleTimeCount();//生存時間処理
        Stop();//停止
        E_WaveSpawn();//ウェイブ生成
        WaveNum();//現在のウェイブ表示
        BossWarning();//ボス登場エフェクト
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
    private void BattleTimeCount()
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

        if (bossMoveCnt <= 0.0f)
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
        if (battleTime > 0.0f)
        {
            this.battleTime = Mathf.Min(this.battleTime + battleTime, b_TimeMax);
        }
        if (battleTime < 0.0f)
        {
            this.battleTime = Mathf.Max(this.battleTime + battleTime, 0.0f);
        }
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
