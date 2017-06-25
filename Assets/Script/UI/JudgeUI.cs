using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgeUI : MonoBehaviour
{
    private Animator anim;
    private RectTransform rect;
    private GameObject target;
    private GameObject camera;
    private GameObject lineObj;
    private GameObject timeObj;
    private Vector3 TargetPos;
    private float Alpha = 0;
    private float scrollSpeed;
    private bool IsStart = false;
    private bool isScroll = false;

    public float time;
    public float speed;

    private Color color;//中山追記

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rect = GetComponent<RectTransform>();
        camera = GameObject.Find("Main Camera");

        rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, TargetPos);

        //if(GetComponent<Text>().material.name== "EVATime Material")
        //{
        //    GetComponent<Text>().material.color = new Color(1, 1, 1, Alpha);
        //}

        if (gameObject.tag == "EVATime")
        {
            color = gameObject.GetComponent<Text>().material.color;//中山追記
        }
    }

    // Update is called once per frame
    void Update()
    {
        TEXT();
        Scroll();
    }

    /// <summary>
    /// 消滅処理
    /// </summary>
    private void Dead()
    {
        var animState = anim.GetCurrentAnimatorStateInfo(0);
        if (animState.normalizedTime >= 1)
        {
            timeObj.transform.parent = transform;
            isScroll = true;
        }
    }

    public void SetTargetPosition(Vector3 Pos)
    {
        TargetPos = Pos;
    }

    public void TEXT()
    {
        if (gameObject.tag == "EVATime")
        {
            //GetComponent<Text>().material.color = new Color(1,1,1, Alpha);
            GetComponent<Text>().color = new Color(color.r, color.g, color.b, Alpha);//中山一部変更

            if (IsStart) time--;
            rect.position += new Vector3(0, 0.1f);
            FadeIn();
            //if (time <= 0) FadeOut();
        }
        else
        {
            Dead();//消滅処理
        }
    }

    private void FadeIn()
    {
        if (Alpha < 1f && !IsStart) Alpha += 0.01f * speed;
        else if (Alpha >= 1f)
        {
            IsStart = true;
        }
    }

    private void FadeOut()
    {
        if (Alpha <= 0 && IsStart == true)
        {
            Destroy(gameObject);
        }
        Alpha -= 0.01f * speed;
    }

    /// <summary>
    /// 下にスクロール移動
    /// </summary>
    private void Scroll()
    {
        if (!isScroll) return;

        //下にスクロール
        if (lineObj != null)
        {
            lineObj.transform.Translate(0, -scrollSpeed * Time.timeScale, 0, Space.World);
            rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, lineObj.transform.position);
        }
        else
        {
            rect.Translate(0, -scrollSpeed * Time.timeScale, 0, Space.World);
        }

        //画面下に出たら消滅
        if (transform.position.y < camera.GetComponent<Camera>().ScreenToWorldPoint(Vector3.zero).y - transform.lossyScale.y / 2)
        {
            Destroy(lineObj);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 下移動速度設定
    /// </summary>
    /// <param name="scrollSpeed"></param>
    public void SetScroll(float scrollSpeed, GameObject lineObj, GameObject timeObj)
    {
        this.scrollSpeed = scrollSpeed;
        this.lineObj = lineObj;
        this.timeObj = timeObj;
    }
}
