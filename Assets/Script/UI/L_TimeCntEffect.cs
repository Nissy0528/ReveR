using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L_TimeCntEffect : MonoBehaviour
{
    public float speed;

    private float time;
    private float range;
    private Text text;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        Debug.Log(range);
    }

    // Update is called once per frame
    void Update()
    {
        Count();
    }

    /// <summary>
    /// カウント
    /// </summary>
    private void Count()
    {
        time = Mathf.Lerp(time, range, Time.deltaTime * speed);
        text.text = time.ToString();
    }

    /// <summary>
    /// カウントエフェクトの設定
    /// </summary>
    /// <param name="time"></param>
    /// <param name="type"></param>
    public void SetTime(float time, float range)
    {
        this.time = time;
        this.range = range;
    }
}
