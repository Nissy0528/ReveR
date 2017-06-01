using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public GameObject player;//プレイヤー

    private RectTransform rect;//レクトトランスフォーム
    private Vector3 posPlus;//加算する座標

    // Use this for initialization
    void Start()
    {
        rect = GetComponent<RectTransform>();
        //プレイヤーの座標から設定した座標分加算してスクリーン座標に変換
        rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player.transform.position + posPlus);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 加算する座標設定
    /// </summary>
    /// <param name="posPlus">加算する座標</param>
    public void SetPosPlus(Vector3 posPlus)
    {
        this.posPlus = posPlus;
    }
}
