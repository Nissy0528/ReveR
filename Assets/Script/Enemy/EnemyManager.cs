﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float scrollSpeed;//移動速度
    public float addBattleTime;//ボスバトル時間に加算する値（最大値）
    public bool isScroll;//下にスクロールするか
    public bool isStopScroll;//途中で停止するか
    public GameObject stopPoint;//停止ポイント

    private int iniChildCnt;//初期の子オブジェクトの数
    private bool isAddTime;//ボスバトル時間を加算するか
    private GameObject lastChild;

    // Use this for initialization
    void Start()
    {
        iniChildCnt = transform.childCount;//初期の子オブジェクトの数取得
        isAddTime = false;//最初はコンボ時間を取らない
    }

    // Update is called once per frame
    void Update()
    {
        Move();//移動
        MoveStop();//停止
        AddBattleTime();//コンボ時間
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
    private void AddBattleTime()
    {
        int currentChildCnt = transform.childCount;//現在の子オブジェクトの数を取得
        GameObject main = GameObject.Find("MainManager");

        //子オブジェクトが減ったら
        if (currentChildCnt < iniChildCnt)
        {
            //ボスバトル時間の減少を始める
            main.GetComponent<Main>().SetIsBattleTime(true);
        }

        //子オブジェクトが1つになったら
        if (currentChildCnt == 1)
        {
            if (lastChild == null)
            {
                lastChild = transform.GetChild(0).gameObject;
            }
        }

        if (lastChild != null)
        {
            Debug.Log(lastChild.GetComponent<Enemy>().IsDead());

            //子オブジェクトのエネミーが画面外に出なければtrueに
            isAddTime = lastChild.GetComponent<Enemy>().IsDead();
        }

        //子オブジェクトがいなくなったら
        if (currentChildCnt == 0 && lastChild == null)
        {
            if (isAddTime)
            {
                addBattleTime = 0;//加算フラグがfalseなら0にする
            }
            main.GetComponent<Main>().SetBattleTime(addBattleTime);//バトル時間加算
            Destroy(gameObject);//消滅
        }
    }
}
