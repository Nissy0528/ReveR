using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Input : MonoBehaviour
{
    private List<float> delayList;
    private List<float> b_delayList;
    private List<Vector2> inputList;
    private List<bool> b_changeList;
    private int delayNum;
    private int b_delayNum;
    private int inputNum;
    private int b_changeNum;
    private float delay;
    private float b_delay;
    private Vector2 input;
    private bool b_change;

    // Use this for initialization
    void Start()
    {
        //delayList = PlayerPrefsUtility.LoadList<float>("DelayKey");
        //inputListX = PlayerPrefsUtility.LoadList<float>("PastInputXKey");
        //inputListY = PlayerPrefsUtility.LoadList<float>("PastInputYKey");
        delayList = SaveInput.saveDelay;
        inputList = SaveInput.savePastInput;
        b_delayList = SaveInput.b_saveDelay;
        b_changeList = SaveInput.boostChange;

        delayNum = 0;
        inputNum = 0;
        b_delayNum = 0;
        b_changeNum = 0;
        delay = delayList[delayNum];
        input = inputList[inputNum];
        b_delay = b_delayList[b_delayNum];
        b_change = b_changeList[b_changeNum];
    }

    // Update is called once per frame
    void Update()
    {
        Input();
        Boost();
    }

    /// <summary>
    /// スティック入力
    /// </summary>
    private void Input()
    {
        if (delayNum >= delayList.Count - 1 || inputNum >= inputList.Count - 1)
        {
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
        //if (delay > 0.0f) return;

        //delayNum += 1;
        inputNum += 1;
        input = inputList[inputNum];
        //delay = delayList[delayNum];
    }

    /// <summary>
    /// ブースト入力
    /// </summary>
    private void Boost()
    {
        if (b_delayNum >= b_delayList.Count - 1 || b_changeNum >= b_changeList.Count - 1)
        {
            return;
        }

        b_delay = Mathf.Max(b_delay - Time.deltaTime, 0.0f);
        SetBoost();
    }

    /// <summary>
    /// ブースト更新
    /// </summary>
    private void SetBoost()
    {
        //if (b_delay > 0.0f) return;

        //b_delayNum += 1;
        b_changeNum += 1;
        //b_delay = b_delayList[b_delayNum];
        b_change = b_changeList[b_changeNum];
    }

    /// <summary>
    /// 入力値取得
    /// </summary>
    /// <returns></returns>
    public Vector2 GetInput()
    {
        return input;
    }

    /// <summary>
    /// ブーストフラグ取得
    /// </summary>
    /// <returns></returns>
    public bool GetIsBoost()
    {
        return b_change;
    }
}
