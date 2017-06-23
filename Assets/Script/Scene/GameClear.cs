using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public Text clearTime;
    public Text S;　　　//Aランク
    public Text A;　　　//bランク
    public Text B;　　　//cランク


    public List<Text> timeRank;

    private List<float> times;
    private string saveKey = "TimesKey";

    // Use this for initialization
    void Start()
    {
        //TimeText();
        EvaluationText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Main.Evaluation.Clear();
            SceneManager.LoadScene("Title");
        }
    }

    /// <summary>
    /// 評価を取る
    /// </summary>
    void EvaluationText()
    {
        var Scount = 0;
        var Acount = 0;
        var Bcount = 0;
        for(int i = 0; i < Main.Evaluation.Count; i++)
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
    /// クリア時間表示
    /// </summary>
    private void TimeText()
    {
        clearTime.text = Main.time.ToString();
        times = PlayerPrefsUtility.LoadList<float>(saveKey);
        times.Add(Main.time);
        times.Sort();

        if (times.Count == 4)
        {
            times.RemoveAt(3);
        }
        PlayerPrefsUtility.SavaList<float>(saveKey, times);

        for (int i = 0; i < times.Count; i++)
        {
            timeRank[i].text = times[i].ToString();
        }
    }


}
