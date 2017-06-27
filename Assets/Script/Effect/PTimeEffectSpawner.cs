using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTimeEffectSpawner : MonoBehaviour
{
    public GameObject[] effect;
    public GameObject[] finishEffect;
    public int spawnCnt;

    private GameObject main;
    private GameObject lifeUI;
    private Main mainClass;
    private Vector3 lifeUIPos;
    private bool isSpawn;
    private float time;
    private int type;

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
            GameObject e = Instantiate(effect[type], transform);
            if(type==1)
            {
                e.GetComponent<PlusTimeEffect>().angle += 20f;
            }
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
            mainClass.StartTime(time, type);
            SpawnFinishEffect();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ライフタイム完了エフェクト生成
    /// </summary>
    private void SpawnFinishEffect()
    {
        if (type == 0)
        {
            GameObject f_effect = Instantiate(finishEffect[type], lifeUIPos, Quaternion.identity, lifeUI.transform);
            f_effect.GetComponent<Text>().text = mainClass.lifeTime.ToString();
            f_effect.transform.SetSiblingIndex(f_effect.transform.GetSiblingIndex() - 1);
        }
        if (type == 1)
        {
            Instantiate(finishEffect[type], GameObject.Find("LifeMeter").transform);
        }
    }

    /// <summary>
    /// 加算ライフタイム設定
    /// </summary>
    /// <param name="addLifeTime"></param>
    public void SetAddTime(float time, int type)
    {
        this.time = time;
        this.type = type;
    }
}
