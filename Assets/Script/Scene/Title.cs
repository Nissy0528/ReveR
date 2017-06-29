using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject[] UI;
    public float delay;

    private int selectionNum = 0;
    private float cnt;

    void Start()
    {
        cnt = delay;
    }

    void Update()
    {
        Cursor.visible = false;

        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            SceneManager.LoadScene("Credit");
        }

        Selection();
        Select();
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
            ui.GetComponent<Text>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        UI[selectionNum].GetComponent<Text>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
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
            SceneManager.LoadScene("Main");
        }
        if(selectionNum==1)
        {
            Application.Quit();
        }
    }
}
