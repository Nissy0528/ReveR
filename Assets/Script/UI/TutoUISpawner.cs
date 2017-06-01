using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoUISpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject[] tutorialUIs;
    public GameObject[] enemys;
    public GameObject mainManager;
    public int moveCnt;
    public Vector3 UIPosPlus;

    private Player p_Class;
    private int tutoNum;
    private int cnt = 0;
    private float iniSpeed;
    private float oldSpeed;
    private bool isDamage;

    // Use this for initialization
    void Start()
    {
        tutoNum = 0;
        tutorialUIs[1].SetActive(true);
        tutorialUIs[1].GetComponent<TutorialUI>().SetPosPlus(UIPosPlus);
        p_Class = player.GetComponent<Player>();
        iniSpeed = p_Class.speed;
        oldSpeed = p_Class.speed;
        isDamage = false;

        mainManager.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        MoveTuto();
        DamageTuto();
        ClushTuto();
        SpeedTuto();
    }

    /// <summary>
    /// 移動チュートリアル
    /// </summary>
    private void MoveTuto()
    {
        if (tutoNum != 0)
        {
            tutorialUIs[1].SetActive(false);
            return;
        }

        float vx = Input.GetAxis("Horizontal");
        float vy = Input.GetAxis("Vertical");

        if (vx >= 0.5f || vx <= -0.5f
            && vy >= 0.5f || vy <= -0.5f)
        {
            cnt += 1;
            if (cnt >= moveCnt)
            {
                cnt = 0;
                tutoNum += 1;
            }
        }
    }

    /// <summary>
    /// ダメージチュートリアル
    /// </summary>
    private void DamageTuto()
    {
        if (tutoNum < 4)
        {
            tutorialUIs[0].SetActive(false);
            return;
        }

        if (isDamage)
        {

            tutorialUIs[0].SetActive(true);
            tutorialUIs[0].GetComponent<TutorialUI>().SetPosPlus(UIPosPlus);
            cnt += 1;
            if (cnt >= 120)
            {
                tutorialUIs[0].SetActive(false);
            }

        }
    }

    /// <summary>
    /// 攻撃チュートリアル
    /// </summary>
    private void ClushTuto()
    {
        if (tutoNum < 1 || tutoNum > 3)
        {
            tutorialUIs[2].SetActive(false);
            tutorialUIs[3].SetActive(false);
            return;
        }

        if (tutoNum == 1)
        {
            tutorialUIs[2].SetActive(true);
            tutorialUIs[2].GetComponent<TutorialUI>().SetPosPlus(UIPosPlus);
        }
        if (tutoNum == 2)
        {
            tutorialUIs[2].SetActive(false);
            tutorialUIs[3].SetActive(true);
            tutorialUIs[3].GetComponent<TutorialUI>().SetPosPlus(UIPosPlus);
        }

        enemys[tutoNum - 1].SetActive(true);
        if (enemys[tutoNum - 1].GetComponent<Tutorial>() == null)
        {
            tutoNum += 1;
        }
    }

    /// <summary>
    /// 速度チュートリアル
    /// </summary>
    private void SpeedTuto()
    {
        if (tutoNum < 4)
        {
            p_Class.speed = iniSpeed;
            tutorialUIs[4].SetActive(false);
            return;
        }

        mainManager.SetActive(true);
        if (!tutorialUIs[5].activeSelf)
        {
            tutorialUIs[4].SetActive(true);
            tutorialUIs[4].GetComponent<TutorialUI>().SetPosPlus(UIPosPlus);
        }

        if (p_Class.speed > oldSpeed)
        {
            tutorialUIs[4].SetActive(false);
            tutorialUIs[5].SetActive(true);
            tutorialUIs[5].GetComponent<TutorialUI>().SetPosPlus(UIPosPlus);
        }

        oldSpeed = p_Class.speed;

        enemys[tutoNum - 1].SetActive(true);
        if (enemys[tutoNum - 1].GetComponent<Tutorial>() == null && tutoNum < enemys.Length)
        {
            tutoNum += 1;
        }

        if (tutoNum >= enemys.Length)
        {
            mainManager.GetComponent<Main>().SetIsClear(true);
        }

    }

    /// <summary>
    /// ダメージ判定設定
    /// </summary>
    public void SetIsDamage()
    {
        isDamage = true;
    }

    /// <summary>
    /// チュートリアル番号設定
    /// </summary>
    public void SetTutoNum(int num)
    {
        this.tutoNum = num;
    }

    /// <summary>
    /// チュートリアル番号取得
    /// </summary>
    public int GetTutoNum()
    {
        return tutoNum;
    }
}
