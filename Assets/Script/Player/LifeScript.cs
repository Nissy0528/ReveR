using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeScript : MonoBehaviour
{
    RectTransform rt;//RectTransform
    private GameObject main;//playerオブジェクト
    private GameObject pinchObj;

    private float speed;
    private Vector2 iniSize;
    private Main mainClass;

    //全てのエネミーを取る
    private float EnemyCount = 0;
    private GameObject[] EnemyS;
    public float MeterMoveSpeed = 10;

    public GameObject gauge;
    public GameObject frame;
    public GameObject needleposition;
    public GameObject pinchEffect;
    public float[] sectionTime;

    //プレイヤーの色を変更するための条件
    public static bool IsRed = false;
    public static bool IsYellow = true;
    public static bool IsGreen = true;

    // Use this for initializatio
    void Start()
    {
        //rt = GetComponent<RectTransform>();//RectTransformを取得
        //iniSize = rt.sizeDelta;

        main = GameObject.Find("MainManager");//playerオブジェクトを取得
        mainClass = main.GetComponent<Main>();
        //speed = Mathf.Abs(player.speed) / player.speedLimit;

        needleposition = GameObject.Find("NeedlePosition");

        EnemyCount = GetAllChildren.GetAll(mainClass.enemyWave[0]).Count;

        //EnemyS = GameObject.FindGameObjectsWithTag("EnemyManager");
        //for (int i = 0; i < EnemyS.Length; i++)
        //    EnemyCount += EnemyS[i].transform.childCount;
    }

    public void Life()
    {
        //speed = Mathf.Abs(player.speed) / player.speedLimit;
        //rt.sizeDelta = new Vector2(iniSize.x, iniSize.y * speed);

        gauge.GetComponent<Image>().fillAmount = Mathf.Lerp(gauge.GetComponent<Image>().fillAmount, (mainClass.lifeTime / mainClass.lifeTimeMax) * 0.69f, 0.25f);
        //gauge.GetComponent<Image>().fillAmount = Mathf.Clamp((player.speed / player.speedLimit), 0.11f, 0.89f);

    }

    public void LifeColor()
    {
        if (mainClass.lifeTime >= sectionTime[2])
        {
            gauge.GetComponent<Image>().color = new Color(0, 1, 0, Main.lifeAlpha);
            IsGreen = true;
            IsRed = false;
            IsYellow = false;
        }

        else if (mainClass.lifeTime >= sectionTime[1])
        {
            gauge.GetComponent<Image>().color = new Color(1, 1, 0, Main.lifeAlpha);
            IsGreen = false;
            IsRed = false;
            IsYellow = true;
        }

        else if (mainClass.lifeTime >= sectionTime[0])
        {
            gauge.GetComponent<Image>().color = new Color(1, 0, 0, Main.lifeAlpha);
            IsGreen = false;
            IsRed = true;
            IsYellow = false;
        }

    }

    public void needleCtrl()
    {
        needleposition.transform.rotation = Quaternion.Euler(0, 0, 122.0f - (244.0f * (mainClass.lifeTime / mainClass.lifeTimeMax)));
    }

    // Update is called once per frame
    void Update()
    {
        Life();
        LifeColor();
        PinchEffect();
        //needleCtrl();
        LifeMeterMove();//Meterの移動
    }

    /// <summary>
    /// ピンチエフェクト生成
    /// </summary>
    private void PinchEffect()
    {
        if (pinchObj == null && mainClass.lifeTime < sectionTime[1])
        {
            pinchObj = Instantiate(pinchEffect, GameObject.Find("Canvas").transform);
        }
        if (pinchObj != null && mainClass.lifeTime >= sectionTime[1])
        {
            Destroy(pinchObj);
        }
    }
    /// <summary>
    ///　LifeMeterの移動
    /// </summary>
    private void LifeMeterMove()
    {
        var CurrentEnenmyCount = GetAllChildren.GetAll(mainClass.enemyWave[0]).Count;
        //for (int i = 0; i < EnemyS.Length; i++)
        //{
        //    if (EnemyS[i] != null)
        //        CurrentEnenmyCount += EnemyS[i].transform.childCount;
        //}

        var ACPO = transform.parent.GetComponent<RectTransform>().anchoredPosition;

        if ((EnemyCount > CurrentEnenmyCount && ACPO.x < 150) || (Player.isDamage && ACPO.x < 150))
            transform.parent.GetComponent<RectTransform>().anchoredPosition += new Vector2(1, 0) * MeterMoveSpeed;

    }
}
