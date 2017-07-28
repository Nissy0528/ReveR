using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject[] UI;//UIオブジェクト
    public GameObject[] se;//効果音
    public Sprite[] normalUI;//選択時の画像
    public Sprite[] changeUI;//選択時の画像

    public GameObject FadeOut;//フェードアウト
    public GameObject FadeIn;//フェードイン
    public GameObject GameEndManager;//ゲーム終了オブジェクト


    private int selectionNum = 0;//カーソル選択番号
    private bool isStick = false;//スティック入力フラグ

    private bool IsLoadCredit = false;//クレジットシーンフラグ
    private bool IsLoadMain = false;//メインシーンフラグ
    private bool IsLoadEnd = false;//ゲーム終了フラグ

    //選択の点滅処理
    private int time = 15;
    private int timeSpeed = 1;
    private bool IsAlpha = false;
    void Start()
    {
        //ゲーム強制終了のオブジェクト検索
        GameObject[] end = GameObject.FindGameObjectsWithTag("GameEnd");

        //強制終了用のオブジェクトが存在していたら生成しない
        if (end.Length < 1)
        {
            Instantiate(GameEndManager);
        }

        Main.waveNum = 0;
    }

    void Update()
    {
        Cursor.visible = false;
        LoadScene();

    }

    /// <summary>
    /// 項目選択
    /// </summary>
    private void Selection()
    {
        float y = Input.GetAxis("Vertical");

        //スティック入力で上下選択
        if (!isStick)
        {
            if (y >= 0.5f)
            {
                selectionNum -= 1;
                Instantiate(se[2]);
                isStick = true;
            }
            if (y <= -0.5f)
            {
                selectionNum += 1;
                Instantiate(se[2]);
                isStick = true;
            }
        }
        if (y == 0.0f && isStick)
        {
            isStick = false;
        }
        if (selectionNum < 0)
        {
            selectionNum -= 1;
        }
        selectionNum = Mathf.Abs(selectionNum % 3);

        //選択されたUIの画像変更（それ以外は普通に）
        for (int i = 0; i < UI.Length; i++)
        {
            UI[i].GetComponent<Image>().sprite = normalUI[i];
        }
        //UI[selectionNum].GetComponent<Image>().sprite = changeUI[selectionNum];

        SelectAlpha();
       
    }

    /// <summary>
    /// 決定
    /// </summary>
    private void Select()
    {
        if (!Input.GetKeyDown(KeyCode.JoystickButton0)) return;

        //選択されたUIによってシーン移行
        if (selectionNum == 0)
        {
            IsLoadMain = true;
        }
        if (selectionNum == 1)
        {
            IsLoadEnd = true;
        }
        if (selectionNum == 2)
        {
            IsLoadCredit= true;
        }
        Instantiate(se[2]);
        FadeOut.gameObject.SetActive(true);
    }
    /// <summary>
    /// 点滅処理
    /// </summary>
    private void SelectAlpha()
    {
        
        if (time == 15) IsAlpha = false;
        if (time == 0) IsAlpha = true;

        time += timeSpeed;

        if (IsAlpha)
        {
            timeSpeed = 1;
            UI[selectionNum].GetComponent<Image>().sprite = normalUI[selectionNum];
        }
        else
        {
            timeSpeed = -1;
            UI[selectionNum].GetComponent<Image>().sprite = changeUI[selectionNum];
        }

       

       
    }

    private void LoadScene()
    {
        if (FadeOut.GetComponent<Fade_Effect>().GetBool())
        {
            if (IsLoadCredit) SceneManager.LoadScene("Credit");
            if (IsLoadMain) SceneManager.LoadScene("Main");
            if (IsLoadEnd) Application.Quit();

        }
        else
        {
            if (!FadeOut.activeSelf && FadeIn.GetComponent<Fade_Effect>().GetBool())
            {

                if (Input.GetKeyDown(KeyCode.JoystickButton7))
                {
                    FadeOut.SetActive(true);
                    IsLoadCredit = true;
                }

                Selection();
                Select();
            }
        }
    }

}
