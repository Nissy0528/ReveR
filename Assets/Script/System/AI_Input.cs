using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Input : MonoBehaviour
{
    private List<float> delayList;
    private List<float> inputListX;
    private List<float> inputListY;
    private int delayNum;
    private int inputNum;
    private float delay;
    private Vector2 input;

    // Use this for initialization
    void Start()
    {
        delayList = PlayerPrefsUtility.LoadList<float>("DelayKey");
        inputListX = PlayerPrefsUtility.LoadList<float>("PastInputXKey");
        inputListY = PlayerPrefsUtility.LoadList<float>("PastInputYKey");

        delayNum = 0;
        inputNum = 0;
        delay = delayList[delayNum];
        input = new Vector2(inputListX[inputNum], inputListY[inputNum]);
    }

    // Update is called once per frame
    void Update()
    {
        if (delayNum >= delayList.Count - 1 || inputNum >= inputListX.Count - 1|| inputNum >= inputListY.Count - 1)
        {
            delayNum = 0;
            inputNum = 0;
            return;
        }

        delay = Mathf.Max(delay - Time.deltaTime, 0.0f);
        SetInput();
    }

    /// <summary>
    /// 入力値更新
    /// </summary>
    private void SetInput()
    {
        if (delay > 0.0f) return;

        delayNum += 1;
        inputNum += 1;
        input = new Vector2(inputListX[inputNum], inputListY[inputNum]);
        delay = delayList[delayNum];
    }

    /// <summary>
    /// 入力値取得
    /// </summary>
    /// <returns></returns>
    public Vector2 GetInput()
    {
        return input;
    }
}
