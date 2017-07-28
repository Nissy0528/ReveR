using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public int maxRank;//それぞれのランクの値
    public Text MyClearTime;
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


    public List<GameObject> rangku;


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

    public static List<Vector3> Ltext = new List<Vector3>();
    public static List<float> Lnum = new List<float>();
    public static List<float> ClearTime = new List<float>();



    //選択の点滅処理
    public GameObject NewText;
    private int time = 15;
    private int timeSpeed = 1;
    private bool IsAlpha = false;
    private List<Vector3> Newtext = new List<Vector3>();
    private float ColorH = 0;

    // Use this for initialization
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        Newtext.Add(new Vector3(412, -10));
        Newtext.Add(new Vector3(412, -80));
        Newtext.Add(new Vector3(412, -150));
        EvaluationText();
        cnt = delay;
        cursorNum = 0;

       
    }

    // Update is called once per frame
    void Update()
    {
        LoadScene();
        CursorMove();
        NewTextColor();
        UIActive();
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

        MyClearTime.text = Main.ClearTime.ToString("F2");

        Rangku2(Scount, Acount, Bcount);
    }

    /// <summary>
    /// ランク
    /// </summary>
    /// <param name="s">Scount</param>
    /// <param name="a">Acount</param>
    /// <param name="b">Bcount</param>
    void Rangku(int s, int a, int b)
    {
        int Vnum = (3 * s + 2 * a + b);
        Dictionary<string, string> text = new Dictionary<string, string>();
        List<int> Num = new List<int>();


        if (Lnum.Count == 0)
        {
            Lnum.Add(Vnum);
            Ltext.Add(new Vector3(s, a, b));
            ClearTime.Add(Main.ClearTime);
            Text[] t = rangku[0].GetComponentsInChildren<Text>();
            t[1].text = Ltext[0].x.ToString();
            t[2].text = Ltext[0].y.ToString();
            t[3].text = Ltext[0].z.ToString();
            t[4].text = ClearTime[0].ToString("F2");
            return;
        }

        int LC = Lnum.Count;
        for (int i = 0; i < LC; i++)
        {
            if (Lnum[i] <= Vnum)
            {
                Lnum.Insert(i, Vnum);
                Ltext.Insert(i, new Vector3(s, a, b));
                ClearTime.Insert(i, Main.ClearTime);
            }

            if (i == Lnum.Count - 1)
            {
                Lnum.Add(Vnum);
                Ltext.Add(new Vector3(s, a, b));
                ClearTime.Add(Main.ClearTime);
                break;
            }
        }

        if (Lnum.Count > 3)
        {
            Lnum.RemoveAt(3);
            Ltext.RemoveAt(3);
            ClearTime.RemoveAt(3);
        }

        for (int i = 0; i < Ltext.Count; i++)
        {
            Text[] t = rangku[i].GetComponentsInChildren<Text>();
            t[1].text = Ltext[i].x.ToString();
            t[2].text = Ltext[i].y.ToString();
            t[3].text = Ltext[i].z.ToString();
            t[4].text = ClearTime[i].ToString("F2");
        }

    }

    /// <summary>
    /// ランク
    /// </summary>
    /// <param name="s">Scount</param>
    /// <param name="a">Acount</param>
    /// <param name="b">Bcount</param>
    void Rangku2(int s, int a, int b)
    {
        float Vnum = (3 * s + 2 * a + b);
        Dictionary<float, string> text = new Dictionary<float, string>();
        List<float> Num = new List<float>();
        Dictionary<float, float> ClearTime = new Dictionary<float, float>();

        var Text = s.ToString() + " " + a.ToString() + " " + b.ToString() + " " + Main.ClearTime.ToString("F2");

        if (!PlayerPrefs.HasKey("Rank0"))
        {
            PlayerPrefs.SetString("Rank0", Text);
            PlayerPrefs.SetFloat("NumRank0", Vnum);
            PlayerPrefs.SetFloat("RankTime0",float.Parse(Main.ClearTime.ToString("F2")));
            PlayerPrefs.Save();

            NewText.SetActive(true);
            NewText.GetComponent<RectTransform>().localPosition = Newtext[0];

            string[] f = Text.Split(' ');
            Text[] t = rangku[0].GetComponentsInChildren<Text>();
            t[1].text = f[0];
            t[2].text = f[1];
            t[3].text = f[2];
            t[4].text = f[3];

            return;
        }

        int x = 1;
        for (int i = 0; i < x; i++)
        {
            if (PlayerPrefs.HasKey("Rank" + i.ToString()))
            {
                text.Add(PlayerPrefs.GetFloat("NumRank" + i.ToString()), PlayerPrefs.GetString("Rank" + i.ToString()));
                Num.Add(PlayerPrefs.GetFloat("NumRank" + i.ToString()));
                ClearTime.Add(Num[i], PlayerPrefs.GetFloat("RankTime" + i.ToString()));
                x++;
            }
            else
            {
                while (true)
                {
                    if (text.ContainsKey(Vnum)) Vnum += 0.1f;
                    else break;
                }

                text.Add(Vnum, Text);
                Num.Add(Vnum);
                ClearTime.Add(Vnum, Main.ClearTime);
                break;
            }
        }

        Num.Sort((j, k) => (int)(k - j));
        int Count = 0;
        while (true)
        {
            for (int i = Count + 1; i < Num.Count; i++)
            {
                int j = (int)Num[Count] - (int)Num[i];
                if (j == 0)
                {
                    float k = ClearTime[Num[Count]] - ClearTime[Num[i]];
                    if (k >= 0)
                    {
                        float l = Num[Count];
                        Num[Count] = Num[i];
                        Num[i] = l;
                    }
                }
            }
            Count++;
            if (Count == Num.Count - 1) break;
        }

        if (Num.Count > 3) Num.RemoveAt(3);

        for (int i = 0; i < Num.Count; i++)
        {
            if (Num[i] == Vnum)
            {
                NewText.SetActive(true);
                NewText.GetComponent<RectTransform>().localPosition = Newtext[i];
            }
            PlayerPrefs.SetFloat("NumRank" + i.ToString(), Num[i]);
            PlayerPrefs.SetString("Rank" + i.ToString(), text[Num[i]]);
            PlayerPrefs.SetFloat("RankTime"+i.ToString(), ClearTime[Num[i]]);
            PlayerPrefs.Save();

            string[] f = text[Num[i]].Split(' ');

            Text[] t = rangku[i].GetComponentsInChildren<Text>();
            t[1].text = f[0];
            t[2].text = f[1];
            t[3].text = f[2];
            t[4].text = f[3];

        }
    }

    void NewTextColor()
    {
        ColorH += 2f / 255f;
        float V = Random.Range(0.8f, 1f);
        float S = Random.Range(0.5f, 1f);
        NewText.GetComponent<Text>().color = Color.HSVToRGB(ColorH, S, V);
        if (ColorH >= 1) ColorH = 0;

    }

    /// <summary>
    /// 評価
    /// </summary>
    private void Juge()
    {
        if (UInum < 4) return;

        num = (3 * Scount + 2 * Acount + Bcount);

        if (num >= maxRank)
        {
            UI[4].GetComponent<Image>().sprite = jugeImage[0];
        }
        if (num < maxRank && num >= maxRank * 2 / 3)
        {
            UI[4].GetComponent<Image>().sprite = jugeImage[1];
        }
        if (num < maxRank * 2 / 3)
        {
            UI[4].GetComponent<Image>().sprite = jugeImage[2];
        }
    }

    /// <summary>
    /// UIをアクティブに
    /// </summary>
    private void UIActive()
    {
        if (UInum > UI.Length - 1 || fade != null) return;

        cnt -= Time.deltaTime;

        if (cnt <= 0.0f)
        {
            if (!UI[UInum].activeSelf)
            {
                UI[UInum].SetActive(true);
            }
            UInum += 1;
            if (UInum < UI.Length - 1)
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
            Main.ClearTime = 0;
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
                        Main.waveNum = 0;
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
        //cursorUI[cursorNum].GetComponent<Image>().sprite = change_Image[cursorNum];
        SelectAlpha();
    }
    private void SelectAlpha()
    {

        if (time == 15) IsAlpha = false;
        if (time == 0) IsAlpha = true;

        time += timeSpeed;

        if (IsAlpha)
        {
            timeSpeed = 1;
            cursorUI[cursorNum].GetComponent<Image>().sprite = cursorImage[cursorNum];
        }
        else
        {
            timeSpeed = -1;
            cursorUI[cursorNum].GetComponent<Image>().sprite = change_Image[cursorNum];
        }


    }
}
