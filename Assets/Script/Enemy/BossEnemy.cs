using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public List<GameObject> childEnemy;　//childEnemy取得
    public GameObject core;//コアオブジェクト
    public GameObject shield;  //シールド取得
    public GameObject core2;//空のコア
    public float addwidthspeed;//ラインの太くなる速さ
    public float stopwidth;//ライン横幅の上限
    public Sprite newBG;//ボス戦で生成する背景

    private Animator animator;//アニメーター
    private BoxCollider2D box; //BoxCollider
    private float width;//ライン横幅

    private bool animcount;//アニメが再生されているか？
    private bool isBossStop;//Bossが止まっているか？


    // Use this for initialization
    void Start()
    {
        box = core.GetComponent<BoxCollider2D>();//BoxCollider2D取得
        animator = shield.GetComponent<Animator>();//Animator取得

        for (int i = 0; i < childEnemy.Count; i++)
        {
            childEnemy[i].GetComponent<Turtroial_Move>().enabled = false;//childEnemyのTurtroial_Moveをfalse
        }
        isBossStop = false;
    }

    // Update is called once per frame
    void Update()
    {
        BossMove();
        childEnemy.RemoveAll(x => x == null);
        if (shield != null)
        {
            animator = shield.GetComponent<Animator>();//shieldのAnimatorを取得
        }
        BossEnemyDead();
        if (childEnemy.Count == 0)//子オブジェクトの個数が0なら
        {
            ShieldAnim();
        }
    }

    /// <summary>
    /// Boss死亡判定
    /// </summary>
    void BossEnemyDead()
    {

        if (ChildEnemyDead() && box!=null) //子オブジェクトが無ければ
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

    void BossMove()
    {
        if (GetComponent<EnemyManager>().IsStop() == true)
        {
            childEnemy.RemoveAll(x => x == null);
            bool isLine = true;
   
            for (int i = 0; i < childEnemy.Count; i++)
            {
                if (childEnemy[i] != null)
                {
                    if (childEnemy[i].GetComponent<Turtroial_Move>() != null)
                    {
                        childEnemy[i].GetComponent<Turtroial_Move>().enabled = true;
                        isLine = false;
                    }
                }
            }

            if (isLine)
            {
                foreach (var t in childEnemy)
                {
                    t.transform.GetChild(0).GetComponent<LineRenderer>().enabled = true;

                    width = width + addwidthspeed;
                    //GetComponent<LineRenderer>().widthMultiplier = width;
                    t.transform.GetChild(0).GetComponent<LineRenderer>().startWidth = width;
                    t.transform.GetChild(0).GetComponent<LineRenderer>().endWidth = width;

                    if (width >= stopwidth)//Lineの横幅が上限を超えたら
                    {
                        t.transform.GetChild(0).GetComponent<LineRenderer>().startWidth = stopwidth;//Lineの横幅上限で停止
                        t.transform.GetChild(0).GetComponent<LineRenderer>().endWidth = stopwidth;//Lineの横幅上限で停止
                        addwidthspeed = 0;//加算を停止
                        GameObject.Find("back3").GetComponent<SpriteRenderer>().sprite = newBG;//ボス戦時背景生成
                        GameObject.Find("back3.3").GetComponent<SpriteRenderer>().sprite = newBG;//ボス戦時背景生成

                        shield.SetActive(true);//shieldのActiveを戻す

                        isBossStop = true;
                    }
                }
            }
        }
    }

    public bool IsBossStop()
    {
        return isBossStop;
    }
}
