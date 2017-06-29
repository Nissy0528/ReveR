using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInput : MonoBehaviour
{
    public static List<Vector2> saveInput;//スティック入力値リスト（記録用）
    public static List<float> saveSpeed;//プレイヤーの速度リスト（記録用）

    private Player player;//プレイヤー
    private Vector2 currentInput;//現在のスティック入力値

    // Use this for initialization
    void Start()
    {
        saveInput = new List<Vector2>();
        saveSpeed = new List<float>();

        player = GameObject.Find("Player").GetComponent<Player>();
        InputSave();
        SpeedSave();
    }

    // Update is called once per frame
    void Update()
    {
        InputSave();//入力を記録
        SpeedSave();//プレイヤーの速度を記録
    }

    /// <summary>
    /// 入力を記録
    /// </summary>
    private void InputSave()
    {
        currentInput = GetComponent<InputManager>().GetInput();
        saveInput.Add(currentInput);
    }

    /// <summary>
    /// プレイヤーの速度を記録
    /// </summary>
    private void SpeedSave()
    {
        float playerSpeed = player.speed;

        saveSpeed.Add(playerSpeed);
    }
}
