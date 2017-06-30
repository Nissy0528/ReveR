using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public int maxRank;//それぞれのランクの値
    public GameObject[] UI;
    public Sprite[] jugeImage;
    public GameObject fade;
    public Text S;　　　//Aランク
    public Text A;　　　//bランク
    public Text B;　　　//cランク
    public float delay;//UIを表示する時間

    private int num;//評価の値
    private int UInum;//表示するUIの番号
    private int Scount = 0;
    private int Acount = 0;
    private int Bcount = 0;
    private float cnt;//UIを表示するまでのカウント

    // Use this for initialization
    void Start()
    {
        EvaluationText();
        cnt = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Main.Evaluation.Clear();
            SceneManager.LoadScene("Title");
        }

        UIActive();
        Juge();
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
}
