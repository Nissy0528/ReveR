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
    private List<float> tutoSpeed = new List<float>();
    private float iniLifeTime;
    private bool isBoost;
    private bool isTime;
    private bool isTimeSpawn;

    // Use this for initialization
    void Start()
    {
        boss = transform.FindChild("Boss").gameObject;
        main = GameObject.Find("MainManager").GetComponent<Main>();

        partsNum = boss.GetComponent<BossEnemy>().GetChildEnemy().Count;
        currentParsNum = boss.GetComponent<BossEnemy>().GetChildEnemy().Count;

        speed *= 1.0f;
        tutoSpeed.Add(speed);
        tutoSpeed.Add(speed);
        tutoSpeed.Add(-speed);
        iniLifeTime = main.lifeTime;

        isBoost = false;
        isTimeSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentParsNum == 0 && boss != null)
        {
            currentParsNum = boss.GetComponent<BossEnemy>().GetChildEnemy().Count;
        }

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
        partsNum = boss.GetComponent<BossEnemy>().GetChildEnemy().Count;
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

        Color color = tutorialUI[0].GetComponent<Image>().color;
        color.a += tutoSpeed[0];
        color.a = Mathf.Clamp(color.a, 0.0f, 1.0f);
        tutorialUI[0].GetComponent<Image>().color = color;
        tutorialUI[0].transform.GetChild(0).GetComponent<Image>().color = color;

        if (boss == null && tutoSpeed[0] > 0.0f)
        {
            tutoSpeed[0] *= -1.0f;
        }
        if (tutoSpeed[0] < 0.0f && color.a <= 0.0f)
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

        Color color = tutorialUI[1].GetComponent<Image>().color;
        color.a += tutoSpeed[1];
        color.a = Mathf.Clamp(color.a, 0.0f, 1.0f);
        tutorialUI[1].GetComponent<Image>().color = color;

        if (IsBoostUI() && tutoSpeed[1] > 0.0f)
        {
            tutoSpeed[1] *= -1.0f;
        }
        if (tutoSpeed[1] < 0.0f && color.a <= 0.0f)
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

        Color color = tutorialUI[2].GetComponent<Image>().color;
        color.a += tutoSpeed[2];
        color.a = Mathf.Clamp(color.a, 0.0f, 1.0f);
        tutorialUI[2].GetComponent<Image>().color = color;

        if (isTime)
        {
            if (color.a >= 1.0f && tutoSpeed[2] > 0.0f && timeEffect == null)
            {
                tutoSpeed[2] *= -1.0f;
            }
            if (color.a <= 0.0f && tutoSpeed[2] < 0.0f)
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
