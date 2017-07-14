using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTimeEffectSpawner : MonoBehaviour
{
    public GameObject[] effect;
    public GameObject[] finishEffect;
    public GameObject damege;
    public int spawnCnt;

    private GameObject main;
    private GameObject lifeUI;
    private Main mainClass;
    private Vector3 lifeUIPos;
    private bool isSpawn;
    private float time;
    private float currentTime;
    private int type;
    private int f_type;

    // Use this for initialization
    void Start()
    {
        isSpawn = false;
        main = GameObject.Find("MainManager");
        mainClass = main.GetComponent<Main>();
        lifeUI = GameObject.Find("LifeMeter");
        //ライフタイムの座標をワールド座標に変換
        lifeUIPos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(lifeUI.transform.position);

        if (type == 0)
        {
            f_type = 1;
        }
        if (type == 1)
        {
            f_type = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        lifeUIPos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(lifeUI.transform.position);
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
            if (type == 1)
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
            if (type == 0)
            {
                mainClass.StartTime(time);
            }
            if (type == 1)
            {
                time *= -1;
                mainClass.StartTime(time);
            }
            SpawnFinishEffect();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ライフタイム完了エフェクト生成
    /// </summary>
    private void SpawnFinishEffect()
    {
        GameObject ather = GameObject.FindGameObjectWithTag("L_CntEffect");

        if (ather != null)
        {
            Destroy(ather.gameObject);
        }
        if (type == 1)
        {
            GameObject damageObj = Instantiate(damege, lifeUI.transform);
            damageObj.GetComponent<Text>().text = time.ToString();
        }
        GameObject f_effect = Instantiate(finishEffect[type], lifeUI.transform);
        f_effect.GetComponent<L_TimeCntEffect>().SetTime(currentTime, currentTime + time * f_type);
    }

    /// <summary>
    /// 加算ライフタイム設定
    /// </summary>
    /// <param name="addLifeTime"></param>
    public void SetAddTime(float time, float currentTime, int type)
    {
        this.time = time;
        this.currentTime = currentTime;
        this.type = type;
    }
}
