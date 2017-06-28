using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInput : MonoBehaviour
{
    public static List<Vector2> savePastInput = new List<Vector2>();//スティック入力値リスト（記録用）
    public static List<float> saveDelay = new List<float>();//スティック入力値が変わるまでの時間リスト（記録用）
    public static List<float> b_saveDelay = new List<float>();//ブースト切り替えまでの時間リスト（記録用）
    public static List<bool> boostChange = new List<bool>();//ブーストフラグリスト（記録用）

    private Vector2 pastInput;//前のスティック入力値
    private Vector2 currentInput;//現在のスティック入力値
    private float changeDelay;//スティック入力値が切り替わるまでの時間
    private float b_changeDelay;//ブーストが切り替わるまでの時間
    private bool isInputSave;//スティック情報を記録するか
    private bool isBoostChange;//ブースト切り替えフラグ
    private bool isBoostSave;//ブースト情報を記録するか

    // Use this for initialization
    void Start()
    {
        pastInput = Vector2.zero;
        changeDelay = 0.0f;
        b_changeDelay = 0.0f;
        isInputSave = false;
        isBoostChange = false;
        isBoostSave = false;
        InputSave();
        BoostSave();

    }

    // Update is called once per frame
    void Update()
    {
        SetSave();//入力の記録を開始
        //InputSave();//入力情報を記録
        SetBoostSave();//ブーストの記録を開始
        //BoostSave();//ブースト情報記録
    }

    /// <summary>
    /// 入力の記録を開始
    /// </summary>
    private void SetSave()
    {
        float x = Input.GetAxis("Horizontal");//横入力
        float y = Input.GetAxis("Vertical");//縦入力

        changeDelay += Time.deltaTime;//カウント加算
        currentInput = new Vector2(x, y);

        InputSave();

        //if (pastInput != currentInput)
        //{
        //    InputSave();//入力情報を記録
        //}
    }

    /// <summary>
    /// ブーストの記録を開始
    /// </summary>
    private void SetBoostSave()
    {
        b_changeDelay += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.JoystickButton0) && !isBoostChange)
        {
            isBoostChange = true;
        }
        if(Input.GetKeyUp(KeyCode.JoystickButton0) && isBoostChange)
        {
            isBoostChange = false;
        }

        BoostSave();
    }

    /// <summary>
    /// 入力情報を記録
    /// </summary>
    private void InputSave()
    {
        //if (!isInputSave) return;

        //入力値が変わるまでの時間を記録
        saveDelay.Add(changeDelay);
        //PlayerPrefsUtility.SavaList<float>("DelayKey", saveDelay);

        //入力値を更新して記録
        savePastInput.Add(currentInput);
        //PlayerPrefsUtility.SavaList<float>("PastInputXKey", savePastInputX);
        //PlayerPrefsUtility.SavaList<float>("PastInputYKey", savePastInputY);

        changeDelay = 0.0f;//カウント初期化
        pastInput = currentInput;
        //isInputSave = false;//記録フラグfalseに
    }

    /// <summary>
    /// ブースト情報記録
    /// </summary>
    private void BoostSave()
    {
        //if (!isBoostSave) return;

        //ブーストが切り替わるまでの時間を記録
        b_saveDelay.Add(b_changeDelay);

        //ブーストフラグを切り替えて記録
        boostChange.Add(isBoostChange);

        //初期化
        b_changeDelay = 0.0f;
        //isBoostChange = !isBoostChange;
    }
}
