using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeScript : MonoBehaviour
{
    RectTransform rt;//RectTransform
    private GameObject main;//playerオブジェクト

    private float speed;
    private Vector2 iniSize;
    private Main mainClass;

    public GameObject gauge;
    public GameObject frame;
    public GameObject needleposition;
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

        gauge = GameObject.Find("speedgauge");
        frame = GameObject.Find("speedframe");
        needleposition = GameObject.Find("NeedlePosition");
    }

    public void Life()
    {
        //speed = Mathf.Abs(player.speed) / player.speedLimit;
        //rt.sizeDelta = new Vector2(iniSize.x, iniSize.y * speed);

        gauge.GetComponent<Image>().fillAmount = (mainClass.battleTime / mainClass.b_TimeMax) * 0.69f;
        //gauge.GetComponent<Image>().fillAmount = Mathf.Clamp((player.speed / player.speedLimit), 0.11f, 0.89f);

    }

    public void LifeColor()
    {
        if (mainClass.battleTime >= sectionTime[2])
        {
            gauge.GetComponent<Image>().color = new Color(0, 1, 0);
            IsGreen = true;
            IsRed = false;
            IsYellow = false;
        }

        else if (mainClass.battleTime >= sectionTime[1])
        {
            gauge.GetComponent<Image>().color = new Color(1, 1, 0);
            IsGreen = false;
            IsRed = false;
            IsYellow = true;
        }

        else if (mainClass.battleTime >= sectionTime[0])
        {
            gauge.GetComponent<Image>().color = new Color(1, 0, 0);
            IsGreen = false;
            IsRed = true ;
            IsYellow = false;
        }
    }

    public void needleCtrl()
    {
        needleposition.transform.rotation = Quaternion.Euler(0, 0, 122.0f - (244.0f * (mainClass.battleTime / mainClass.b_TimeMax)));
    }

    // Update is called once per frame
    void Update()
    {
        Life();
        LifeColor();
        //needleCtrl();
    }
}
