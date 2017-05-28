﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public GameObject player;
    public GameObject[] tutorialUIs;
    public GameObject[] enemys;
    public GameObject mainManager;

    private Player p_Class;
    private int tutoNum;
    private int cnt = 0;
    private float iniHp;
    private float iniSpeed;
    private float oldSpeed;
    private bool isDamage;

    // Use this for initialization
    void Start()
    {
        tutoNum = 0;
        tutorialUIs[tutoNum].SetActive(true);
        p_Class = player.GetComponent<Player>();
        iniHp = p_Class.hp;
        iniSpeed = p_Class.speed;
        oldSpeed = iniSpeed;
        isDamage = false;
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
            tutorialUIs[0].SetActive(false);
            return;
        }

        float vx = Input.GetAxis("Horizontal");
        float vy = Input.GetAxis("Vertical");

        if (vx >= 0.5f || vx <= -0.5f
            && vy >= 0.5f || vy <= -0.5f)
        {
            cnt += 1;
            if (cnt >= 120)
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
        if (tutoNum == 0)
        {
            tutorialUIs[1].SetActive(false);
            return;
        }

        if (p_Class.hp < iniHp && !isDamage)
        {

            tutorialUIs[1].SetActive(true);
            isDamage = true;

        }
        if (isDamage)
        {
            cnt += 1;
            if (cnt >= 60)
            {
                tutorialUIs[1].SetActive(false);
                cnt = 0;
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
        }
        if (tutoNum == 2)
        {
            tutorialUIs[2].SetActive(false);
            tutorialUIs[3].SetActive(true);
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
        }

        if (p_Class.speed > oldSpeed)
        {
            tutorialUIs[4].SetActive(false);
            tutorialUIs[5].SetActive(true);
        }

        oldSpeed = p_Class.speed;

        enemys[tutoNum - 1].SetActive(true);
        if (enemys[tutoNum - 1].GetComponent<Tutorial>() == null && tutoNum < 6)
        {
            tutoNum += 1;
        }

        if (tutoNum >= 6)
        {
            mainManager.GetComponent<Main>().SetIsClear(true);
        }

    }
}
