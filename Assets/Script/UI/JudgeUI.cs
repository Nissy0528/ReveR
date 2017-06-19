using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgeUI : MonoBehaviour
{
    private Animator anim;
    private RectTransform rect;
    private GameObject target;
    private Vector3 TargetPos;
    private float Alpha = 0;
    private bool IsStart = false;

    public float time;
    public float speed;
    

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rect = GetComponent<RectTransform>();
      
        rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, TargetPos);

        if(GetComponent<Text>().material.name== "EVATime Material")
        {
            GetComponent<Text>().material.color = new Color(1, 1, 1, Alpha);
        }
    }

    // Update is called once per frame
    void Update()
    {
        TEXT();
    }

    /// <summary>
    /// 消滅処理
    /// </summary>
    private void Dead()
    {
        var animState = anim.GetCurrentAnimatorStateInfo(0);
        if (animState.normalizedTime >= 1)
        {
            Destroy(gameObject);
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
            GetComponent<Text>().material.color = new Color(1, 1, 1, Alpha);
            if (IsStart) time--;
            rect.position += new Vector3(0, 0.1f);
            FadeIn();
            if (time <= 0) FadeOut();
        }
        else
        {
            Dead();//消滅処理
        }
    }

    public void FadeIn()
    {
        if (Alpha < 1f && !IsStart) Alpha += 0.01f * speed;
        else if (Alpha >= 1f)
        {
            IsStart = true;
        }
           


    }
    public void FadeOut()
    {
        if (Alpha <= 0&& IsStart == true)
        {
            Destroy(gameObject);
        }
        Alpha -= 0.01f * speed;
    }
}
