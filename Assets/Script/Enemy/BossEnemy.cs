using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public List<GameObject> childEnemy;　//childEnemy取得
    public GameObject core;//コアオブジェクト
    private BoxCollider2D box; //BoxCollider
    public GameObject shield;  //シールド取得
    private Animator animator;
    private Animation animation;
    private bool animcount;
    private bool isColActive;

    public GameObject core2;

    // Use this for initialization
    void Start()
    {
        box = core.GetComponent<BoxCollider2D>();//BoxCollider2D取得
        animator = shield.GetComponent<Animator>();
        animation = GetComponent<Animation>();

        foreach (var e in childEnemy)
        {
            e.GetComponent<BoxCollider2D>().enabled = false;
        }
        shield.GetComponent<CircleCollider2D>().enabled = false;

        isColActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        ColliderActive();

        childEnemy.RemoveAll(x => x == null);
        if (shield != null)
        {
            animator = shield.GetComponent<Animator>();
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
        var animState = animator.GetCurrentAnimatorStateInfo(0);
        if (animcount == false)
        {
            animator.SetTrigger("shield");
            animcount = true;
        }

        if (animState.IsName("Shield"))
        {
            if (animState.normalizedTime >= 1 && animcount)
            {
                Destroy(shield); //シールド破壊
            }
        }
    }

    /// <summary>
    /// あたり判定をアクティブに
    /// </summary>
    private void ColliderActive()
    {
        if (!GetComponent<EnemyManager>().IsStop() || isColActive) return;

        foreach (var e in childEnemy)
        {
            e.GetComponent<BoxCollider2D>().enabled = true;
        }
        shield.GetComponent<CircleCollider2D>().enabled = true;

        isColActive = true;
    }
}
