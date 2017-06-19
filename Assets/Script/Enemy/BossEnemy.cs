using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public List<GameObject> childEnemy;　//childEnemy取得
    private BoxCollider2D box; //BoxCollider
    public GameObject shield;  //シールド取得

    // Use this for initialization
    void Start()
    {
        box = GetComponent<BoxCollider2D>();//BoxCollider2D取得
    }

    // Update is called once per frame
    void Update()
    {
        BossEnemyDead();
    }

    /// <summary>
    /// Boss死亡判定
    /// </summary>
    void BossEnemyDead()
    {
        childEnemy.RemoveAll(x => x == null);

        if (ChildEnemyDead()) //子オブジェクトが無ければ
        {
            box.enabled = true; //判定を戻す
        }
        else
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
        if(childEnemy.Count == 0)//子オブジェクトの個数が0なら
        {
            Destroy(shield);　//シールド破壊
            return true;
        }
        else
        {
            return false;
        }
    }
}
