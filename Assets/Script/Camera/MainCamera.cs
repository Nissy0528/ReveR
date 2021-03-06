﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject player;//プレイヤー
    public float shakeTime;//振動時間
    public float range;
    public float shakeDelay;

    private Vector3 playerSize;//プレイヤーのサイズ
    private Vector2 playerPos;//プレイヤーの座標
    private Vector3 backGroundSize;//背景のサイズ
    private Vector3 savePosition;
    private Vector3 screenPosMin;//画面左下の座標
    private Vector3 screenPosMax;//画面右上の座標
    private Camera mainCamera;//カメラ
    private float lifeTime;
    private float minRangeX;
    private float maxRangeX;
    private float minRangeY;
    private float maxRangeY;
    private float delay;

    // Use this for initialization
    void Start()
    {
        mainCamera = GetComponent<Camera>();//カメラ取得
        playerSize = player.transform.localScale;//プレイヤーのサイズ取得
        screenPosMin = mainCamera.ScreenToWorldPoint(Vector3.zero);//画面左下の座標
        screenPosMax = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));//画面右上の座標
        lifeTime = 0.0f;

        savePosition = transform.position;
        minRangeX = savePosition.x - range;
        maxRangeX = savePosition.x + range;
        minRangeY = savePosition.y - range;
        maxRangeY = savePosition.y + range;

        lifeTime = 0.0f;
        delay = shakeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        Clamp();//プレイヤーの移動制限
        Shake();//振動
    }

    /// <summary>
    /// プレイヤーの移動制限
    /// </summary>
    private void Clamp()
    {
        if (player == null) return;

        playerPos = player.transform.position;//プレイヤーの座標

        playerPos.x = Mathf.Clamp(playerPos.x, screenPosMin.x + playerSize.x / 10, screenPosMax.x - playerSize.x / 10);//画面の横幅内に横移動を制限
        playerPos.y = Mathf.Clamp(playerPos.y, screenPosMin.y + playerSize.y / 10, screenPosMax.y - playerSize.y / 10);//画面の縦幅内に縦移動を制限

        player.transform.position = playerPos;//プレイヤーの座標制限
    }

    /// <summary>
    /// 振動
    /// </summary>
    private void Shake()
    {
        if (lifeTime <= 0.0f)
        {
            transform.position = savePosition;
            return;
        }

        lifeTime -= Time.deltaTime;
        if (delay >= shakeDelay && Time.timeScale > 0.0f)
        {
            float x_val = Random.Range(minRangeX, maxRangeX);
            float y_val = Random.Range(minRangeY, maxRangeY);
            transform.position = new Vector3(x_val, y_val, transform.position.z);
            delay = 0.0f;
        }
        if (delay < shakeDelay)
        {
            delay += 1.0f;
        }
    }

    /// <summary>
    /// 振動設定
    /// </summary>
    public void SetShake()
    {
        minRangeX = savePosition.x - range;
        maxRangeX = savePosition.x + range;
        minRangeY = savePosition.y - range;
        maxRangeY = savePosition.y + range;

        lifeTime = shakeTime;
        delay = shakeDelay;
    }

    /// <summary>
    /// 画面の左下
    /// </summary>
    /// <returns></returns>
    public Vector3 GetScreenPosMin()
    {
        return screenPosMin;
    }
    /// <summary>
    /// 画面の右上
    /// </summary>
    /// <returns></returns>
    public Vector3 GetScreenPosMax()
    {
        return screenPosMax;
    }
}
