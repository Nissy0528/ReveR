using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTimeEffectSpawner : MonoBehaviour
{
    public GameObject plusEffect;
    public GameObject finishEffect;
    public int spawnCnt;

    private GameObject main;
    private GameObject lifeUI;
    private Main mainClass;
    private Vector3 lifeUIPos;
    private bool isSpawn;
    private float addLifeTime;

    // Use this for initialization
    void Start()
    {
        isSpawn = false;
        main = GameObject.Find("MainManager");
        mainClass = main.GetComponent<Main>();
        lifeUI = GameObject.Find("LifeMeter");
        //ライフタイムの座標をワールド座標に変換
        lifeUIPos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(lifeUI.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        EffectSpawn();
        Dead();
    }

    /// <summary>
    /// エフェクト生成
    /// </summary>
    private void EffectSpawn()
    {
        if (isSpawn) return;

        if (transform.childCount < spawnCnt)
        {
            Instantiate(plusEffect, transform);
        }
        else
        {
            isSpawn = true;
        }
    }

    /// <summary>
    ///　消滅
    /// </summary>
    private void Dead()
    {
        if (!isSpawn) return;

        if (transform.childCount == 0)
        {
            Debug.Log(addLifeTime);
            mainClass.StartTime(addLifeTime, 0);
            SpawnFinishEffect();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ライフタイム加算完了エフェクト生成
    /// </summary>
    private void SpawnFinishEffect()
    {
        GameObject f_effect = Instantiate(finishEffect, lifeUIPos, Quaternion.identity, lifeUI.transform);
        f_effect.GetComponent<Text>().text = mainClass.lifeTime.ToString();
        f_effect.transform.SetSiblingIndex(f_effect.transform.GetSiblingIndex() - 1);
    }

    /// <summary>
    /// 加算ライフタイム設定
    /// </summary>
    /// <param name="addLifeTime"></param>
    public void SetAddTime(float addLifeTime)
    {
        this.addLifeTime = addLifeTime;
    }
}
