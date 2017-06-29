using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogo : MonoBehaviour
{
    public float speed;//速度
    public float delay;

    private RectTransform rect;//レクトトランスフォーム
    private Vector3 iniPos;//初期座標

    // Use this for initialization
    void Start()
    {
        rect = GetComponent<RectTransform>();
        iniPos = rect.anchoredPosition;//初期座標取得
        rect.anchoredPosition = new Vector3(iniPos.x, 150, iniPos.z);//画面外に移動
    }

    // Update is called once per frame
    void Update()
    {
        delay -= Time.deltaTime;
        if (delay > 0.0f) return;

        Move();
        Rotate();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        //初期座標まで移動
        rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, iniPos, speed);
    }

    /// <summary>
    /// 回転
    /// </summary>
    private void Rotate()
    {
        //角度を戻す
        rect.rotation = Quaternion.Lerp(rect.rotation, Quaternion.identity, speed / 2);
    }
}
