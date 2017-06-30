﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public GameObject core;//コアオブジェクト
    public GameObject shield;  //シールド取得

    public GameObject core2;//空のコア
    public float addwidthspeed;//ラインの太くなる速さ
    public float stopwidth;//ライン横幅の上限
    public Sprite newBG;//ボス戦で生成する背景

    public AudioClip[] SE;

    private List<GameObject> childEnemy = new List<GameObject>();//childEnemy取得
    private Animator animator;//アニメーター
    private BoxCollider2D box; //BoxCollider
    private float width;//ライン横幅
    private float width1;
    private float width2;

    private bool animcount;//アニメが再生されているか？
    private bool isBossStop;//Bossが止まっているか？
    private bool isActive;//起動状態フラグ
    private bool isSE;


    private Animation animation;
    private bool isColActive;


    // Use this for initialization
    void Start()
    {
        box = core.GetComponent<BoxCollider2D>();//BoxCollider2D取得

        animator = shield.GetComponent<Animator>();//Animator取得

        childEnemy = GetAllChildren.GetAll(gameObject);

        //敵の種類によってコンポーネントのアクティブを切る
        foreach (var e in childEnemy)
        {
            if (e.GetComponent<Turtroial_Move>() != null)
            {
                e.GetComponent<Turtroial_Move>().enabled = false;
            }

            if (e.GetComponent<BoxCollider2D>() != null)
            {
                e.GetComponent<BoxCollider2D>().enabled = false;
            }
            if (e.GetComponent<Laser>() != null)
            {
                e.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        isBossStop = false;

        animator = shield.GetComponent<Animator>();
        animation = GetComponent<Animation>();
        shield.GetComponent<CircleCollider2D>().enabled = false;

        isColActive = false;
        isActive = false;
        isSE = false;

    }

    // Update is called once per frame
    void Update()
    {

        BossActive();

        //ColliderActive();

        childEnemy.RemoveAll(x => x == null);
        if (shield != null)
        {
            animator = shield.GetComponent<Animator>();//shieldのAnimatorを取得
        }
        BossEnemyDead();

        bool isAnim = true;

        foreach (var e in childEnemy)
        {
            if (e.tag == "Enemy" && e.name != "Core")
            {
                isAnim = false;
            }
        }

        if (isAnim)//子オブジェクトの個数が0なら
        {
            ShieldAnim();
        }

        SetELmove();
    }

    /// <summary>
    /// Boss死亡判定
    /// </summary>
    void BossEnemyDead()
    {
        if (ChildEnemyDead() && box != null) //子オブジェクトが無ければ
        {
            box.enabled = true; //判定を戻す
        }
        else if (!ChildEnemyDead() && box != null)
        {
            box.enabled = false;
        }
    }

    /// <summary>
    /// 子オブジェクト死亡判定
    /// </summary>
    /// <returns></returns>
    public bool ChildEnemyDead()
    {
        if (shield == null)//子オブジェクトの個数が0なら
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// シールド消滅アニメーション開始
    /// </summary>
    private void ShieldAnim()
    {
        if (shield == null)
            return;
        var animState = animator.GetCurrentAnimatorStateInfo(0);//Animationの最初を取得
        if (animcount == false)
        {
            animator.SetTrigger("shield");
            AudioSource se = GetComponent<AudioSource>();
            se.PlayOneShot(SE[1]);
            animcount = true;
        }

        if (animState.IsName("Shield"))
        {
            if (animState.normalizedTime >= 1 && animcount)//Animationが終わったら　&&　animcountがtrueなら
            {
                Destroy(shield); //シールド破壊
            }
        }
    }


    void BossActive()
    {
        if (GetComponent<EnemyManager>().IsStop() == true)
        {
            childEnemy.RemoveAll(x => x == null);
            bool isLine = true;

            foreach (var e in childEnemy)
            {
                {
                    if (e.GetComponent<Turtroial_Move>() != null)
                    {
                        e.GetComponent<Turtroial_Move>().enabled = true;
                        isLine = false;
                    }
                }
            }

            if (isLine)
            {
                PlaySE();
                LaserActive();
                BulletActive();
                foreach (var t in childEnemy)
                {
                    if (t.GetComponent<LineRenderer>() != null)
                    {
                        width = width + addwidthspeed;

                        t.GetComponent<LineRenderer>().enabled = true;

                        //GetComponent<LineRenderer>().widthMultiplier = width;
                        t.GetComponent<LineRenderer>().startWidth = width;
                        t.GetComponent<LineRenderer>().endWidth = width;

                        if (width >= stopwidth)//Lineの横幅が上限を超えたら
                        {
                            t.GetComponent<LineRenderer>().startWidth = stopwidth;//Lineの横幅上限で停止
                            t.GetComponent<LineRenderer>().endWidth = stopwidth;//Lineの横幅上限で停止
                            //addwidthspeed = 0;//加算を停止

                            if (GameObject.Find("MainManager").GetComponent<Main>().GetWave() > 0)
                            {
                                GameObject.Find("back3").GetComponent<SpriteRenderer>().sprite = newBG;//ボス戦時背景生成
                                GameObject.Find("back3.3").GetComponent<SpriteRenderer>().sprite = newBG;//ボス戦時背景生成
                            }

                            shield.SetActive(true);//shieldのActiveを戻す
                            ColliderActive();

                            isBossStop = true;
                            isActive = true;
                        }
                    }
                }
            }
        }
    }

    public bool IsBossStop()
    {
        return isBossStop;
    }
    /// <summary>
    /// childrenのELmoveを設定する
    /// </summary>
    private void SetELmove()
    {
        if (!GetComponent<EnemyManager>().GetIsScroll())
        {
            foreach (var e in childEnemy)
            {
                if (e.GetComponent<ELMove>() != null && IsELMove())
                {
                    e.GetComponent<ELMove>().enabled = true;
                    e.GetComponent<ELMove>().GetStartPosition(e.transform.localPosition);
                }
            }
        }
    }

    /// <summary>
    /// 広がっている敵がいるか
    /// </summary>
    /// <returns>一体でもいたらtrue</returns>
    private bool IsELMove()
    {
        bool isELMove = true;
        foreach (var e in childEnemy)
        {
            if (e.GetComponent<Turtroial_Move>() != null)
            {
                isELMove = false;
            }
        }
        return isELMove;
    }

    /// <summary>
    /// あたり判定をアクティブに
    /// </summary>
    private void ColliderActive()
    {
        //if (!GetComponent<EnemyManager>().IsStop() || isColActive) return;

        foreach (var e in childEnemy)
        {
            if (e.GetComponent<BoxCollider2D>() != null)
            {
                e.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        shield.GetComponent<CircleCollider2D>().enabled = true;

        //isColActive = true;
    }

    /// <summary>
    /// レーザーをアクティブに
    /// </summary>
    private void LaserActive()
    {
        foreach (var e in childEnemy)
        {
            if (e.GetComponent<Laser>() != null)
            {
                e.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    /// <summary>
    /// バレットをアクティブ
    /// </summary>
    private void BulletActive()
    {
        foreach (var e in childEnemy)
        {
            if (e.GetComponent<ShootBullet>() != null)
            {
                e.GetComponent<ShootBullet>().enabled = true;
            }
        }
    }

    /// <summary>
    /// 起動効果音再生
    /// </summary>
    private void PlaySE()
    {
        if (isSE) return;

        AudioSource se = GetComponent<AudioSource>();
        se.PlayOneShot(SE[0]);

        isSE = true;
    }

    /// <summary>
    /// 子の敵を取得
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetChildEnemy()
    {
        List<GameObject> enemys = new List<GameObject>();
        childEnemy.RemoveAll(x => x == null);

        foreach (var e in childEnemy)
        {
            if (e.tag == "Enemy" && e.name != "Core")
            {
                enemys.Add(e);
            }
        }
        return enemys;
    }

    /// <summary>
    /// 起動フラグ取得
    /// </summary>
    /// <returns></returns>
    public bool IsActive()
    {
        return isActive;
    }
}
