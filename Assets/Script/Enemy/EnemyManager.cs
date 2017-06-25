using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public float scrollSpeed;//移動速度
    public float addBattleTime;//ボスバトル時間に加算する値（最大値）
    public bool isScroll;//下にスクロールするか
    public bool isStopScroll;//途中で停止するか
    public GameObject stopPoint;//停止ポイント
    public GameObject ExC;
    public GameObject Nic;
    public GameObject Nor;
    public GameObject PlusTime;
    public GameObject MinusTime;
    public GameObject plusEffect;

    private float LimitTime;　　　//制限時間フレーム化
    private float CurrentTime;　　 //進行中のフレーム
    private int iniChildCnt;//初期の子オブジェクトの数
    private bool IsTimeStart;　　　//読秒開始
    private bool isChildDestroy;
    private GameObject main;
    private GameObject camera;
    private GameObject rankPoint;
    private Vector3 spawmPos;

    private float previousTime;

    // Use this for initialization
    void Start()
    {
        camera = GameObject.Find("Main Camera");
        main = GameObject.Find("MainManager");
        if (tag != "Boss")
        {
            rankPoint = transform.FindChild("EnemyLine").gameObject;
            spawmPos = rankPoint.transform.position;
        }

        LimitTime = addBattleTime * 60;
        CurrentTime = LimitTime;
        iniChildCnt = transform.childCount;//初期の子オブジェクトの数取得
        IsTimeStart = false;
        isChildDestroy = false;
    }

    // Update is called once per frame
    void Update()
    {

        Move();//移動
        MoveStop();//停止
        TimeStart();//コンボ時間
        ChildDestroy();//子オブジェクト全削除
        Evaluation();//ランクの分割

        //画面下に出たら
        if (transform.position.y < camera.GetComponent<Camera>().ScreenToWorldPoint(Vector3.zero).y - transform.lossyScale.y / 2)
        {
            main.GetComponent<Main>().SetIsLifeTime(false);
            DestroyObj();
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (!isScroll) return;//スクロールしなければ何もしない
        transform.Translate(0, -scrollSpeed * Time.timeScale, 0, Space.World);//下にスクロール
    }

    /// <summary>
    /// 途中停止
    /// </summary>
    private void MoveStop()
    {
        if (!isStopScroll) return;//途中停止しなければ何もしない

        if (transform.position.y <= stopPoint.transform.position.y + 1.0f)
        {
            isScroll = false;
            Vector3 pos = transform.position;//更新用座標
            pos.y = Mathf.Lerp(pos.y, stopPoint.transform.position.y, scrollSpeed);//停止ポイントまで補間移動
            transform.position = pos;//座標更新
        }
    }

    /// <summary>
    /// 時間経過開始
    /// </summary>
    private void TimeStart()
    {
        int currentChildCnt = transform.childCount;//現在の子オブジェクトの数を取得

        //子オブジェクトが減ったら
        if (currentChildCnt < iniChildCnt && !IsTimeStart)
        {
            EnemysActive();
            //ボスバトル時間の減少を始める
            main.GetComponent<Main>().SetIsLifeTime(true);
            IsTimeStart = true;
        }

    }

    /// <summary>
    /// 消滅
    /// </summary>
    private void DestroyObj()
    {
        if (isStopScroll)
        {
            Destroy(stopPoint.gameObject);//停止ポイント消滅
        }
        Destroy(gameObject);//消滅
    }

    /// <summary>
    /// ランクの分割
    /// </summary>
    private void Evaluation()
    {
        //敵一つを倒すと,読秒開始
        if (!IsTimeStart) return;

        CurrentTime--;　//読秒
        GameObject TextObj = null;

        //敵を全部消すと、残った時間でランクを判断する、リストに入れる
        if (transform.childCount == 0)
        {
            if (CurrentTime >= LimitTime * 2 / 3)
            {
                Main.Evaluation.Add("S");
                TextObj = Instantiate(ExC, GameObject.Find("Canvas").transform);
            }
            if ((CurrentTime >= LimitTime * 1 / 3) && (CurrentTime < LimitTime * 2 / 3))
            {
                Main.Evaluation.Add("A");
                TextObj = Instantiate(Nic, GameObject.Find("Canvas").transform);
                addBattleTime = addBattleTime * 2 / 3;
            }
            if (CurrentTime < LimitTime * 1 / 3)
            {
                if (CurrentTime < 0) CurrentTime = 0;
                Main.Evaluation.Add("B");
                TextObj = Instantiate(Nor, GameObject.Find("Canvas").transform);
                addBattleTime = addBattleTime * 1 / 3;
            }

            TextObj.GetComponent<JudgeUI>().SetTargetPosition(spawmPos);

            GetPlusTime(TextObj);
            //GetMinusTime();
            PlusEffect();

            main.GetComponent<Main>().SetAddTime(addBattleTime);
            main.GetComponent<Main>().SetIsLifeTime(false);
            DestroyObj();//判定終了後、このオブジェクトを消す
        }
    }

    /// <summary>
    /// 子オブジェクト全削除
    /// </summary>
    private void ChildDestroy()
    {
        if (tag == "Boss") return;

        isChildDestroy = true;

        //タグがLineじゃない子オブジェクトが存在していたら削除フラグfalse
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag != "Line")
            {
                isChildDestroy = false;
            }
        }

        //削除フラグがtrueなら子オブジェクト全削除
        if (isChildDestroy)
        {
            spawmPos = rankPoint.transform.position;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).parent = null;
            }
        }
    }

    /// <summary>
    /// 下スクロールフラグ設定
    /// </summary>
    /// <param name="isScroll">スクロールフラグ</param>
    public void SetIsScroll(bool isScroll)
    {
        this.isScroll = isScroll;
    }

    /// <summary>
    /// PlusTime
    /// </summary>
    public void GetPlusTime(GameObject textObj)
    {
        GameObject PlusTimeObj;
        PlusTimeObj = Instantiate(PlusTime, GameObject.Find("Canvas").transform);
        PlusTimeObj.name = "PlusTime";


        PlusTimeObj.GetComponent<JudgeUI>().SetTargetPosition(new Vector3(spawmPos.x + 1f, spawmPos.y + 1f));

        PlusTimeObj.GetComponent<Text>().text = "+" + ((int)CurrentTime / 60).ToString() + "." +
            ((int)((int)CurrentTime % 60) / 10).ToString() + ((int)((int)CurrentTime % 60) % 10).ToString();

        textObj.GetComponent<JudgeUI>().SetScroll(scrollSpeed, rankPoint, PlusTimeObj);
    }
    /// <summary>
    /// MinusTime
    /// </summary>
    public void GetMinusTime()
    {
        GameObject MinusTimeObj;
        MinusTimeObj = Instantiate(MinusTime, GameObject.Find("Canvas").transform);

        MinusTimeObj.GetComponent<JudgeUI>().SetTargetPosition(new Vector3(spawmPos.x, spawmPos.y + 0.4f));

        MinusTimeObj.GetComponent<Text>().text = "-" + ((int)(LimitTime - CurrentTime) / 60).ToString() + "." +
            ((int)((int)(LimitTime - CurrentTime) % 60) / 10).ToString() + ((int)((int)(LimitTime - CurrentTime) % 60) % 10).ToString();

    }

    /// <summary>
    /// ライフタイム加算エフェクト
    /// </summary>
    private void PlusEffect()
    {
        GameObject p_Effect = null;

        //ライフタイムエフェクトが5個未満だったら生成
        if (p_Effect == null)
        {
            p_Effect = Instantiate(plusEffect, transform.position, transform.rotation);
        }
    }

    /// <summary>
    /// グループ内の敵を起動状態にする
    /// </summary>
    private void EnemysActive()
    {
        if (tag == "KeyEnemy") return;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "Enemy")
            {
                transform.GetChild(i).GetComponent<Enemy>().SetActive();
            }

        }
    }
}
