using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float scrollSpeed;//移動速度
    public float addLifeTime;//生存時間に加算する値（最大値）
    public bool isScroll;//下にスクロールするか
    public bool isStopScroll;//途中で停止するか
    public GameObject stopPoint;//停止ポイント

    private int iniChildCnt;//初期の子オブジェクトの数
    private bool isCombo;//コンボ時間を取るか
    private float comboTime;//コンボ時間

    // Use this for initialization
    void Start()
    {
        iniChildCnt = transform.childCount;//初期の子オブジェクトの数取得
        isCombo = false;//最初はコンボ時間を取らない
    }

    // Update is called once per frame
    void Update()
    {
        Move();//移動
        MoveStop();//停止
        ComboTime();//コンボ時間
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (!isScroll) return;//スクロールしなければ何もしない
        transform.Translate(Vector3.down * scrollSpeed);//下にスクロール
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
    /// コンボ時間
    /// </summary>
    private void ComboTime()
    {
        int currentChildCnt = transform.childCount;//現在の子オブジェクトの数を取得
        GameObject main = GameObject.Find("MainManager");

        //子オブジェクトが減ったら
        if (currentChildCnt < iniChildCnt && !isCombo)
        {
            main.GetComponent<Main>().SetIsLifeCnt(true);
            isCombo = true;//コンボ時間を取る
        }

        if (isCombo)
        {
            //子オブジェクトがいなくなったら
            if (currentChildCnt == 0)
            {
                main.GetComponent<Main>().SetLifeTime(addLifeTime);
                Destroy(gameObject);//消滅
            }
        }
    }
}
