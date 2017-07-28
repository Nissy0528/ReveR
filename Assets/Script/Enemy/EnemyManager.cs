using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public float scrollSpeed;//移動速度
    public float addLifeTime;//ボスバトル時間に加算する値（最大値）
    public bool isScroll;//下にスクロールするか
    public bool isStopScroll;//途中で停止するか
    public GameObject stopPoint;//停止ポイント
    public GameObject ExC;//ランクS_UI
    public GameObject Nic;//ランクA_UI
    public GameObject Nor;//ランクB_UI
    public GameObject PlusTime;//加算タイムUI
    public GameObject MinusTime;//減算タイムUI
    public GameObject plusEffect;//ライフタイム加算エフェクト
    public GameObject enemyLine;//グループライン
    public Material Line;//変更するグループラインマテリアル
    public Material greenAura;//変更するオーラマテリアル（緑）

    private float LimitTime;　　　//制限時間フレーム化
    private float CurrentTime;   //進行中のフレーム
    private float damage;//ダメージ
    private int iniChildCnt;//初期の子オブジェクトの数
    private bool IsTimeStart;　　　//読秒開始
    private bool isChildDestroy;
    private bool IsStopPoint;//StopPointに着いたか
    private GameObject main;
    private GameObject camera;
    private GameObject rankPoint;
    private Vector3 spawmPos;
    private Vector3 plusEffectPos;

    private bool IsLine;

    // Use this for initialization
    void Start()
    {
        camera = GameObject.Find("Main Camera");//カメラオブジェクト取得
        main = GameObject.Find("MainManager");//メインオブジェクト取得

        //ボスじゃなければランクの表示座標設定
        if (tag != "Boss")
        {
            rankPoint = transform.FindChild("EnemyLine").gameObject;
            spawmPos = rankPoint.transform.position;
        }
        plusEffectPos = Vector3.zero;

        LimitTime = addLifeTime * 60;
        CurrentTime = LimitTime;
        damage = GameObject.Find("Player").GetComponent<Player>().damage;
        iniChildCnt = GetAllChildren.GetAll(gameObject).Count;//全ての子オブジェクトの数取得

        IsTimeStart = false;
        isChildDestroy = false;
        IsLine = true;

    }

    // Update is called once per frame
    void Update()
    {

        Move();//移動
        MoveStop();//停止
        TimeStart();//コンボ時間
        ChildDestroy();//子オブジェクト全削除
        Evaluation();//ランクの分割
        EnemyAuraChange();//最後の敵のオーラの色を変える

        //画面下に出たら
        if (transform.position.y < camera.GetComponent<Camera>().ScreenToWorldPoint(Vector3.zero).y - transform.lossyScale.y / 2)
        {
            main.GetComponent<Main>().SetIsLifeTime(false);//ライフタイム減少停止
            PlusEffect(damage, main.GetComponent<Main>().lifeTime, 1);//ライフタイムにダメージ
            DestroyObj();//消滅
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
            IsStopPoint = true;
        }
    }

    /// <summary>
    /// 時間経過開始
    /// </summary>
    private void TimeStart()
    {
        //子オブジェクトが減ったら
        if (GetAllChildren.GetAll(gameObject).Count < iniChildCnt && !IsTimeStart)
        {
            EnemysActive();
            //ボスバトル時間の減少を始める
            main.GetComponent<Main>().SetIsLifeTime(true);
            IsTimeStart = true;

            if (gameObject.tag != "Boss"&& IsLine == true)
            {
                enemyLine.GetComponent<EnemyLine>().GetComponent<LineRenderer>().material = Line;
                IsLine = false;
            }
        }

    }

    /// <summary>
    /// StopPoint着いたかとか
    /// </summary>
    /// <returns></returns>
    public bool IsStop()
    {
        return IsStopPoint;
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

        main.GetComponent<Main>().SetIsLifeTime(true);
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
                addLifeTime = addLifeTime * 2 / 3;
            }
            if (CurrentTime < LimitTime * 1 / 3)
            {
                if (CurrentTime < 0) CurrentTime = 0;
                Main.Evaluation.Add("B");
                TextObj = Instantiate(Nor, GameObject.Find("Canvas").transform);
                addLifeTime = addLifeTime * 1 / 3;
            }

            TextObj.GetComponent<JudgeUI>().SetTargetPosition(spawmPos);

            if (gameObject.tag == "Boss")
            {
                TextObj.GetComponent<JudgeUI>().SetFade(true, true, 1.0f);
            }


            GetPlusTime(TextObj);
            //GetMinusTime();
            PlusEffect(addLifeTime, main.GetComponent<Main>().lifeTime, 0);

            main.GetComponent<Main>().SetIsLifeTime(false);
            main.GetComponent<Main>().StartTime(addLifeTime);

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

        if (gameObject.tag == "Boss")
        {
            PlusTimeObj.GetComponent<JudgeUI>().SetFade(true, false, 0.0f);
        }

        PlusTimeObj.GetComponent<JudgeUI>().SetTargetPosition(new Vector3(spawmPos.x + 1f, spawmPos.y + 1f));


        PlusTimeObj.GetComponent<Text>().text = "+" + ((int)addLifeTime).ToString() + "." +
            ((int)addLifeTime / 10).ToString() + ((int)(addLifeTime % 10)).ToString();

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
    private void PlusEffect(float time, float currentTime, int type)
    {
        if (plusEffectPos == Vector3.zero)
        {
            if (tag != "Boss")
            {
                plusEffectPos = rankPoint.transform.position;
            }
            else
            {
                plusEffectPos = transform.position;
            }
        }

        GameObject p_Effect = Instantiate(plusEffect, plusEffectPos, transform.rotation); ;
        p_Effect.GetComponent<PTimeEffectSpawner>().SetAddTime(time, currentTime, type);
    }

    /// <summary>
    /// グループ内の敵を起動状態にする
    /// </summary>
    private void EnemysActive()
    {
        if (tag == "KeyEnemy" || tag == "Boss") return;

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.GetComponent<Enemy>() != null)
            {
                transform.GetChild(i).GetComponent<Enemy>().SetActive();
            }
            if (child.transform.childCount > 0)
            {
                for (int j = 0; j < child.transform.childCount; j++)
                {
                    GameObject grandson = child.transform.GetChild(j).gameObject;
                    if (grandson.GetComponent<Enemy>() != null)
                    {
                        child.transform.GetChild(j).GetComponent<Enemy>().SetActive();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 最後の敵のオーラの色を変える
    /// </summary>
    private void EnemyAuraChange()
    {
        if (tag == "Boss") return;

        //全ての子オブジェクト取得
        List<GameObject> children = GetAllChildren.GetAll(gameObject);
        //エネミー用リスト
        List<GameObject> enemys = new List<GameObject>();
        //タグがEnemyの子オブジェクト用意したリストに追加
        foreach (GameObject c in children)
        {
            if (c.tag == "Enemy")
            {
                enemys.Add(c);
            }
        }

        enemys.RemoveAll(x => x == null);//空の要素削除
        //エネミーが一つしか残ってなかったらオーラの色変更
        if (enemys.Count == 1)
        {
            enemys[0].transform.Find("EnemyAura").GetComponent<SpriteRenderer>().material = greenAura;
            plusEffectPos = enemys[0].transform.position;
        }
    }

    /// <summary>
    /// タイム減少フラグ取得
    /// </summary>
    /// <returns></returns>
    public bool GetIsTimeStart()
    {
        return IsTimeStart;
    }

    /// <summary>
    /// スクロールしているかを取得
    /// </summary>
    /// <returns></returns>
    public bool GetIsScroll()
    {
        return isScroll;
    }
}
