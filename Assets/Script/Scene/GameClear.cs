using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public int maxRank;//それぞれのランクの値
    public GameObject[] UI;
    public GameObject[] cursorUI;
    public GameObject[] se;
    public Sprite[] jugeImage;
    public Sprite[] cursorImage;
    public Sprite[] change_Image;
    public GameObject fade;
    public Text S;　　　//Aランク
    public Text A;　　　//bランク
    public Text B;　　　//cランク
    public float delay;//UIを表示する時間

    public GameObject FadeOut;//fadeOut
    private bool IsLoadTitle = false;//
    private bool IsLoaRetry = false;

    private int num;//評価の値
    private int UInum;//表示するUIの番号
    private int Scount = 0;
    private int Acount = 0;
    private int Bcount = 0;
    private int cursorNum;
    private float cnt;//UIを表示するまでのカウント

    // Use this for initialization
    void Start()
    {
        EvaluationText();
        cnt = delay;
        cursorNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        LoadScene();
        CursorMove();
    }

    /// <summary>
    /// 評価を取る
    /// </summary>
    void EvaluationText()
    {
        for (int i = 0; i < Main.Evaluation.Count; i++)
        {
            if (Main.Evaluation[i] == "S") Scount++;
            if (Main.Evaluation[i] == "A") Acount++;
            if (Main.Evaluation[i] == "B") Bcount++;
        }
        S.text = Scount.ToString();
        A.text = Acount.ToString();
        B.text = Bcount.ToString();
    }

    /// <summary>
    /// 評価
    /// </summary>
    private void Juge()
    {
        if (UInum < 2) return;

        num = (3 * Scount + 2 * Acount + Bcount);

        if (num >= maxRank)
        {
            UI[3].GetComponent<Image>().sprite = jugeImage[0];
        }
        if (num < maxRank && num >= maxRank * 2 / 3)
        {
            UI[3].GetComponent<Image>().sprite = jugeImage[1];
        }
        if (num < maxRank * 2 / 3)
        {
            UI[3].GetComponent<Image>().sprite = jugeImage[2];
        }
    }

    /// <summary>
    /// UIをアクティブに
    /// </summary>
    private void UIActive()
    {
        if (UInum > 3 || fade != null) return;

        cnt -= Time.deltaTime;

        if (cnt <= 0.0f)
        {
            if (!UI[UInum].activeSelf)
            {
                UI[UInum].SetActive(true);
            }
            UInum += 1;
            if (UInum < 3)
            {
                cnt = delay;
            }
            else
            {
                cnt = delay * 2.0f;
            }
        }
    }


    void LoadScene()
    {
        if (FadeOut.GetComponent<Fade_Effect>().GetBool())
        {
            if (IsLoadTitle) SceneManager.LoadScene("Title");
            if (IsLoaRetry) SceneManager.LoadScene("Main");

        }
        else
        {
            if (!FadeOut.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    Instantiate(se[1]);
                    Main.Evaluation.Clear();
                    FadeOut.SetActive(true);
                    if (cursorNum == 0)
                    {
                        IsLoadTitle = true;
                    }
                    if (cursorNum == 1)
                    {
                        IsLoaRetry = true;
                    }
                }

                UIActive();
                Juge();
            }
        }
    }

    /// <summary>
    /// カーソル移動
    /// </summary>
    private void CursorMove()
    {
        float x = Input.GetAxis("Horizontal");
        if (x <= -1.0f && cursorNum == 0)
        {
            Instantiate(se[0]);
            cursorNum += 1;
        }
        if (x >= 1.0f && cursorNum == 1)
        {
            Instantiate(se[0]);
            cursorNum -= 1;
        }

        for (int i = 0; i < cursorUI.Length; i++)
        {
            cursorUI[i].GetComponent<Image>().sprite = cursorImage[i];
        }
        cursorUI[cursorNum].GetComponent<Image>().sprite = change_Image[cursorNum];
    }
}
