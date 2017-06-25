using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTimeEffectSpawner : MonoBehaviour
{
    public GameObject plusEffect;
    public int spawnCnt;

    private GameObject main;
    private Main mainClass;
    private bool isSpawn;
    private float addLifeTime;

    // Use this for initialization
    void Start()
    {
        isSpawn = false;
        addLifeTime = 0.0f;
        main = GameObject.Find("MainManager");
        mainClass = main.GetComponent<Main>();
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
            mainClass.StartAddTime();
            Destroy(gameObject);
        }
    }
}
