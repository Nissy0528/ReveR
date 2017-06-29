using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Input : MonoBehaviour
{
    private List<Vector2> inputList;
    private List<float> speedList;
    private int inputNum;
    private int speedNum;
    private Vector2 input;
    private float speed;

    // Use this for initialization
    void Start()
    {
        inputList = SaveInput.saveInput;
        speedList = SaveInput.saveSpeed;

        inputNum = 0;
        speedNum = 0;

        input = inputList[inputNum];
        speed = speedList[speedNum];
    }

    // Update is called once per frame
    void Update()
    {
        Input();
        Speed();
    }

    /// <summary>
    /// スティック入力
    /// </summary>
    private void Input()
    {
        if (inputNum >= inputList.Count - 1) return;

        inputNum += 1;
        input = inputList[inputNum];
    }

    /// <summary>
    /// ブースト入力
    /// </summary>
    private void Speed()
    {
        if (speedNum >= speedList.Count - 1) return;
        speedNum += 1;
        speed = speedList[speedNum];
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
    public float GetSpeed()
    {
        return speed;
    }
}
