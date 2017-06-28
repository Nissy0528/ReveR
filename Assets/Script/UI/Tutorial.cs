using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject[] tutorialUI;
    public GameObject count;
    public GameObject iniBack;
    public GameObject changeBack;
    public float speed;
    public float maxX;
    public float boostTutoTime;

    private GameObject boss;
    private Main main;
    private int currentParsNum;
    private int partsNum;
    private float[] tutoSpeed;
    private float iniLifeTime;
    private bool isBoost;
    private bool isTime;
    private bool isTimeSpawn;

    // Use this for initialization
    void Start()
    {
        boss = transform.FindChild("Boss").gameObject;
        main = GameObject.Find("MainManager").GetComponent<Main>();

        partsNum = boss.GetComponent<BossEnemy>().childEnemy.Count;
        currentParsNum = partsNum;

        speed *= -1.0f;
        tutoSpeed = new float[] { speed, speed, speed };
        iniLifeTime = main.lifeTime;

        isBoost = false;
        isTimeSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        Count();
        ChangeBack();
        InputTutorialMove();
        BoostTutoMove();
        TimeTutoMove();

        foreach (var t in tutorialUI)
        {
            if (t != null)
            {
                return;
            }
        }
        Destroy(this);
    }

    /// <summary>
    /// チュートリアルカウント
    /// </summary>
    private void Count()
    {
        if (boss == null) return;

        //ボスのパーツが一つ減るごとにカウント
        partsNum = boss.GetComponent<BossEnemy>().childEnemy.Count;
        if (partsNum < currentParsNum)
        {
            GameObject cnt = Instantiate(count, GameObject.Find("Canvas").transform);
            cnt.GetComponent<Text>().text = currentParsNum.ToString();
            currentParsNum = partsNum;
        }
    }

    /// <summary>
    /// 背景変更
    /// </summary>
    private void ChangeBack()
    {
        if (boss != null) return;

        //スクロールする背景に変更
        if (iniBack != null)
        {
            Instantiate(changeBack);
            Destroy(iniBack);
        }
    }

    /// <summary>
    /// 切り替えしチュートリアルUIの移動
    /// </summary>
    private void InputTutorialMove()
    {
        if (tutorialUI[0] == null) return;

        Vector2 inputTutoPos = tutorialUI[0].GetComponent<RectTransform>().anchoredPosition;
        inputTutoPos.x += tutoSpeed[0] * Time.timeScale;
        inputTutoPos.x = Mathf.Clamp(inputTutoPos.x, -maxX, maxX);
        tutorialUI[0].GetComponent<RectTransform>().anchoredPosition = inputTutoPos;

        if (boss == null && tutoSpeed[0] < 0.0f)
        {
            tutoSpeed[0] *= -1.0f;
        }
        if (tutoSpeed[0] > 0.0f && inputTutoPos.x >= maxX)
        {
            Destroy(tutorialUI[0]);
        }
    }

    /// <summary>
    /// ブーストチュートリアルUIの移動
    /// </summary>
    private void BoostTutoMove()
    {
        if (tutorialUI[0] != null || tutorialUI[1] == null) return;

        Vector2 boostTutoPos = tutorialUI[1].GetComponent<RectTransform>().anchoredPosition;
        boostTutoPos.x += tutoSpeed[1] * Time.timeScale;
        boostTutoPos.x = Mathf.Clamp(boostTutoPos.x, -maxX, maxX);
        tutorialUI[1].GetComponent<RectTransform>().anchoredPosition = boostTutoPos;

        if (IsBoostUI() && tutoSpeed[1] < 0.0f)
        {
            tutoSpeed[1] *= -1.0f;
        }
        if (tutoSpeed[1] > 0.0f && boostTutoPos.x >= maxX)
        {
            Destroy(tutorialUI[1]);
        }
    }

    /// <summary>
    /// ライフタイムチュートリアルUIの移動
    /// </summary>
    private void TimeTutoMove()
    {
        if (tutorialUI[0] != null || tutorialUI[2] == null) return;

        GameObject timeEffect = GameObject.FindGameObjectWithTag("P_Effect");
        IsTimeUI(timeEffect);

        Vector2 timeTutoPos = tutorialUI[2].GetComponent<RectTransform>().anchoredPosition;
        timeTutoPos.x += tutoSpeed[2] * Time.timeScale;
        timeTutoPos.x = Mathf.Clamp(timeTutoPos.x, -maxX, maxX);
        tutorialUI[2].GetComponent<RectTransform>().anchoredPosition = timeTutoPos;

        if (isTime)
        {
            if (timeTutoPos.x >= maxX && tutoSpeed[2] > 0.0f && timeEffect == null)
            {
                tutoSpeed[2] *= -1.0f;
            }
            if (timeTutoPos.x <= -maxX && tutoSpeed[2] < 0.0f)
            {
                Destroy(tutorialUI[2]);
            }
        }
    }

    /// <summary>
    /// ブーストチュートリアル完了フラグ
    /// </summary>
    /// <returns></returns>
    private bool IsBoostUI()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            isBoost = true;
        }

        if (isBoost)
        {
            boostTutoTime -= Time.deltaTime;
        }

        return boostTutoTime <= 0.0f || isBoost && Input.GetKeyUp(KeyCode.JoystickButton0);
    }

    /// <summary>
    /// ライフタイムチュートリアルフラグ
    /// </summary>
    /// <returns></returns>
    private void IsTimeUI(GameObject timeEffect)
    {
        if (main.GetWave() <= 0) return;

        if (!isTimeSpawn && timeEffect == null)
        {
            isTimeSpawn = true;
        }
        if (isTimeSpawn && timeEffect != null
            && !isTime && tutoSpeed[2] < 0.0f)
        {
            tutoSpeed[2] *= -1.0f;
            isTime = true;
        }
    }
}
