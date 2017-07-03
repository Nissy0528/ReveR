using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBreakUp : MonoBehaviour
{
    public GameObject core2;//空のコア
    public GameObject boss;//ボス取得
    public GameObject BoomEffect;//爆発エフェクト
    public GameObject se;//SE
    public Main main;//MainManager取得
    public Sprite newBG;//ボス戦終了後背景
    public float upspeed;//ボス撤退の上昇速度
    public float time;//time
    public float range;

    private GameObject core2Obj;
    private Camera camera;//カメラ
    private Vector3 savePosition;
    private float minRangeX;
    private float maxRangeX;
    private float minRangeY;
    private float maxRangeY;
    private bool shake;//振動してるか？
    private bool isSE;

    // Use this for initialization
    void Start()
    {
        savePosition = transform.position;
        minRangeX = savePosition.x - range;
        maxRangeX = savePosition.x + range;
        minRangeY = savePosition.y - range;
        maxRangeY = savePosition.y + range;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();//カメラ取得
        isSE = false;
        //main = GetComponent<Main>();
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        BossFrom();
        Shake();

        Vector3 screenPosMax = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));//画面左上の座標
        if (transform.position.y >= screenPosMax.y + transform.localScale.y)//画面上に出たら
        {
            Destroy(gameObject);//コア破壊

            if (GameObject.Find("MainManager").GetComponent<Main>().GetWave() > 0)//次のウェイブがあれば
            {
                GameObject.Find("back3").GetComponent<SpriteRenderer>().sprite = newBG;//通常時背景に切り替え
                GameObject.Find("back3.3").GetComponent<SpriteRenderer>().sprite = newBG;
            }
        }
    }

    /// <summary>
    /// 空Core生成
    /// </summary>
    void BossFrom()
    {
        if (core2Obj == null&&boss.transform.childCount == 1)//空のコアがnull && ボスの子オブジェクトが１なら
        {
            shake = true;//振動をtrue
            core2Obj = Instantiate(core2,transform);//空のCoreを生成
        }

        if (core2Obj != null)//空のコアが生成されたら
        {
            if (main.LastWave() == false)//最終ウェイブでなければ
            {
                bossBreakUp();//ボス撤退処理
            }
            else
            {
                BossDead();//ボス死亡処理
            }
        }
    }

    /// <summary>
    /// ボス振動処理
    /// </summary>
    private void Shake()
    {
        if (shake == true)
        {
            if (time <= 0.0f)
            {
                return;
            }

            time -= Time.deltaTime;
            if (Time.timeScale > 0.0f)
            {
                float x_val = Random.Range(minRangeX, maxRangeX);//振動範囲
                float y_val = Random.Range(minRangeY, maxRangeY);//振動範囲
                transform.position = new Vector3(x_val, y_val, transform.position.z);//コアの座標で振動

                if(main.LastWave() == true)//最終ウェイブなら
                {
                    GameObject effect = Instantiate(BoomEffect, transform.position, transform.rotation);//爆発エフェクトを生成
                    effect.name = "Boom_effct";
                }
            }
        }
    }

    /// <summary>
    /// ボス撤退処理
    /// </summary>
    void bossBreakUp()
    {
        Shake();//振動処理
        if (time <= 0)//時間になったら
        {
            //ボスを画面上方向に移動
            transform.position = new Vector3(core2Obj.transform.position.x,
                                                      core2Obj.transform.position.y + upspeed,
                                                      core2Obj.transform.position.z);

            if(!isSE)
            {
                Instantiate(se);
                isSE = true;
            }
        }
    }
    
    /// <summary>
    /// Boss死亡処理
    /// </summary>
    void BossDead()
    {
        Shake();//振動処理
        if (time <= 0)//時間になったら
        {
            GameObject effect = Instantiate(BoomEffect, transform.position, transform.rotation);//爆発エフェクト生成
            effect.name = "Boom_effct";
            Destroy(gameObject);//コア破壊
        }
        
    } 
}
