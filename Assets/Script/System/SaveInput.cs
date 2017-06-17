using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInput : MonoBehaviour
{
    private Vector2 pastInput;//前の入力値
    private Vector2 currentInput;//現在の入力値
    private List<float> savePastInputX = new List<float>();//入力値リスト（記録用）
    private List<float> savePastInputY = new List<float>();//入力値リスト（記録用）
    private List<float> saveDelay = new List<float>();//入力値が変わるまでの時間リスト（記録用）
    private float changeCnt;//入力値が切り替わるまでのカウント
    private float changeDelay;//入力値が切り替わるまでの時間
    private bool isSave;//情報を記録するか

    // Use this for initialization
    void Start()
    {
        pastInput = Vector2.zero;
        changeCnt = 0.0f;
        changeDelay = 0.0f;
        isSave = false;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");//横入力
        float y = Input.GetAxis("Vertical");//縦入力

        changeCnt += Time.deltaTime;//カウント加算
        currentInput = new Vector2(x, y);

        SetSave();//入力の記録を開始
        InputSave();//入力情報を記録
    }

    /// <summary>
    /// 入力の記録を開始
    /// </summary>
    /// <param name="x">横入力</param>
    /// <param name="y">縦入力</param>
    private void SetSave()
    {
        if (pastInput != currentInput && !isSave)
        {
            isSave = true;//入力値が変わったらture
        }
    }

    /// <summary>
    /// 入力情報を記録
    /// </summary>
    private void InputSave()
    {
        if (!isSave) return;

        //入力値が変わるまでの時間を記録
        changeDelay = changeCnt;
        saveDelay.Add(changeDelay);
        PlayerPrefsUtility.SavaList<float>("DelayKey", saveDelay);

        //入力値を更新して記録
        pastInput = currentInput;
        savePastInputX.Add(pastInput.x);
        savePastInputY.Add(pastInput.y);
        PlayerPrefsUtility.SavaList<float>("PastInputXKey", savePastInputX);
        PlayerPrefsUtility.SavaList<float>("PastInputYKey", savePastInputY);

        changeCnt = 0.0f;//カウント初期化
        isSave = false;//記録フラグfalseに
    }
}
