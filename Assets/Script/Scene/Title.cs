using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject[] UI;
    public float delay;

    public Sprite GameStart1;
    public Sprite GameStart2;
    public Sprite GameEnd1;
    public Sprite GameEnd2;
    public GameObject FadeOut;
    public GameObject FadeIn;


    private int selectionNum = 0;
    private float cnt;

    private bool IsLoadCredit = false;
    private bool IsLoadMain = false;
    private bool IsLoadEnd = false;

    void Start()
    {
        cnt = delay;
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
        if (y >= 1.0f || y <= -1.0f)
        {
            cnt -= Time.deltaTime;
            if (cnt <= 0.0f)
            {
                selectionNum += (int)y;
                cnt = delay;
            }
        }
        else
        {
            if (cnt < delay)
            {
                selectionNum = Mathf.Abs(selectionNum - 1);
                cnt = delay;
            }
        }
        selectionNum = Mathf.Abs(selectionNum % 2);

        //選択されたUIの色を赤に（それ以外は白に）
        foreach (var ui in UI)
        {
            UI[0].GetComponent<Image>().sprite = GameStart1;
            UI[1].GetComponent<Image>().sprite = GameEnd1;
        }
        if (selectionNum == 0) { UI[selectionNum].GetComponent<Image>().sprite = GameStart2; }
        else if(selectionNum == 1) { UI[selectionNum].GetComponent<Image>().sprite = GameEnd2; }
    }

    /// <summary>
    /// 決定
    /// </summary>
    private void Select()
    {
        if (!Input.GetKeyDown(KeyCode.JoystickButton0)) return;

        //選択されたUIによってシーン移行
        if(selectionNum==0)
        {
            FadeOut.gameObject.SetActive(true);
            IsLoadMain = true;
        }
        if(selectionNum==1)
        {
            FadeOut.gameObject.SetActive(true);
            IsLoadEnd = true;
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
