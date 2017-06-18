using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float LimitTime;　　　//制限時間フレーム化
    private float CurrentTime;　　 //進行中のフレーム
    private int iniChildCnt;//初期の子オブジェクトの数
    private bool IsTimeStart;　　　//読秒開始
    private bool isChildDestroy;
    private GameObject main;
    private GameObject camera;

    // Use this for initialization
    void Start()
    {
        camera = GameObject.Find("Main Camera");
        main = GameObject.Find("MainManager");

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
            main.GetComponent<Main>().SetIsBattleTime(false);
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
        if (currentChildCnt < iniChildCnt)
        {
            //ボスバトル時間の減少を始める
            main.GetComponent<Main>().SetIsBattleTime(true);
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

        //敵を全部消すと、残った時間でランクを判断する、リストに入れる
        if (transform.childCount == 0)
        {
            if (CurrentTime >= LimitTime * 2 / 3)
            {
                Main.Evaluation.Add("S");
                Instantiate(ExC, GameObject.Find("Canvas").transform);
                JudgeUI.TargetPos = transform.position;
            }
            if ((CurrentTime >= LimitTime * 1 / 3) && (CurrentTime < LimitTime * 2 / 3))
            {
                Main.Evaluation.Add("A");
                Instantiate(Nic, GameObject.Find("Canvas").transform);
                JudgeUI.TargetPos = transform.position;
                addBattleTime = addBattleTime * 2 / 3;
            }
            if (CurrentTime < LimitTime * 1 / 3)
            {
                Main.Evaluation.Add("B");
                Instantiate(Nor, GameObject.Find("Canvas").transform);
                JudgeUI.TargetPos = transform.position;
                addBattleTime = addBattleTime * 1 / 3;
            }

            main.GetComponent<Main>().SetBattleTime(addBattleTime);//バトル時間加算
            DestroyObj();//判定終了後、このオブジェクトを消す
        }
    }

    /// <summary>
    /// 子オブジェクト全削除
    /// </summary>
    private void ChildDestroy()
    {
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
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
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
}
