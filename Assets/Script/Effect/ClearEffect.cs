using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEffect : MonoBehaviour
{
    public float speed;

    private RectTransform rect;
    private Vector2 newSize;
    private Vector2 screenSize;
    private Vector2 halfScreenSize;
    private Vector2 size;
    private Vector2 iniPos;

    // Use this for initialization
    void Start()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
        halfScreenSize = new Vector2(screenSize.x / 2, screenSize.y / 2);
        rect = GetComponent<RectTransform>();
        size = Vector2.zero;
        iniPos = rect.localPosition;
        Spread();
    }

    // Update is called once per frame
    void Update()
    {
        rect.sizeDelta += new Vector2(speed, speed);
    }

    /// <summary>
    /// 画像を広げる
    /// </summary>
    private void Spread()
    {
        float dis = Vector2.Distance(iniPos, rect.localPosition);
        float addScaleValue = dis / (screenSize.x / 4);
        float addScale = 2.0f + addScaleValue;
        newSize = new Vector2(screenSize.x * addScale, screenSize.x * addScale);
    }

    /// <summary>
    /// 広がり終わったか
    /// </summary>
    /// <returns></returns>
    public bool IsEnd()
    {
        if (Mathf.Round(rect.sizeDelta.x * 100) / 100 >= (Mathf.Round(newSize.x * 100) / 100)
            && Mathf.Round(rect.sizeDelta.y * 100) / 100 >= (Mathf.Round(newSize.y * 100) / 100))
        {
            return true;
        }
        return false;
    }

}
