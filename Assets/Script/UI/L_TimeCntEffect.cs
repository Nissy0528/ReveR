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
    private GameObject child;
    private Main main;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        main = GameObject.Find("MainManager").GetComponent<Main>();
        if (transform.childCount > 0)
        {
            child = transform.GetChild(0).gameObject;
            child.transform.parent = transform.parent;
        }
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
        time = Mathf.Clamp(time, 0.0f, main.lifeTimeMax);
        text.text = time.ToString();
        if (child != null)
        {
            child.GetComponent<Text>().text = time.ToString();
        }

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
